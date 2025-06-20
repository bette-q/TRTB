using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// make list item draggable
public class ListItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public EvidenceBlock myBlock;
    public System.Func<bool> canDragCheck;
    public GameObject dragGhostPrefab; // Assign your evidence block prefab or a ghost version in inspector

    private GameObject dragGhost;
    private RectTransform dragGhostRect;
    private RectTransform notebookPanelRect;
    private Canvas rootCanvas;

    public void Init(
    EvidenceBlock block,
    System.Func<bool> canDragCheck,
    RectTransform notebookPanelRect)
    {
        this.myBlock = block;
        this.canDragCheck = canDragCheck;
        this.notebookPanelRect = notebookPanelRect;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDragCheck != null && !canDragCheck())
            return;

        // Instantiate ghost card and parent it to the root canvas (so it appears on top)
        dragGhost = Instantiate(dragGhostPrefab, notebookPanelRect);
        dragGhostRect = dragGhost.GetComponent<RectTransform>();

        // Set ghost visuals
        dragGhost.transform.Find("Title").GetComponent<TMP_Text>().text = myBlock.title;
        var cg = dragGhost.GetComponent<CanvasGroup>();
        if (cg == null) cg = dragGhost.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.alpha = 0.8f; // Make it a bit transparent
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragGhostRect != null && notebookPanelRect != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                notebookPanelRect,                     // <-- now using the notebook panel!
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);

            // Clamp within the panel bounds
            Vector2 min = notebookPanelRect.rect.min + dragGhostRect.rect.size * dragGhostRect.pivot;
            Vector2 max = notebookPanelRect.rect.max - dragGhostRect.rect.size * (Vector2.one - dragGhostRect.pivot);

            localPoint.x = Mathf.Clamp(localPoint.x, min.x, max.x);
            localPoint.y = Mathf.Clamp(localPoint.y, min.y, max.y);

            dragGhostRect.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragGhost != null)
        {
            Destroy(dragGhost);
            dragGhost = null;
            dragGhostRect = null;
        }
    }
}
