using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    [Header("Popup UI")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    [Header("Item Box UI")]
    public GameObject showItemPanel;
    public TMP_Text itemBoxText;
    public Image itemIconImage;

    [Header("CharacterManager")]
    public CharacterManager characterManager;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide all UI by default
        if (popupPanel) popupPanel.SetActive(false);
        if (showItemPanel) showItemPanel.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    // ---- DIALOGUE ----

    public void ShowDialogue(string speaker, string text)
    {
        if (dialoguePanel) dialoguePanel.SetActive(true);
        if (nameText) nameText.text = speaker ?? "";
        if (dialogueText) dialogueText.text = text;
    }

    public void HideDialogue()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    // ---- POPUP ----

    public void ShowPopup(string text, float duration = 2.0f)
    {
        if (popupPanel) popupPanel.SetActive(true);
        if (popupText) popupText.text = text;

        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), duration);
    }

    public void HidePopup()
    {
        if (popupPanel) popupPanel.SetActive(false);
    }

    // ---- ITEM DISPLAY ----
    public void ShowItem(string itemId)
    {
        var info = EvidenceDatabase.Instance.GetEvidenceInfo(itemId);
        if (info == null)
        {
            Debug.LogWarning($"[UIManager] Unknown item: {itemId}");
            return;
        }
        if (showItemPanel) showItemPanel.SetActive(true);
        if (itemBoxText) itemBoxText.text = info.text;
        if (itemIconImage) itemIconImage.sprite = info.icon;
    }

    public void HideItem()
    {
        if (showItemPanel) showItemPanel.SetActive(false);
    }

    // ---- CHARACTER ----
    public void ArrangeCharacters(CharacterID controlled, string targetName)
    {
        if (characterManager != null)
            characterManager.ArrangeForDialogue(controlled, targetName);
    }

    public void ChangeCharacterSprite(string side, string tagName)
    {
        if (characterManager != null)
            characterManager.ChangeSprite(side, tagName);
    }
    public void ShowSpeaker(string speakerName)
    {
        if (characterManager != null)
            characterManager.ShowSpeaker(speakerName, GameStateManager.Instance.GetCurrentCharacter());
    }
    public void HideCharacters()
    {
        if (characterManager != null)
            characterManager.HideAllCharacters();
    }
    // ---- Panel/Other Utility ----

    public void EnablePanel(string panelName)
    {
        Debug.Log($"[UIManager] EnablePanel: {panelName} (implement as needed)");
    }

    // Optional: Force popup to appear instantly (no timer)
    public void ShowPopupImmediate(string text)
    {
        if (popupPanel) popupPanel.SetActive(true);
        if (popupText) popupText.text = text;
        CancelInvoke(nameof(HidePopup));
    }
}
