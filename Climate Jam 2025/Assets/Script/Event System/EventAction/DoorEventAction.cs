using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "DoorEA", menuName = "EventSystem/Actions/DoorEventAction")]
public class DoorEventAction : EventAction
{
    [Header("Scene Transition")]
    public string targetSceneName;

    [Header("Paired Door System")]
    [Tooltip("ID of the door in the target scene to spawn near")]
    public string pairedDoorID;

    public override void Execute()
    {
        if (!ArePrerequisitesMet()) return;

        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning("DoorEventAction: No target scene specified!");
            return;
        }

        if (string.IsNullOrEmpty(pairedDoorID))
        {
            Debug.LogWarning("DoorEventAction: No paired door ID specified!");
            return;
        }

        // Save which door to spawn near in the target scene
        GameStateManager.Instance.SetPlayerSpawnInfo(pairedDoorID);

        // Load the target scene
        SceneManager.LoadScene(targetSceneName);
    }

    // Method for BuildingDoor to get the paired door ID
    public string GetPairedDoorID()
    {
        return pairedDoorID;
    }
}