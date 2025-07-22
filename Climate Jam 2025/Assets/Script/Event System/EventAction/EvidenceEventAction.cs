using UnityEngine;

[CreateAssetMenu(menuName = "EventSystem/Actions/EvidenceEventAction")]
public class EvidenceEventAction : EventAction
{
    public override void Execute()
    {
        var ed = EvidenceEventContext.CurrentEvidenceData;
        if (ed == null)
        {
            Debug.LogWarning("[EvidenceEventAction] No EvidenceData provided via context!");
            return;
        }

        CharacterID characterID = GameStateManager.Instance.currentCharacter;
        EvidenceBlock block = GenerateEvidenceBlock(ed, characterID);
        GameStateManager.Instance.AddBlock(block);
        Debug.Log("[EvidenceEventAction] Collected evidence: " + ed.name);

        EvidenceEventContext.CurrentEvidenceData = null;
    }

    public static EvidenceBlock GenerateEvidenceBlock(EvidenceData ed, CharacterID characterID)
    {
        // Get the proper EvidenceInfo and id for this character
        EvidenceInfo chosenInfo = (ed.specialEvidence != null && ed.specialEvidence.characterID == characterID)
            ? ed.specialEvidence
            : ed.info;

        return new EvidenceBlock(chosenInfo.id, ed.type);
    }
}




// Holds the currently "active" evidence for this event
public static class EvidenceEventContext
{
    public static EvidenceData CurrentEvidenceData;
}

