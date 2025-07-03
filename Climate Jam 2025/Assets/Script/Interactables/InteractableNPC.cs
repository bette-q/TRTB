using UnityEngine;

public class InteractableNPC : Interactable
{
    [Header("NPC Info")]
    public string npcID;
    public string displayName;
    //public string eventID; // Dialogue, side quest, or any event

    public override void Interact()
    {
        //if (!string.IsNullOrEmpty(eventID))
        //{
        //    EventManager.Instance.ExecuteEvent(eventID);
        //}
    }
}
