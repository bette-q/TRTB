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
    }

    public static EvidenceBlock GenerateEvidenceBlock(EvidenceData ed, CharacterID characterID)
    {
        if (ed.specialEvidence.characterID == characterID)
            return new EvidenceBlock(ed.specialEvidence, ed.type);
        else
            return new EvidenceBlock(ed.info, ed.type);
    }
}

// Holds the currently "active" evidence for this event
public static class EvidenceEventContext
{
    public static EvidenceData CurrentEvidenceData;
}
