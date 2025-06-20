using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
        // Prevent duplicates
        foreach (var l in links)
            if ((l.Item1 == from && l.Item2 == to) || (l.Item1 == to && l.Item2 == from)) return;

        var line = Instantiate(linePrefab, linesOverlay);
        links.Add((from, to, line));
        RedrawAllLines();
        // TODO: Call combo logic here if needed!
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
}
