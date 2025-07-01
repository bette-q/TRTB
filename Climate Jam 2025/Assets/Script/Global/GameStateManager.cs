using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//global, handles player state, stores evidence, s/l, and ending
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // Current controlled character
    public CharacterID currentCharacterID { get; private set; }

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
    }

    // ---- PLAYER (Character) Logic ----

    public void SwitchCharacter(CharacterID newID)
    {
        currentCharacterID = newID;
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

    public IReadOnlyList<EvidenceBlock> GetAvailableBlocks() => availableBlocks;


    public List<EvidenceBlock> GetAllFinalBlocks()
    {
        // Only return blocks of type ComboBlock
        return availableBlocks.Where(b => b.blockType == EvidenceBlockType.FinalCombo).ToList();
    }

    // ---- SAVE/LOAD LOGIC ----
    // Add your serialization/deserialization here in the future.
}
