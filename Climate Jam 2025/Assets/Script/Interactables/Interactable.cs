using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public EventSequence eventSeq;
    public EventAction eventAction; // NEW: for single-action use

    public virtual void Interact()
    {
        if (eventSeq != null && eventAction != null)
        {
            Debug.LogWarning($"{name} Interactable: Both EventSequence and EventAction are assigned. Only one should be used!");
        }

        if (eventSeq != null)
        {
            EventManager.Instance.Execute(eventSeq);
        }
        else if (eventAction != null)
        {
            // Wrap the action in an array or whatever EventManager expects
            EventManager.Instance.Execute(eventAction);
        }
        else
        {
            Debug.LogWarning($"{name} Interactable has no EventSequence or EventAction assigned!");
        }
    }
}
public static class DestroySelfEventContext
{
    // Holds the reference to the currently interacted NPC GameObject
    public static GameObject CurrentSourceGameObject;
}