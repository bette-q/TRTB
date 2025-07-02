using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

//handles entire notebook ui panel + deduction panel card placement
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

    // Track spawned cards inside deduction panel by evidence ID
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

        // Update evidence list UI with current blocks
        evidenceListPanelManager.RefreshEvidenceList(GameStateManager.Instance.GetAvailableBlocks());
    }

    public void Close()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
    }

    public void AddNewBlock(EvidenceBlock block, Vector2 localDropPos)
    {
        if (spawnedCardDict.ContainsKey(block.id))
            return;

        var go = Instantiate(evidenceBlockPrefab, blockParentPanel);
        go.GetComponent<FreeDragBlock>().Init(blockParentPanel);
        go.GetComponent<CardLinkHandler>().Init(cardPanelLinkManager, block);

        go.transform.Find("Title").GetComponent<TMP_Text>().text = block.info.title;

        var btn = go.GetComponent<Button>();
        btn.onClick.AddListener(() => descriptionBox.text = block.info.text);

        if (block.blockType != EvidenceBlockType.Evidence)
            go.GetComponent<Image>().color = Color.cyan;

        var rect = (RectTransform)go.transform;
        rect.anchoredPosition = localDropPos; // Place at drop position!

        spawnedCardDict.Add(block.id, go);
    }


    public void OnComboCreated(ComboData comboBlock, EvidenceBlockType type)
    {
        // Remove used EBs from combo panel and evidence list panel
        RemoveBlocksByIds(comboBlock.comboOrder); 
        GameStateManager.Instance.RemoveBlocksByIds(comboBlock.comboOrder);
        evidenceListPanelManager.RemoveBlocksByIds(comboBlock.comboOrder); 

        var newBlock = new EvidenceBlock(comboBlock.resultEvidence, type);

        GameStateManager.Instance.AddBlock(newBlock);
        evidenceListPanelManager.AddBlock(newBlock); // add to evidence list panel
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
