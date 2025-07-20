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

    [Header("Scene Transition Door System")]
    private string targetDoorID; // Which door ID to spawn near
    private bool hasPendingDoorSpawn = false;

    // Collected notebook + combined blocks
    private List<EvidenceBlock> availableBlocks = new List<EvidenceBlock>();

    // Progress Tracking
    private HashSet<EventSequence> triggeredSequences = new();
    private HashSet<EventAction> triggeredActions = new();

    public string currentChapter;
    public List<string> completedChapters = new();
    public Dictionary<int, ChapterProgress> chapters = new()
    {
        [0] = new ChapterProgress
        {
            missions = new()
            {
                [1] = new MissionProgress
                {
                    flags = new() { ["phone_call"] = false, ["mateo_init"] = false }
                },
            }
        }
    };


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
        partyMembers.Add(CharacterID.Player);
        currentCharacter = CharacterID.Player;
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
    public void SetFlag(int chapter, int mission, string flag)
    {
        if (!chapters.ContainsKey(chapter))
            chapters[chapter] = new ChapterProgress();
        if (!chapters[chapter].missions.ContainsKey(mission))
            chapters[chapter].missions[mission] = new MissionProgress();

        chapters[chapter].missions[mission].flags[flag] = true;
    }

    public bool GetFlag(int chapter, int mission, string flag)
    {
        if (!chapters.TryGetValue(chapter, out var ch)) return false;
        if (!ch.missions.TryGetValue(mission, out var ms)) return false;
        if (!ms.flags.TryGetValue(flag, out var value)) return false;
        return value;
    }

    public bool HasTriggered(EventAction action)
    {
        return triggeredActions.Contains(action);
    }

    public void MarkAsTriggered(EventAction action)
    {
        triggeredActions.Add(action);
    }

    public bool HasTriggered(EventSequence sequence)
    {
        return triggeredSequences.Contains(sequence);
    }

    public void MarkAsTriggered(EventSequence sequence)
    {
        triggeredSequences.Add(sequence);
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
    public IReadOnlyDictionary<int, ChapterProgress> GetChapters()
     => new System.Collections.ObjectModel.ReadOnlyDictionary<int, ChapterProgress>(chapters);

    public void SetPartyMembers(IEnumerable<CharacterID> members)
    {
        partyMembers = new HashSet<CharacterID>(members);
    }
    public void SetAvailableBlocks(IEnumerable<EvidenceBlock> blocks)
    {
        availableBlocks = new List<EvidenceBlock>(blocks);
    }
    public void SetChapters(Dictionary<int, ChapterProgress> value)
    {
        chapters = new Dictionary<int, ChapterProgress>(value);
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

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Prototype Test");
    }

    public void SetPlayerSpawnInfo(string doorID)
    {
        targetDoorID = doorID;
        hasPendingDoorSpawn = true;

        Debug.Log($"Set target door ID: {doorID}");
    }

    public bool HasPendingDoorSpawn() => hasPendingDoorSpawn;

    public string GetTargetDoorID() => targetDoorID;

    public void ClearPendingDoorSpawn()
    {
        hasPendingDoorSpawn = false;
        targetDoorID = null;
    }

    // Method to spawn player near a specific door
    public void SpawnPlayerNearDoor(string doorID)
    {
        // Find all BuildingDoors in the scene
        BuildingDoor[] allDoors = FindObjectsByType<BuildingDoor>(FindObjectsSortMode.None);

        Debug.Log($"Found {allDoors.Length} doors in scene, looking for doorID: {doorID}");

        foreach (BuildingDoor door in allDoors)
        {
            if (door.GetDoorID() == doorID)
            {
                // Found the matching door! Spawn player near it
                Vector3 spawnPosition = door.GetSpawnPosition();

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawnPosition;
                    Debug.Log($"Player spawned near door: {doorID} at position {spawnPosition}");
                }
                else
                {
                    Debug.LogWarning("Player GameObject not found! Make sure player has 'Player' tag.");
                }
                return;
            }
        }

        Debug.LogWarning($"Could not find door with ID: {doorID} in scene {SceneManager.GetActiveScene().name}");
    }

    // Enhanced scene loading with door spawn
    public void LoadSceneWithDoorSpawn(string sceneName, string doorID)
    {
        SetPlayerSpawnInfo(doorID);
        SceneManager.LoadScene(sceneName);
    }
}
