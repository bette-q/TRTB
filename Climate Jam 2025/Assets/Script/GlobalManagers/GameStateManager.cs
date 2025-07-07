using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

//global, handles player state, stores evidence, s/l, and ending
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // Player Logic
    public CharacterID currentCharacter { get; private set; } // Current controlled character
    private HashSet<CharacterID> partyMembers = new HashSet<CharacterID>();
    public event System.Action OnPlayableCharacterListChanged; //event for player man

    // Collected notebook + combined blocks
    private List<EvidenceBlock> availableBlocks = new List<EvidenceBlock>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // If you want to start with just the investigator:
        partyMembers.Clear();
        partyMembers.Add(CharacterID.Main);
        currentCharacter = CharacterID.Main;
    }

    // ---- PLAYER (Character) Logic ----

    // Called by PlayerManager or NPC when adding new member
    public void AddSwitchableCharacter(CharacterID newChar)
    {
        if (partyMembers.Add(newChar))
            OnPlayableCharacterListChanged?.Invoke();
    }

    public IEnumerable<CharacterID> GetSwitchableCharacters() => partyMembers;

    public bool HasCharacter(CharacterID id) => partyMembers.Contains(id);

    public void SwitchCharacter(CharacterID id)
    {
        if (partyMembers.Contains(id))
            currentCharacter = id;
    }

    public CharacterID GetCurrentCharacter() => currentCharacter;

    // ---- NOTEBOOK (Evidence) Logic ----

    public bool AddBlock(EvidenceBlock block)
    {
        if (HasBlock(block.id)) return false;
        availableBlocks.Add(block);
        return true;
    }

    public void RemoveBlocksByIds(List<string> ids)
    {
        availableBlocks.RemoveAll(b => ids.Contains(b.id));
    }

    public bool HasBlock(string id)
    {
        return availableBlocks.Exists(b => b.id == id);
    }

    public IReadOnlyList<EvidenceBlock> GetAvailableBlocks() => availableBlocks;


    public List<EvidenceBlock> GetAllFinalBlocks()
    {
        // Only return blocks of type ComboBlock
        return availableBlocks.Where(b => b.blockType == EvidenceBlockType.FinalCombo).ToList();
    }

    public bool AddEvidenceById(string evidenceId)
    {
        // Assume you have a singleton EvidenceDatabase with GetEvidenceById()
        var ed = EvidenceDatabase.Instance.GetEvidenceData(evidenceId);
        if (ed == null)
        {
            Debug.LogWarning($"[GSM] No EvidenceData found for id: {evidenceId}");
            return false;
        }
        var characterID = GetCurrentCharacter();
        var eb = EvidenceEventAction.GenerateEvidenceBlock(ed, characterID);
        return AddBlock(eb);
    }


    // ---- SAVE/LOAD LOGIC ----
    // Add your serialization/deserialization here in the future.

    // ---- SCENE SWITCHING ----
    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }
}
