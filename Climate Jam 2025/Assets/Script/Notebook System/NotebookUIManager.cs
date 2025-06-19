using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class NotebookUIManager : MonoBehaviour
{
    [HideInInspector]
    public static bool IsOpen { get; private set; }

    public GameObject notebookPanel;
    public RectTransform blockParentPanel;
    public Transform blockLayout;
    public GameObject evidenceBlockPrefab;
    public TMP_Text descriptionBox;
    public Button combineButton;
    public KeyCode toggleKey = KeyCode.N;

    public CardPanelLinkManager cardPanelLinkManager;

    private List<GameObject> spawnedCards = new List<GameObject>();
    private List<EvidenceBlock> selectedBlocks = new List<EvidenceBlock>();

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
        // Only allow combine if 2+ EBs are selected
        combineButton.interactable = selectedBlocks.Count >= 2 && selectedBlocks.All(b => b.blockType == EvidenceBlockType.Evidence);
    }

    public void Open()
    {
        notebookPanel.SetActive(true);
        IsOpen = true;
        RefreshUI();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        notebookPanel.SetActive(false);
        IsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RefreshUI()
    {
        foreach (var go in spawnedCards) Destroy(go);
        spawnedCards.Clear();
        selectedBlocks.Clear();

        var blocks = GameStateManager.Instance.GetAvailableBlocks();
        foreach (var block in blocks)
        {
            var go = Instantiate(evidenceBlockPrefab, blockLayout);

            go.GetComponent<FreeDragBlock>().Init(blockParentPanel);
            go.GetComponent<CardLinkHandler>().Init(cardPanelLinkManager);

            go.transform.Find("Title").GetComponent<TMP_Text>().text = block.title;

            var btn = go.GetComponent<Button>();
            if (block.blockType == EvidenceBlockType.Evidence)
            {
                btn.onClick.AddListener(() =>
                {
                    descriptionBox.text = block.text;
                    // Toggle selection
                    if (selectedBlocks.Contains(block))
                    {
                        selectedBlocks.Remove(block);
                        go.GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        selectedBlocks.Add(block);
                        go.GetComponent<Image>().color = Color.yellow;
                    }
                });
            }
            else
            {
                btn.onClick.AddListener(() =>
                {
                    descriptionBox.text = block.text;
                });
                go.GetComponent<Image>().color = Color.cyan;
            }
            spawnedCards.Add(go);
        }

        // Clear description if nothing is selected
        if (blocks.Count == 0)
            descriptionBox.text = "";
    }

    public void OnCombineButtonClicked()
    {
        var selectedIDs = selectedBlocks.Select(b => b.id).ToList();
        var combo = ComboManager.Instance.FindValidCombo(selectedIDs);

        if (combo != null)
        {
            // Remove used evidence blocks
            GameStateManager.Instance.RemoveBlocksByIds(selectedIDs);

            // Add the new combo block
            var deductionEB = InteractEvidence.GenerateEvidenceBlock(combo.resultEvidence, GameStateManager.Instance.currentCharacterID);
            deductionEB.blockType = EvidenceBlockType.ComboBlock; // Mark as deduction
            GameStateManager.Instance.AddBlock(deductionEB);

            RefreshUI();
        }
        else
        {
            Debug.Log("Invalid combo.");
            // Optionally: feedback for invalid combo (shake, sound, etc.)
        }
    }
}
