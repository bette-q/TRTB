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
        if (NotebookUIController.IsOpen)
            return;

        // Remove any interactables that are null or inactive
        nearbyInteractables.RemoveAll(i => i == null || !i.gameObject.activeInHierarchy);

        // Show prompt only if something is in range
        interactPrompt.SetActive(nearbyInteractables.Count > 0);

        // Find the closest interactable
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

        // Interact logic
        if (Input.GetKeyDown(interactKey) && currentlyClosest != null)
        {
            currentlyClosest.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        // Ignore area triggers for prompt/interact key logic
        if (interactable != null
            && !(interactable is InteractAreaTrigger)
            && !nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Add(interactable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        if (interactable != null
            && !(interactable is InteractAreaTrigger)
            && nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Remove(interactable);
        }
    }

}
