using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// handles character switch
public class PlayerManager : MonoBehaviour
{
    public GameObject[] characters; // assign Main, Qiu, Ella, Mateo in order
    public CharacterID[] characterIDs = new CharacterID[4]; // [0]=Main, [1]=Qiu, [2]=Ella, [3]=Mateo
    public GameObject[] characterIcons;

    private List<CharacterID> playableList = new List<CharacterID>();
    private Dictionary<CharacterID, int> characterIndexMap = new Dictionary<CharacterID, int>();

    void Awake()
    {
        // Build dictionary mapping CharacterID -> index (based on array order)
        for (int i = 0; i < characterIDs.Length; i++)
        {
            if (!characterIndexMap.ContainsKey(characterIDs[i]))
                characterIndexMap[characterIDs[i]] = i;
        }
    }

    void Start()
    {
        UpdatePlayableList();
        SetActiveCharacter(GameStateManager.Instance.GetCurrentCharacter());
    }

    void Update()
    {
        if (NotebookUIController.IsOpen)
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

        for (int i = 0; i < characters.Length; i++)
        {
            var renderer = characters[i].GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = (i == idx);
        }

        // Update GameStateManager with new active character
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SwitchCharacter(characterIDs[idx]);
    }

    void UpdatePlayableList()
    {
        var party = GameStateManager.Instance.GetPartyMembers();

        // Sort playableList by your fixed characterIDs order (always Main, Qiu, Ella, Mateo order)
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
