using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public EventSequence eventSeq;
    public abstract void Interact();
}
