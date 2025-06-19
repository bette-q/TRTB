using UnityEngine;
using System.Collections.Generic;

//global, handles player state, stores evidence, s/l, and ending
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // Current controlled character
    public CharacterID currentCharacterID { get; private set; }

    // Collected notebook blocks (runtime only)
    private List<EvidenceBlock> collectedBlocks = new List<EvidenceBlock>();

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
        // TODO: Notify UI, handle camera, etc.
    }

    // ---- NOTEBOOK (Evidence) Logic ----

    public bool AddEvidence(EvidenceBlock block)
    {
        if (collectedBlocks.Exists(b => b.id == block.id))
            return false; // Already collected
        collectedBlocks.Add(block);
        // TODO: Notify notebook UI
        Debug.Log($"Collected: {block.title} ({block.id})");
        return true;
    }

    public IReadOnlyList<EvidenceBlock> GetCollectedBlocks() => collectedBlocks.AsReadOnly();

    public bool HasEvidence(string edID)
    {
        return collectedBlocks.Exists(b => b.id == edID);
    }

    // ---- SAVE/LOAD LOGIC ----
    // Add your serialization/deserialization here in the future.
}
