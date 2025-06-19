using UnityEngine;
using UnityEngine.EventSystems;

public class CardLinkHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform linkHandle;       // Assign in Inspector
    public CardPanelLinkManager linkManager; // Assign in Inspector or Find at Start
    private bool isLinking = false;

    public void Init(CardPanelLinkManager manager)
    {
        linkManager = manager;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isLinking = RectTransformUtility.RectangleContainsScreenPoint(linkHandle, eventData.position, null);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLinking)
        {
            linkManager.BeginTempLine(linkHandle.position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLinking)
        {
            linkManager.UpdateTempLine(linkHandle.position, eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLinking)
        {
            linkManager.EndTempLine();
            // Add code here to detect link target and register if needed
        }
        isLinking = false;
    }
}
