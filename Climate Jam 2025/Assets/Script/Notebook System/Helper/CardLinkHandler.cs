using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CardLinkHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerClickHandler
{
    public RectTransform linkHandle;       // Assign in Inspector
    public CardPanelLinkManager linkManager; // Assign in Inspector or Find at Start
    public EvidenceBlock myBlock;

    private bool isLinking = false;

    public void Init(CardPanelLinkManager manager, EvidenceBlock block)
    {
        linkManager = manager;
        myBlock = block;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isLinking = RectTransformUtility.RectangleContainsScreenPoint(linkHandle, eventData.position, null);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            linkManager.RemoveOutgoingLinks(this);
        }
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
            if (Input.GetMouseButtonDown(1)) // Right mouse
            {
                linkManager.EndTempLine();
                isLinking = false;
            }
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLinking)
        {
            linkManager.EndTempLine();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var hit in results)
            {
                // Look for CardLinkHandler that is NOT yourself
                var target = hit.gameObject.GetComponent<CardLinkHandler>();
                if (target != null && target != this)
                {
                    linkManager.RegisterLink(this, target);
                    break; // Only register once!
                }
            }
        }
        isLinking = false;
    }
}
