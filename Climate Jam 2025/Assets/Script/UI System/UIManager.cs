using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Popup UI")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    [Header("Item Display UI")]
    public GameObject itemPanel;
    public Image itemIcon;
    public TMP_Text itemDescription;

    [Header("CharacterManager")]
    public CharacterManager characterManager;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide all UI by default
        if (popupPanel) popupPanel.SetActive(false);
        if (itemPanel) itemPanel.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    // ---- DIALOGUE ----

    public void ShowDialogue(string text)
    {
        if (dialoguePanel) dialoguePanel.SetActive(true);
        if (dialogueText) dialogueText.text = text;
    }

    public void HideDialogue()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    // ---- POPUP ----

    public void ShowPopup(string text)
    {
        if (popupPanel) popupPanel.SetActive(true);
        if (popupText) popupText.text = text;

        // Optionally auto-hide after a few seconds
        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), 2.0f);
    }

    public void HidePopup()
    {
        if (popupPanel) popupPanel.SetActive(false);
    }

    // ---- ITEM ----

    public void ShowItem(string itemId)
    {
        // Look up icon/description (assume EvidenceDatabase or similar)
        var info = EvidenceDatabase.Instance.GetEvidenceInfo(itemId);
        if (info == null)
        {
            Debug.LogWarning($"[UIManager] Unknown item: {itemId}");
            return;
        }

        if (itemPanel) itemPanel.SetActive(true);
        if (itemIcon) itemIcon.sprite = info.icon;
        if (itemDescription) itemDescription.text = info.text;

        // Also show description in dialogue
        ShowDialogue(info.text);

        // Optionally, auto-hide after a few seconds or on click
        CancelInvoke(nameof(HideItem));
        Invoke(nameof(HideItem), 2.5f);
    }

    public void HideItem()
    {
        if (itemPanel) itemPanel.SetActive(false);
    }

    // ---- CHARACTER ----

    public void ShowCharacter(string characterName, string spriteTag = "")
    {
        if (characterManager == null)
        {
            Debug.LogWarning("[UIManager] No CharacterManager assigned.");
            return;
        }
        if (string.IsNullOrEmpty(spriteTag))
            characterManager.ShowCharacter(characterName);
        else
            characterManager.ForceShowCharacter(characterName, spriteTag);
    }

    public void HideCharacter(string characterName)
    {
        if (characterManager == null) return;
        characterManager.HideCharacter(characterName);
    }

    // ---- Panel/Other Utility (Optional for enable/disable UI panels) ----

    public void EnablePanel(string panelName)
    {
        // This method can be extended for your own panel management (e.g. map, notebook, etc)
        // Example:
        // if (panelName == "Map") mapPanel.SetActive(true);
        Debug.Log($"[UIManager] EnablePanel: {panelName} (implement as needed)");
    }

    public void ShowPopupImmediate(string text)
    {
        if (popupPanel) popupPanel.SetActive(true);
        if (popupText) popupText.text = text;
    }
}
