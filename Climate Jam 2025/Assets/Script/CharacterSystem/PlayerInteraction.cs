using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactPrompt;

    private List<Interactable> nearbyInteractables = new List<Interactable>();

    void Update()
    {
        interactPrompt.SetActive(nearbyInteractables.Count > 0);

        if (Input.GetKeyDown(interactKey) && nearbyInteractables.Count > 0)
        {
            // Interact with the closest interactable
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

            if (closest != null)
            {
                closest.Interact();
            }
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
