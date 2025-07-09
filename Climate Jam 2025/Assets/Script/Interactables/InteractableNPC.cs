using UnityEngine;

public class InteractableNPC : Interactable
{
    [Header("NPC Info")]
    public string npcID;
    public string displayName;
    //public string eventID; // Dialogue, side quest, or any event

    public override void Interact()
    {
        // 1. Set the context before executing the event.
        AddCharacterEventContext.CurrentSourceGameObject = this.gameObject;

        // 2. Run the event as usual.
        base.Interact();

        // 3. (Optional) Clear the context after use, to avoid future issues.
        AddCharacterEventContext.CurrentSourceGameObject = null;
    }


}
