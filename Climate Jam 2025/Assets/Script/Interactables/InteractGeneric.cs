using UnityEngine;

public class GenericInteractable : Interactable
{
    public override void Interact()
    {
        Debug.LogWarning("Interacted with Generic Interactable!");
        EventManager.Instance.Execute(eventSeq);
    }
}
