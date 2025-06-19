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
        GameStateManager.Instance.AddEvidence(block);
        // Remove this object from the world after collection
        gameObject.SetActive(false);
    }

    // Converts EvidenceData to an EvidenceBlock based on characterID
    public static EvidenceBlock GenerateEvidenceBlock(EvidenceData ed, CharacterID characterID)
    {
        var special = ed.specialBlocks.Find(s => s.characterID == characterID);
        if (special != null)
            return new EvidenceBlock(ed.id, special.title, special.description, special.icon);
        else
            return new EvidenceBlock(ed.id, ed.genericTitle, ed.genericText, ed.genericIcon);
    }
}
