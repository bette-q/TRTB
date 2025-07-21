using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class InfoReportUIManager : MonoBehaviour
{
    [Header("Grid List")]
    public Transform gridContent; // The parent object with Grid Layout Group
    public GameObject blockItemPrefab;

    [Header("Paging Controls")]
    public Button prevButton;
    public Button nextButton;

    [Header("Detail Panel")]
    public GameObject detailPanel;
    public Image detailIcon;
    public TMP_Text detailTitle;
    public TMP_Text detailDesc;

    [Header("Block Types To Display")]
    public List<EvidenceBlockType> allowedTypes;

    [Header("Visual Options")]
    public bool dimFinishedBlocks = true;

    private List<EvidenceBlock> filteredBlocks = new List<EvidenceBlock>();
    private int currentPage = 0;
    private const int blocksPerPage = 8;

    public void SetBlocks(IReadOnlyList<EvidenceBlock> allBlocks)
    {
        filteredBlocks = allBlocks
            .Where(b => allowedTypes.Contains(b.blockType))
            .OrderBy(b => b.missionFinished) // Optional: unfinished first
            .ToList();

        currentPage = 0;
        RefreshGrid();
        ShowDetails(null); // Hide detail by default
    }

    //void Start()
    //{
    //    prevButton.onClick.AddListener(OnPrevPage);
    //    nextButton.onClick.AddListener(OnNextPage);
    //}

    //private void OnEnable()
    //{
    //    prevButton.onClick.AddListener(OnPrevPage);
    //    nextButton.onClick.AddListener(OnNextPage);
    //}

    //private void OnDisable()
    //{
    //    prevButton.onClick.RemoveAllListeners();
    //    nextButton.onClick.RemoveAllListeners();
    //}

    void RefreshGrid()
    {
        foreach (Transform child in gridContent)
            Destroy(child.gameObject);

        int start = currentPage * blocksPerPage;
        int end = Mathf.Min(start + blocksPerPage, filteredBlocks.Count);

        for (int i = start; i < end; ++i)
        {
            var block = filteredBlocks[i];
            var go = Instantiate(blockItemPrefab, gridContent);
            var item = go.GetComponent<InfoBlockItemUI>();
            item.Setup(block, OnSelectBlock);

            // Optional: visually dim if missionFinished and option is on
            var cg = go.GetComponent<CanvasGroup>();
            if (cg != null && dimFinishedBlocks)
                cg.alpha = block.missionFinished ? 0.4f : 1.0f;
        }

        prevButton.interactable = currentPage > 0;
        nextButton.interactable = end < filteredBlocks.Count;
    }

    public void OnPrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshGrid();
        }
    }

    public void OnNextPage()
    {
        if ((currentPage + 1) * blocksPerPage < filteredBlocks.Count)
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
