using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactPrompt;

    private Interactable currentlyClosest = null;
    private List<Interactable> nearbyInteractables = new List<Interactable>();

    void Update()
    {
        // Show prompt only if something is in range
        interactPrompt.SetActive(nearbyInteractables.Count > 0);

        // Find the closest interactable (no highlighting now)
        Interactable closest = null;
        float closestDist = float.MaxValue;
        foreach (var interactable in nearbyInteractables)
        {
            float dist = (interactable.transform.position - transform.position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = interactable;
            }
        }

        currentlyClosest = closest;

        // Interact logic (delegates to Interactable, which uses GSM)
        if (Input.GetKeyDown(interactKey) && currentlyClosest != null)
        {
            currentlyClosest.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        if (interactable != null && !nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Add(interactable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        if (interactable != null && nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Remove(interactable);
        }
    }
}
