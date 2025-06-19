using UnityEngine;
using UnityEngine.EventSystems;

public class FreeDragBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform dragHandle;      // Assign in Inspector
    [HideInInspector] public RectTransform blockPanelRect; // Assign on spawn
    private bool canDrag = false;
    private RectTransform rect;
    private Canvas canvas;
    private Vector2 pointerOffset;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    public void Init(RectTransform panelRect)
    {
        blockPanelRect = panelRect;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        bool contains = RectTransformUtility.RectangleContainsScreenPoint(dragHandle, eventData.position, null);
        canDrag = contains;
        if (canDrag)
        {
            rect.SetAsLastSibling();

            // Get offset from pointer to pivot
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, canvas.worldCamera, out pointerOffset);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        // Store the current world position
        Vector3 worldPos = rect.position;

        // Re-parent to blockPanelRect if not already
        if (rect.parent != blockPanelRect)
        {
            rect.SetParent(blockPanelRect, true); // Keep world position
            // Now set the anchoredPosition to match the old world position, but in the new parent's space
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                blockPanelRect, worldPos, canvas.worldCamera, out localPoint);
            rect.anchoredPosition = localPoint;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(blockPanelRect, eventData.position, canvas.worldCamera, out localPoint);

        // Adjust by pointer offset
        localPoint -= pointerOffset;

        // Clamp to bounds as before
        Vector2 min = blockPanelRect.rect.min + rect.rect.size * rect.pivot;
        Vector2 max = blockPanelRect.rect.max - rect.rect.size * (Vector2.one - rect.pivot);
        localPoint.x = Mathf.Clamp(localPoint.x, min.x, max.x);
        localPoint.y = Mathf.Clamp(localPoint.y, min.y, max.y);

        rect.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canDrag = false;
    }
}
