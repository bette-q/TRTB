using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

//handles line drawing and linking
public class CardPanelLinkManager : MonoBehaviour
{
    public RectTransform linesOverlay; // Fullscreen overlay for lines (UI canvas)
    public GameObject linePrefab;      // Simple prefab with UI LineRenderer/Image

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
            var (f, t, l) = links[i];
            if (f == from)
            {
                Destroy(l);
                links.RemoveAt(i);
            }
        }

        var line = Instantiate(linePrefab, linesOverlay);

        links.Add((from, to, line));
        RedrawAllLines();
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


}
