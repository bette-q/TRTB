using UnityEngine;

public class InteractableNPC : Interactable
{
    [Header("NPC Info")]
    public string npcID;
    public string displayName;
    //public string eventID; // Dialogue, side quest, or any event

    public override void Interact()
    {
        AddCharacterEventContext.CurrentSourceGameObject = this.gameObject;

        // Subscribe to dialogue end event (one-time)
        InkManager.Instance.OnDialogueEnd += ClearNPCContext;

        base.Interact();
    }

    void ClearNPCContext()
    {
        AddCharacterEventContext.CurrentSourceGameObject = null;
        // Unsubscribe so it only runs once
        InkManager.Instance.OnDialogueEnd -= ClearNPCContext;
    }


}
public static class AddCharacterEventContext
{
    // Holds the reference to the currently interacted NPC GameObject
    public static GameObject CurrentSourceGameObject;
}