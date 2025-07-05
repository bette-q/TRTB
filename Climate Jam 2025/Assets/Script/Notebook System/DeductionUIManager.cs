using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

//handles deduction panel card placement
public class DeductionUIManager : MonoBehaviour
{
    //Deduction Page
    public GameObject deductionPagePanel;
    public RectTransform ComboCardParentPanel;
    public GameObject evidenceBlockPrefab;
    public TMP_Text DeductionText;
    public CardPanelLinkManager cardPanelLinkManager;
    public EvidenceListPanelManager evidenceListPanelManager;

    // Track spawned cards inside deduction panel by evidence ID
    private Dictionary<string, GameObject> spawnedCardDict = new Dictionary<string, GameObject>();

    public void RefreshPage()
    {
        evidenceListPanelManager.RefreshEvidenceList(GameStateManager.Instance.GetAvailableBlocks());
    }

    public void AddNewBlock(EvidenceBlock block, Vector2 localDropPos)
    {
        if (spawnedCardDict.ContainsKey(block.id))
            return;

        var go = Instantiate(evidenceBlockPrefab, ComboCardParentPanel);
        go.GetComponent<FreeDragBlock>().Init(ComboCardParentPanel);
        go.GetComponent<CardLinkHandler>().Init(cardPanelLinkManager, block);

        go.transform.Find("Title").GetComponent<TMP_Text>().text = block.info.title;

        var btn = go.GetComponent<Button>();
        btn.onClick.AddListener(() => DeductionText.text = block.info.text);

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
