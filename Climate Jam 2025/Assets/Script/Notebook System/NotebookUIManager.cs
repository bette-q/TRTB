using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NotebookUIManager : MonoBehaviour
{
    [HideInInspector]
    public static bool IsOpen { get; private set; }

    public GameObject notebookPanel;
    public RectTransform blockParentPanel;
    public RectTransform blockGridPanel;
    public GameObject evidenceBlockPrefab;
    public TMP_Text descriptionBox;
    public KeyCode toggleKey = KeyCode.N;

    public CardPanelLinkManager cardPanelLinkManager;

    // Track spawned cards by evidence ID
    private Dictionary<string, GameObject> spawnedCardDict = new Dictionary<string, GameObject>();

    void Start()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!notebookPanel.activeSelf)
                Open();
            else
                Close();
        }
    }

    public void Open()
    {
        notebookPanel.SetActive(true);
        IsOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AddNewBlocksIfAny();
    }

    public void Close()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void AddNewBlocksIfAny()
    {
        var blocks = GameStateManager.Instance.GetAvailableBlocks();
        foreach (var block in blocks)
        {
            if (!spawnedCardDict.ContainsKey(block.id))
                AddNewBlock(block);
        }
    }

    public void AddNewBlock(EvidenceBlock block)
    {
        var go = Instantiate(evidenceBlockPrefab, blockParentPanel);
        go.GetComponent<FreeDragBlock>().Init(blockParentPanel);
        go.GetComponent<CardLinkHandler>().Init(cardPanelLinkManager);

        go.transform.Find("Title").GetComponent<TMP_Text>().text = block.title;

        var btn = go.GetComponent<Button>();
        btn.onClick.AddListener(() => descriptionBox.text = block.text);

        // ComboBlock visuals
        if (block.blockType != EvidenceBlockType.Evidence)
            go.GetComponent<Image>().color = Color.cyan;

        spawnedCardDict.Add(block.id, go);
    }

    public void OnComboCreated(ComboBlock comboBlock)
    {
        // 1. Calculate average anchored position of all used EBs
        Vector2 avgPosition = Vector2.zero;
        int count = 0;
        foreach (var id in comboBlock.comboOrder)
        {
            if (spawnedCardDict.TryGetValue(id, out var go))
            {
                avgPosition += ((RectTransform)go.transform).anchoredPosition;
                count++;
            }
        }
        if (count > 0) avgPosition /= count;

        // 2. Remove used EBs (UI and GSM)
        RemoveBlocksByIds(comboBlock.comboOrder);
        GameStateManager.Instance.RemoveBlocksByIds(comboBlock.comboOrder);

        // 3. Create and add the new CB using AddNewBlock
        var deductionEB = InteractEvidence.GenerateEvidenceBlock(
            comboBlock.resultEvidence, GameStateManager.Instance.currentCharacterID);
        deductionEB.blockType = EvidenceBlockType.ComboBlock;
        GameStateManager.Instance.AddBlock(deductionEB);

        AddNewBlock(deductionEB);

        // 4. Set CB position (get from dict immediately after adding)
        if (spawnedCardDict.TryGetValue(deductionEB.id, out var cbGO))
        {
            ((RectTransform)cbGO.transform).anchoredPosition = avgPosition;
        }
    }

    public void RemoveBlocksByIds(List<string> ids)
    {
        foreach (var id in ids)
        {
            if (spawnedCardDict.TryGetValue(id, out var go))
            {
                Destroy(go);
                spawnedCardDict.Remove(id);
            }
        }
    }
}
