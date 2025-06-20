using UnityEngine;
using UnityEngine.EventSystems;

public class LinkLineRemover : MonoBehaviour, IPointerClickHandler
{
    public CardPanelLinkManager linkManager;
    public CardLinkHandler from;
    public CardLinkHandler to;

    public void Init(CardPanelLinkManager man, CardLinkHandler from_in, CardLinkHandler to_in)
    {
        linkManager = man;
        from = from_in;
        to = to_in;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            linkManager.RemoveLink(from, to);
        }
    }
}
