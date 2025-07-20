using UnityEngine;

[CreateAssetMenu(menuName = "EventSystem/Actions/AddCharacterEventAction")]
public class AddCharacterEventAction : EventAction
{
    public CharacterID characterID; // Set in Inspector for this asset

    public override void Execute()
    {
        GameStateManager.Instance.AddSwitchableCharacter(characterID);
        //Debug.Log($"[AddCharacterEventAction] Added {characterID} to playable characters.");

        // Destroy the NPC GameObject (context)
        var npcGO = AddCharacterEventContext.CurrentSourceGameObject;
        if (npcGO != null)
        {
            //Debug.Log($"[AddCharacterEventAction] Destroying NPC object: {npcGO.name}");
            Object.Destroy(npcGO);
        }
        else
        {
            //Debug.LogWarning("[AddCharacterEventAction] No NPC GameObject context found to destroy.");
        }
    }
}

public static class AddCharacterEventContext
{
    // Holds the reference to the currently interacted NPC GameObject
    public static GameObject CurrentSourceGameObject;
}
