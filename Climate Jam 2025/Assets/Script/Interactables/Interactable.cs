using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public EventSequence eventSeq; // Assign this in Inspector per prefab/type

    public virtual void Interact()
    {
        if (eventSeq != null)
            EventManager.Instance.Execute(eventSeq);
        else
            Debug.LogWarning($"{name} Interactable has no EventSequence assigned!");
    }
}
