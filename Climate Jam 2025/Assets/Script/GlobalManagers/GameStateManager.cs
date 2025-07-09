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

    // Player state
    public SerializableVector3 playerPosition;
    public float playerRotationY;

    // Collected notebook + combined blocks
    private List<EvidenceBlock> availableBlocks = new List<EvidenceBlock>();

    // Progress Tracking
    public string currentChapter;
    public List<string> completedChapters = new();
    private Dictionary<string, ChapterProgress> chapters = new();

    private HashSet<EventAction> triggeredActions = new();
    private HashSet<EventSequence> triggeredSequences = new();

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

    public bool HasCharacter(CharacterID id) => partyMembers.Contains(id);

    public void SwitchCharacter(CharacterID id)
    {
        if (partyMembers.Contains(id))
            currentCharacter = id;
    }

    public CharacterID GetCurrentCharacter() => currentCharacter;
    public CharacterID SetCurrentCharacter(CharacterID charIn) => currentCharacter = charIn;

    // ---- PROGRESS TRACKING ----
    public void SetFlag(string chapter, string mission, string flag, bool value = true)
    {
        if (!chapters.ContainsKey(chapter))
            chapters[chapter] = new ChapterProgress();
        if (!chapters[chapter].missions.ContainsKey(mission))
            chapters[chapter].missions[mission] = new MissionProgress();

        chapters[chapter].missions[mission].flags[flag] = value;
    }

    public bool GetFlag(string chapter, string mission, string flag)
    {
        if (!chapters.TryGetValue(chapter, out var ch)) return false;
        if (!ch.missions.TryGetValue(mission, out var ms)) return false;
        if (!ms.flags.TryGetValue(flag, out var value)) return false;
        return value;
    }

    public bool Triggered(EventAction action)
    {
        if (triggeredActions.Contains(action)) return true;
        triggeredActions.Add(action);
        return false;
    }

    public bool Triggered(EventSequence sequence)
    {
        if (triggeredSequences.Contains(sequence)) return true;
        triggeredSequences.Add(sequence);
        return false;
    }

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
    public IReadOnlyCollection<CharacterID> GetPartyMembers() => partyMembers;
    public IReadOnlyList<EvidenceBlock> GetAvailableBlocks() => availableBlocks.AsReadOnly();
    public IReadOnlyCollection<EventAction> GetTriggeredActions() => triggeredActions;
    public IReadOnlyCollection<EventSequence> GetTriggeredSequences() => triggeredSequences;
    public IReadOnlyDictionary<string, ChapterProgress> GetChapters()
     => new System.Collections.ObjectModel.ReadOnlyDictionary<string, ChapterProgress>(chapters);

    public void SetPartyMembers(IEnumerable<CharacterID> members)
    {
        partyMembers = new HashSet<CharacterID>(members);
    }
    public void SetAvailableBlocks(IEnumerable<EvidenceBlock> blocks)
    {
        availableBlocks = new List<EvidenceBlock>(blocks);
    }
    public void SetChapters(Dictionary<string, ChapterProgress> value)
    {
        chapters = new Dictionary<string, ChapterProgress>(value);
    }

    public void SetTriggeredActions(IEnumerable<EventAction> set)
    {
        triggeredActions = new HashSet<EventAction>(set);
    }

    public void SetTriggeredSequences(IEnumerable<EventSequence> set)
    {
        triggeredSequences = new HashSet<EventSequence>(set);
    }


    // ---- SCENE SWITCHING ----
    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }
}
