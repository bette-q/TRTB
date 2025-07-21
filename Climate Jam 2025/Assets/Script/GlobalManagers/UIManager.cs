using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public bool IsDialogueActive;

    [Header("Popup UI")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    // ---- POPUP QUEUE SUPPORT ----
    private Queue<(string text, float duration)> popupQueue = new Queue<(string, float)>();
    private bool isShowingPopup = false;

    [Header("Item Box UI")]
    public GameObject showItemPanel;
    public TMP_Text itemBoxText;
    public Image itemIconImage;

    [Header("Panels")]
    public GameObject blackOutPanel;

    [Header("CharacterManager")]
    public CharacterManager characterManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        HideAllPanel();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[UIManager] OnSceneLoaded called for: " + scene.name);
        RelinkPanels();
    }
    public void ReactivateMainUICanvas()
    {
        Debug.Log("[UIManager] OnSceneLoaded called for: main");
        RelinkPanels();
    }

    void RelinkPanels()
    {
       
        GameObject canvas = GameObject.Find("UICanvas");
        if (canvas == null)
        {
            Debug.LogWarning("[UIManager] UICanvas not found in scene!");
            return;
        }

        // Panels
        dialoguePanel = canvas.transform.Find("DialoguePanel")?.gameObject;
        blackOutPanel = canvas.transform.Find("BlackOutPanel")?.gameObject;
        popupPanel = canvas.transform.Find("PopUpPanel")?.gameObject;
        showItemPanel = canvas.transform.Find("ShowItemPanel")?.gameObject;

        // DialoguePanel children
        if (dialoguePanel != null)
        {
            var dialogueTextGO = dialoguePanel.transform.Find("DialogueText");
            if (dialogueTextGO != null)
            {
                dialogueText = dialogueTextGO.transform.Find("Dialogue")?.GetComponent<TMPro.TMP_Text>();
                nameText = dialogueTextGO.transform.Find("Name")?.GetComponent<TMPro.TMP_Text>();
            }
            else
            {
                dialogueText = null;
                nameText = null;
            }

            // CharacterManager under DialoguePanel
            characterManager = dialoguePanel.GetComponentInChildren<CharacterManager>();
        }

        // ShowItemPanel
        if (showItemPanel != null)
        {
            var showDialogueTextGO = showItemPanel.transform.Find("DialogueText");
            itemBoxText = showDialogueTextGO != null
                ? showDialogueTextGO.GetComponent<TMPro.TMP_Text>()
                : null;

            itemIconImage = showItemPanel.transform.Find("ItemIcon")?.GetComponent<UnityEngine.UI.Image>();
        }

        // PopUpPanel
        if (popupPanel != null)
        {
            popupText = popupPanel.transform.Find("PopUpText")?.GetComponent<TMPro.TMP_Text>();
        }

        HideAllPanel();
        //Debug.Log("[UIManager] Auto-linked UI panels for scene: " + scene.name);
    }


    private void HideAllPanel()
    {
        // Hide all UI by default
        if (popupPanel) popupPanel.SetActive(false);
        if (showItemPanel) showItemPanel.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (blackOutPanel) blackOutPanel.SetActive(false);
    }

    // ---- DIALOGUE ----
    public void ShowDialogue(string speaker, string text)
    {
        IsDialogueActive = true;
        if (dialoguePanel) dialoguePanel.SetActive(true);
        if (nameText) nameText.text = speaker ?? "";
        if (dialogueText) dialogueText.text = text;
    }

    public void HideDialogue()
    {
        IsDialogueActive = false;
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    // ---- POPUP ----
    public void ShowPopup(string text, float duration = 2.0f)
    {
        popupQueue.Enqueue((text, duration));
        TryShowNextPopup();
    }

    private void TryShowNextPopup()
    {
        if (isShowingPopup || popupQueue.Count == 0)
            return;

        var (text, duration) = popupQueue.Dequeue();
        isShowingPopup = true;

        if (popupPanel) popupPanel.SetActive(true);
        if (popupText) popupText.text = text;

        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), duration);
    }

    public void HidePopup()
    {
        if (popupPanel) popupPanel.SetActive(false);
        isShowingPopup = false;
        // Now try showing the next popup in the queue, if any
        TryShowNextPopup();
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
        // (itemBoxText) itemBoxText.text = info.text;
        if (itemIconImage) itemIconImage.sprite = info.icon;
    }

    public void HideItem()
    {
        if (showItemPanel) showItemPanel.SetActive(false);
    }

    // ---- CHARACTER ----
    public void ArrangeCharacters(string leftName, string rightName)
    {
        if (characterManager != null)
            characterManager.ArrangeForDialogue(leftName, rightName);
    }

    public void ChangeCharacterSprite(string side, string tagName)
    {
        if (characterManager != null)
            characterManager.ChangeSprite(side, tagName);
    }
    public void ShowSpeaker(string speakerName)
    {
        if (characterManager != null)
            characterManager.ShowSpeaker(speakerName);
    }

    public void HideCharacters()
    {
        if (characterManager != null)
            characterManager.HideAllCharacters();
    }

    // ---- Panel/Other Utility ----
    public void SetBlackOut(bool on)
    {
        if (blackOutPanel)
            blackOutPanel.SetActive(on);
    }

    public void EnablePanel(string name)
    {
        if (name == "sphere")
        {
            GameStateManager.Instance.sphereEnabled = true;
        }    
        else
        {
            Debug.Log($"[UIManager] EnablePanel: {name} (implement as needed)");

        }

    }

    // Optional: Force popup to appear instantly (no timer)
    public void ShowPopupImmediate(string text)
    {
        if (popupPanel) popupPanel.SetActive(true);
        if (popupText) popupText.text = text;
        CancelInvoke(nameof(HidePopup));
    }
}
