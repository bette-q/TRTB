using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// Handles line drawing, linking, and combo chain logic
public class CardPanelLinkManager : MonoBehaviour
{
    public RectTransform linesOverlay;
    public GameObject linePrefab;
    public DeductionUIManager deductionUIMan;

    // Tuple: (from, to, lineGO)
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

    // Register a new link and check for combos
    public void RegisterLink(CardLinkHandler from, CardLinkHandler to)
    {
        // 1. Prevent linking between different block types
        if (from.myBlock.blockType != to.myBlock.blockType)
            return;

        // 2. Prevent duplicate or reverse links
        foreach (var l in links)
            if ((l.Item1 == from && l.Item2 == to) || (l.Item1 == to && l.Item2 == from))
                return;

        // 3. Prevent multiple incoming (forks)
        foreach (var l in links)
        {
            if (l.Item2 == to)
                return;
        }

        // 4. Prevent cycles (can't link to block in your own chain)
        var chain = GetChainFromHead(from);
        if (chain.Contains(to))
            return;

        // Remove existing outgoing link -> can have at most one outgoing
        for (int i = links.Count - 1; i >= 0; i--)
        {
            var (f, t, oldLine) = links[i];
            if (f == from)
            {
                Destroy(oldLine);
                links.RemoveAt(i);
                break;
            }
        }

        // 5. Add new link and redraw
        var line = Instantiate(linePrefab, linesOverlay);
        links.Add((from, to, line));
        RedrawAllLines();

        // 6. Find full chain from head (always linear)
        chain = GetChainFromHead(from);

        // 7. Build ID list for combo check
        List<string> selectedIDs = new List<string>();
        foreach (var handler in chain)
            selectedIDs.Add(handler.myBlock.info.id);

        // 8. Determine which combo list to check
        var type = from.myBlock.blockType;
        var combo = ComboManager.Instance.FindValidCombo(selectedIDs, type);
        if (combo != null)
        {
            deductionUIMan.OnComboCreated(combo,
                type == EvidenceBlockType.Evidence ? EvidenceBlockType.SecCombo : EvidenceBlockType.FinalCombo);

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

    // Remove a specific link and redraw
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

    // Remove all outgoing links from a block
    public void RemoveOutgoingLinks(CardLinkHandler block)
    {
        for (int i = links.Count - 1; i >= 0; i--)
        {
            var (from, to, line) = links[i];
            if (from == block)
            {
                Destroy(line);
                links.RemoveAt(i);

                // Only one outgoing possible, so break.
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
        Vector2 dir = to - from;
        float length = dir.magnitude;
        rect.position = from;
        rect.sizeDelta = new Vector2(length, 3);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Returns the linear chain from the head node containing the given block
    public List<CardLinkHandler> GetChainFromHead(CardLinkHandler anyBlock)
    {
        // Go to head
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

        // Traverse forward
        List<CardLinkHandler> chain = new List<CardLinkHandler>();
        var current = head;
        var visited = new HashSet<CardLinkHandler>();
        while (current != null)
        {
            if (!visited.Add(current))
                break; // Cycle safety
            chain.Add(current);
            var nextLink = links.Find(l => l.Item1 == current);
            if (nextLink == default) break;
            current = nextLink.Item2;
        }
        return chain;
    }
}
