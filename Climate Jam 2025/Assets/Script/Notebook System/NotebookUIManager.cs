using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

//handles entire notebook ui panel
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
    public EvidenceListPanelManager evidenceListPanelManager;

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

        // Update evidence list UI with current blocks
        evidenceListPanelManager.RefreshEvidenceList(GameStateManager.Instance.GetAvailableBlocks());
    }

    public void Close()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AddNewBlock(EvidenceBlock block, Vector2 localDropPos)
    {
        if (spawnedCardDict.ContainsKey(block.id))
            return;

        var go = Instantiate(evidenceBlockPrefab, blockParentPanel);
        go.GetComponent<FreeDragBlock>().Init(blockParentPanel);
        go.GetComponent<CardLinkHandler>().Init(cardPanelLinkManager, block);

        go.transform.Find("Title").GetComponent<TMP_Text>().text = block.title;

        var btn = go.GetComponent<Button>();
        btn.onClick.AddListener(() => descriptionBox.text = block.text);

        if (block.blockType != EvidenceBlockType.Evidence)
            go.GetComponent<Image>().color = Color.cyan;

        var rect = (RectTransform)go.transform;
        rect.anchoredPosition = localDropPos; // Place at drop position!

        spawnedCardDict.Add(block.id, go);
    }


    public void OnComboCreated(ComboBlock comboBlock)
    {
        // Remove used EBs from combo panel and evidence list panel
        RemoveBlocksByIds(comboBlock.comboOrder); // for combo panel
        GameStateManager.Instance.RemoveBlocksByIds(comboBlock.comboOrder);

        evidenceListPanelManager.RemoveBlocksByIds(comboBlock.comboOrder); // for evidence list panel

        // Add new CB to both GSM and evidence list panel
        var deductionEB = InteractEvidence.GenerateEvidenceBlock(
            comboBlock.resultEvidence, GameStateManager.Instance.currentCharacterID);
        deductionEB.blockType = EvidenceBlockType.ComboBlock;
        GameStateManager.Instance.AddBlock(deductionEB);

        evidenceListPanelManager.AddBlock(deductionEB); // add to evidence list panel
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

    public bool HasBlockInComboPanel(string id)
    {
        return spawnedCardDict.ContainsKey(id);
    }

}
