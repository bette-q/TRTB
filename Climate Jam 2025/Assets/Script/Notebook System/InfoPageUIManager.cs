using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class InfoPageUIManager : MonoBehaviour
{
    [Header("Grid List")]
    public Transform gridContent; // Parent with Grid Layout Group
    public GameObject infoBlockItemPrefab;

    [Header("Paging Controls")]
    public Button prevButton;
    public Button nextButton;

    [Header("Detail Panel")]
    public GameObject detailPanel;
    public Image detailIcon;
    public TMP_Text detailTitle;
    public TMP_Text detailDesc;

    private List<EvidenceBlock> infoBlocks = new List<EvidenceBlock>();
    private int currentPage = 0;
    private const int blocksPerPage = 8;

    // Call this when switching to Info tab!
    public void SetBlocks(List<EvidenceBlock> allBlocks)
    {
        // Filter: show all except FinalCombo
        infoBlocks = allBlocks
            .Where(b => b.blockType != EvidenceBlockType.FinalCombo)
            .OrderBy(b => b.missionFinished) // Optional: unfinished first
            .ToList();

        currentPage = 0;
        RefreshGrid();
        ShowDetails(null); // Clear detail by default
    }

    void Start()
    {
        prevButton.onClick.AddListener(OnPrevPage);
        nextButton.onClick.AddListener(OnNextPage);
    }

    void RefreshGrid()
    {
        foreach (Transform child in gridContent)
            Destroy(child.gameObject);

        int start = currentPage * blocksPerPage;
        int end = Mathf.Min(start + blocksPerPage, infoBlocks.Count);

        for (int i = start; i < end; ++i)
        {
            var block = infoBlocks[i];
            var go = Instantiate(infoBlockItemPrefab, gridContent);
            var item = go.GetComponent<InfoBlockItemUI>();
            item.Setup(block, OnSelectBlock);

            // Optional: visually dim if missionFinished
            var cg = go.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.alpha = block.missionFinished ? 0.4f : 1.0f;
        }

        prevButton.interactable = currentPage > 0;
        nextButton.interactable = end < infoBlocks.Count;
    }

    void OnPrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshGrid();
        }
    }

    void OnNextPage()
    {
        if ((currentPage + 1) * blocksPerPage < infoBlocks.Count)
        {
            currentPage++;
            RefreshGrid();
        }
    }

    void OnSelectBlock(EvidenceBlock block)
    {
        ShowDetails(block);
    }

    void ShowDetails(EvidenceBlock block)
    {
        if (block == null)
        {
            detailPanel.SetActive(false);
            return;
        }
        detailPanel.SetActive(true);
        detailIcon.sprite = block.info.icon;
        detailTitle.text = block.info.title;
        detailDesc.text = block.info.text;
    }
}
