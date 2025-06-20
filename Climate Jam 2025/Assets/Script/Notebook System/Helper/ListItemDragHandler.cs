using UnityEngine;
using UnityEngine.EventSystems;

//make list item draggable
public class ListItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public System.Func<bool> canDragCheck;
    public EvidenceBlock myBlock; // Assign when instantiating
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDragCheck != null && !canDragCheck())
        {
            Debug.LogWarning("block already exist");
            return;
        }
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false; // So drop target can receive events
        canvasGroup.alpha = 0.6f; // Optional: make it look ¡°dragged¡±
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetCanvasScaleFactor();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = originalPosition; // Snap back to list

        // If dropped over ComboPanel, ComboPanel will handle creating the card
    }

    // Helper for correct screen scaling
    private float GetCanvasScaleFactor()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        return canvas ? canvas.scaleFactor : 1f;
    }
}
