using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

// handles scroll bar for evidence list
public class EvidenceListPanelManager : MonoBehaviour
{
    public GameObject evidenceListItemPrefab; // Assign your EvidenceListItem prefab
    public Transform contentParent;           // Assign EvidenceListPanel/Viewport/Content here
    public TMP_Text descriptionBox;           // Assign your Description box
    public RectTransform notebookPanelRect;
    public DeductionUIManager deductionUIMan; // Assign in Inspector

    // To keep track of current entries
    private Dictionary<string, GameObject> evidenceItemDict = new Dictionary<string, GameObject>();

    // Call this to (re)populate the list when notebook opens or new blocks are added
    public void RefreshEvidenceList(IReadOnlyList<EvidenceBlock> allBlocks)
    {
        // 1. Clear old items (for now¡ªcan optimize later for partial updates)
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
        evidenceItemDict.Clear();

        // 2. Create one item per EB/CB in list
        foreach (var block in allBlocks)
        {
            if (block.blockType != EvidenceBlockType.Evidence && block.blockType != EvidenceBlockType.SecCombo)
                continue;

            var go = Instantiate(evidenceListItemPrefab, contentParent);
            go.transform.Find("Text").GetComponent<TMP_Text>().text = block.info.text;

            // Set up click to show description
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                descriptionBox.text = block.info.text;
            });

            // Set up drag handler with duplicate check
            var dragHandler = go.GetComponent<ListItemDragHandler>();
            dragHandler.Init(
                block,
                () => !deductionUIMan.HasBlockInComboPanel(block.id),
                notebookPanelRect
            );

            evidenceItemDict.Add(block.id, go);
        }
    }

    // For when a CB is created and EBs need to be removed from the list
    public void RemoveBlocksByIds(List<string> ids)
    {
        foreach (var id in ids)
        {
            if (evidenceItemDict.TryGetValue(id, out var go))
            {
                Destroy(go);
                evidenceItemDict.Remove(id);
            }
        }
    }

    // For when a new EB or CB is added
    public void AddBlock(EvidenceBlock block)
    {
        if (block.blockType != EvidenceBlockType.Evidence && block.blockType != EvidenceBlockType.SecCombo)
            return;

        if (evidenceItemDict.ContainsKey(block.id))
            return;

        var go = Instantiate(evidenceListItemPrefab, contentParent);
        go.transform.Find("Text").GetComponent<TMP_Text>().text = block.info.title;

        go.GetComponent<Button>().onClick.AddListener(() => descriptionBox.text = block.info.text);

        // Assign the EvidenceBlock to the drag handler
        var dragHandler = go.GetComponent<ListItemDragHandler>();
        dragHandler.Init(
            block,
            () => !deductionUIMan.HasBlockInComboPanel(block.id),
            notebookPanelRect
        );

        evidenceItemDict.Add(block.id, go);
    }
}
