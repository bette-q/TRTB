using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//global, handles player state, stores evidence, s/l, and ending
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // Player Logic
    public CharacterID currentCharacter { get; private set; } // Current controlled character
    private List<CharacterID> switchableCharacters = new List<CharacterID>(); // Playable party
    public event System.Action OnPlayableCharacterListChanged;

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
        switchableCharacters.Clear();
        switchableCharacters.Add(CharacterID.Main);
        currentCharacter = CharacterID.Main;
    }

    // ---- PLAYER (Character) Logic ----

    // Called by PlayerManager or NPC when adding new member
    public void AddSwitchableCharacter(CharacterID newChar)
    {
        if (!switchableCharacters.Contains(newChar))
            switchableCharacters.Add(newChar);

        OnPlayableCharacterListChanged?.Invoke();// notify PlayerManager
    }

    public List<CharacterID> GetSwitchableCharacters() => new List<CharacterID>(switchableCharacters);

    public void SwitchCharacter(CharacterID id)
    {
        if (switchableCharacters.Contains(id))
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

    // ---- SAVE/LOAD LOGIC ----
    // Add your serialization/deserialization here in the future.
}
