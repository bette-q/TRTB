using UnityEngine;

public class InteractEvidence : Interactable // your abstract base class
{
    public EvidenceData evidenceData; // Assign this in the Inspector

    public override void Interact()
    {
        // Get the currently active character from GameStateManager
        CharacterID characterID = GameStateManager.Instance.currentCharacterID;

        // Create the appropriate evidence block
        EvidenceBlock block = GenerateEvidenceBlock(evidenceData, characterID);

        // Add it to the notebook via GameStateManager
        GameStateManager.Instance.AddBlock(block);

        // Remove this object from the world after collection
        gameObject.SetActive(false);
    }

    // Converts EvidenceData to an EvidenceBlock based on characterID
    public static EvidenceBlock GenerateEvidenceBlock(EvidenceData ed, CharacterID characterID)
    {
        var sd = ed.specialEvidence;
        if (sd.characterID == characterID)
            return new EvidenceBlock(sd);
        else
            return new EvidenceBlock(ed.info);
    }
}
