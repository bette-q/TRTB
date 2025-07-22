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

        DestroySelfEventContext.CurrentSourceGameObject = this.gameObject;

        // Subscribe to dialogue end event (one-time)
        InkManager.Instance.OnDialogueEnd += ClearGOContext;

        //EvidenceEventContext.CurrentEvidenceData = EvidenceDatabase.Instance.GetEvidenceData(evidenceId);
        //base.Interact();
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

    void ClearGOContext()
    {
        DestroySelfEventContext.CurrentSourceGameObject = null;
        // Unsubscribe so it only runs once
        InkManager.Instance.OnDialogueEnd -= ClearGOContext;
    }

}
