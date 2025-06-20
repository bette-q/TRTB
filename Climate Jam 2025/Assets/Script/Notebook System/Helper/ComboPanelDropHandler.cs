using UnityEngine;
using UnityEngine.EventSystems;

//handles list item drops on combo panel
public class ComboPanelDropHandler : MonoBehaviour, IDropHandler
{
    public NotebookUIManager notebookUI;

    public void OnDrop(PointerEventData eventData)
    {
        var dragHandler = eventData.pointerDrag?.GetComponent<ListItemDragHandler>();
        if (dragHandler != null)
        {
            // Convert screen position to local position in combo panel
            RectTransform panelRect = (RectTransform)transform;
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRect, eventData.position, eventData.pressEventCamera, out localPoint);

            notebookUI.AddNewBlock(dragHandler.myBlock, localPoint);
        }
    }

}
