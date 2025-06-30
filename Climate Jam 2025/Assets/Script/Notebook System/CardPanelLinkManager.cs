using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

//handles line drawing and linking
public class CardPanelLinkManager : MonoBehaviour
{
    public RectTransform linesOverlay; // Fullscreen overlay for lines (UI canvas)
    public GameObject linePrefab;      // Simple prefab with UI LineRenderer/Image
    public NotebookUIManager notebookUIManager;

    private List<(CardLinkHandler, CardLinkHandler, GameObject)> links = new();
    private GameObject tempLine;

    public void BeginTempLine(Vector2 from)
    {
        tempLine = Instantiate(linePrefab, linesOverlay);
        UpdateTempLine(from, from);
    }

    public void UpdateTempLine(Vector2 from, Vector2 to)
    {
        if (tempLine == null) return;
        DrawUILine(tempLine.GetComponent<RectTransform>(), from, to);
    }

    public void EndTempLine()
    {
        if (tempLine != null) Destroy(tempLine);
    }

    public void RegisterLink(CardLinkHandler from, CardLinkHandler to)
    {
        // Block duplicate or reverse (b->a if a->b exists)
        foreach (var l in links)
            if ((l.Item1 == from && l.Item2 == to) || (l.Item1 == to && l.Item2 == from))
                return;

        // Remove any existing outgoing link from 'from'
        for (int i = links.Count - 1; i >= 0; i--)
        {
            var (f, t, oldLine) = links[i];
            if (f == from)
            {
                Destroy(oldLine);
                links.RemoveAt(i);
            }
        }

        // Add the new link
        var line = Instantiate(linePrefab, linesOverlay);
        links.Add((from, to, line));
        RedrawAllLines();

        // 1. Traverse to chain head (block with no incoming links)
        CardLinkHandler head = from;
        bool foundHead;
        do
        {
            foundHead = false;
            foreach (var l in links)
            {
                if (l.Item2 == head)
                {
                    head = l.Item1;
                    foundHead = true;
                    break;
                }
            }
        } while (foundHead);

        // 2. Traverse forward to build the whole chain
        List<CardLinkHandler> chain = new List<CardLinkHandler>();
        var visited = new HashSet<CardLinkHandler>();
        var current = head;
        while (current != null)
        {
            if (visited.Contains(current))
                break; // Cycle detected, abort traversal
            visited.Add(current);
            chain.Add(current);

            var nextLink = links.Find(l => l.Item1 == current);
            if (nextLink == default) break;
            current = nextLink.Item2;
        }

        // 3. Build the ID list
        List<string> selectedIDs = new List<string>();
        foreach (var handler in chain)
            selectedIDs.Add(handler.myBlock.id);

        // 4. Check for a matching combo
        var combo = ComboManager.Instance.FindValidCombo(selectedIDs);
        if (combo != null)
        {
            notebookUIManager.OnComboCreated(combo);

            // Remove all involved links/lines in the chain
            for (int i = links.Count - 1; i >= 0; i--)
            {
                if (chain.Contains(links[i].Item1) || chain.Contains(links[i].Item2))
                {
                    Destroy(links[i].Item3);
                    links.RemoveAt(i);
                }
            }
        }
    }



    public void RemoveLink(CardLinkHandler from, CardLinkHandler to)
    {
        for (int i = 0; i < links.Count; i++)
        {
            var (f, t, line) = links[i];
            if ((f == from && t == to) || (f == to && t == from))
            {
                Destroy(line);
                links.RemoveAt(i);
                break;
            }
        }
    }

    void Update()
    {
        RedrawAllLines();
    }

    void RedrawAllLines()
    {
        foreach (var (from, to, line) in links)
        {
            DrawUILine(line.GetComponent<RectTransform>(), from.linkHandle.position, to.linkHandle.position);
        }
    }

    void DrawUILine(RectTransform rect, Vector2 from, Vector2 to)
    {
        // Quick/dirty UI line: reposition, rotate, stretch an Image between from/to
        Vector2 dir = to - from;
        float length = dir.magnitude;
        rect.position = from;
        rect.sizeDelta = new Vector2(length, 3); // 3px thick
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle);
    }
        public void RemoveOutgoingLinks(CardLinkHandler block)
    {
        // Remove only links where block is the FROM node (outgoing)
        for (int i = links.Count - 1; i >= 0; i--)
        {
            var (from, to, line) = links[i];
            if (from == block)
            {
                Destroy(line);
                links.RemoveAt(i);
                Debug.Log("Removed outgoing line from: " + block.name);
            }
        }
    }

    public List<CardLinkHandler> GetChainFromHead(CardLinkHandler anyBlock)
    {
        // Step 1: Find head (block with no incoming links)
        CardLinkHandler head = anyBlock;
        bool foundHead;
        do
        {
            foundHead = false;
            foreach (var l in links)
            {
                if (l.Item2 == head)
                {
                    head = l.Item1;
                    foundHead = true;
                    break;
                }
            }
        } while (foundHead);

        // Step 2: Traverse forward through outgoing links
        List<CardLinkHandler> chain = new List<CardLinkHandler>();
        var current = head;
        while (current != null)
        {
            chain.Add(current);
            var nextLink = links.Find(l => l.Item1 == current);
            if (nextLink == default) break;
            current = nextLink.Item2;
            // Safety: Stop if we hit a cycle
            if (chain.Contains(current)) break;
        }
        return chain;
    }



}
