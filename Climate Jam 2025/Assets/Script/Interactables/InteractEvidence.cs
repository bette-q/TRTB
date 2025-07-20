using UnityEngine;
using UnityEngine.SceneManagement;

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

        string tmpID = evidenceId;
        CharacterID cID = GameStateManager.Instance.currentCharacter;
        if (cID == ed.specialEvidence.characterID) tmpID = ed.specialEvidence.id;

        GameStateManager.Instance.SetCurEvidence(tmpID);

        Destroy(gameObject);
        SceneController.Instance.EnterAdditiveScene("POVGame");
    }
}
