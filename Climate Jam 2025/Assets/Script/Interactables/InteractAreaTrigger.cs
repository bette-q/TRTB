using UnityEngine;

public class InteractAreaTrigger : Interactable
{
    private bool triggered = false; // Prevents retrigger while player stays inside

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            Interact();
        }
    }

    // Optional: Allow retrigger when player leaves and re-enters
    private void OnTriggerExit(Collider other)
    {
        triggered = false;
    }
}
