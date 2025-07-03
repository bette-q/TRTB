using UnityEngine;

public class InteractEvidence : Interactable
{
    public EvidenceData evidenceData; // assign per prefab

    public override void Interact()
    {
        EvidenceEventContext.CurrentEvidenceData = evidenceData;
        base.Interact(); // Will execute the eventSequence (should include EvidenceEventAction)
        EvidenceEventContext.CurrentEvidenceData = null;
        gameObject.SetActive(false); // Or whatever cleanup you need
    }
}
