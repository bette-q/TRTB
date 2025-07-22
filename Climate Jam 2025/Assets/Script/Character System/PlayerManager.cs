using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Handles character switch with a single player GameObject and sprite swapping
public class PlayerManager : MonoBehaviour
{
    public GameObject player; // Assign your player prefab here (has child with SpriteRenderer)
    public Sprite[] characterSprites; // Assign sprites in the same order as characterIDs
    public CharacterID[] characterIDs = new CharacterID[4]; // [0]=Main, [1]=Qiu, [2]=Ella, [3]=Mateo
    public GameObject[] characterIcons;

    private List<CharacterID> playableList = new List<CharacterID>();
    private Dictionary<CharacterID, int> characterIndexMap = new Dictionary<CharacterID, int>();

    private SpriteRenderer spriteRenderer; // Cached ref to the child's SpriteRenderer

    void Awake()
    {
        // Build dictionary mapping CharacterID -> index (based on array order)
        for (int i = 0; i < characterIDs.Length; i++)
        {
            if (!characterIndexMap.ContainsKey(characterIDs[i]))
                characterIndexMap[characterIDs[i]] = i;
        }

        // Find PlayerArt under Player
        if (player != null)
        {
            var art = player.transform.Find("PlayerArt");
            if (art != null)
                spriteRenderer = art.GetComponent<SpriteRenderer>();
            else
                Debug.LogError("PlayerArt child not found under Player!");
        }
    }

    void Start()
    {
        UpdatePlayableList();
        SetActiveCharacter(GameStateManager.Instance.GetCurrentCharacter());
    }

    void Update()
    {
        if (NotebookUIController.IsOpen || UIManager.Instance.IsDialogueActive)
            return;

        UpdatePlayableList();

        for (int i = 0; i < characterIDs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (playableList.Contains(characterIDs[i]))
                {
                    SetActiveCharacter(characterIDs[i]);
                    break;
                }
            }
        }
    }

    void SetActiveCharacter(CharacterID id)
    {
        if (!characterIndexMap.TryGetValue(id, out int idx) || idx < 0) return;
        if (spriteRenderer != null && idx < characterSprites.Length)
            spriteRenderer.sprite = characterSprites[idx];

        // Update GameStateManager with new active character
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SwitchCharacter(characterIDs[idx]);
    }

    void UpdatePlayableList()
    {
        var party = GameStateManager.Instance.GetPartyMembers();

        playableList = new List<CharacterID>();
        for (int i = 0; i < characterIDs.Length; i++)
        {
            if (party.Contains(characterIDs[i]))
                playableList.Add(characterIDs[i]);
        }

        UpdatePlayerListUI();
    }

    void UpdatePlayerListUI()
    {
        if (characterIcons == null || characterIcons.Length != characterIDs.Length)
            return;

        for (int i = 0; i < characterIcons.Length; i++)
        {
            bool shouldShow = playableList.Contains(characterIDs[i]);
            characterIcons[i].SetActive(shouldShow);
        }
    }

    void OnEnable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnPlayableCharacterListChanged += UpdatePlayableList;
    }

    void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnPlayableCharacterListChanged -= UpdatePlayableList;
    }
}
