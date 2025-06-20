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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panelRect,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint
            );

            // --- Clamp localPoint to panel bounds ---
            // Get the prefab's rect for proper clamping
            RectTransform ghostRect = dragHandler.dragGhostPrefab.GetComponent<RectTransform>();

            // If for any reason ghostRect is null (shouldn't be), set size as zero
            Vector2 cardSize = ghostRect ? ghostRect.rect.size : Vector2.zero;
            Vector2 cardPivot = ghostRect ? ghostRect.pivot : Vector2.one * 0.5f;

            Vector2 min = panelRect.rect.min + cardSize * cardPivot;
            Vector2 max = panelRect.rect.max - cardSize * (Vector2.one - cardPivot);

            localPoint.x = Mathf.Clamp(localPoint.x, min.x, max.x);
            localPoint.y = Mathf.Clamp(localPoint.y, min.y, max.y);
            // --- End Clamp ---

            notebookUI.AddNewBlock(dragHandler.myBlock, localPoint);
        }
    }


}
