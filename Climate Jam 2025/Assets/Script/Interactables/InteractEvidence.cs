using UnityEngine;

public class InteractEvidence : Interactable
{
    [Tooltip("Unique evidence ID from EvidenceData.info.id")]
    public string evidenceId; // assign per prefab, not direct ref!

    public override void Interact()
    {
        var ed = EvidenceDatabase.Instance.GetEvidenceData(evidenceId); // <-- This fetches the SO
        if (ed == null)
        {
            Debug.LogWarning("No EvidenceData found for evidenceId: " + evidenceId);
            return;
        }
        EvidenceEventContext.CurrentEvidenceData = ed;
        base.Interact(); // Will execute the eventSequence (should include EvidenceEventAction)
        EvidenceEventContext.CurrentEvidenceData = null;
        gameObject.SetActive(false); // Or whatever cleanup you need
    }
}
