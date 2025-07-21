using UnityEngine;
using System.Collections;

public class InteractEvidence : Interactable
{
    [Tooltip("Unique evidence ID from EvidenceData.info.id")]
    public string evidenceId; // assign per prefab, not direct ref!

    public override void Interact()
    {
        if (!GameStateManager.Instance.sphereEnabled)
        {
            StartCoroutine(ShowDialogueAndWaitForClick());
            return;
        }

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

    private IEnumerator ShowDialogueAndWaitForClick()
    {
        UIManager.Instance.ShowDialogue("", "Nothing is happening");

        // Wait until the player clicks the left mouse button
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null; // Wait for next frame
        }

        UIManager.Instance.HideDialogue();
        // Now the player can continue; do any additional logic here if needed
    }
}
