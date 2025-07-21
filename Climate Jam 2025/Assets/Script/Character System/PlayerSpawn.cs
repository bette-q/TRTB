using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
    void Start()
    {
        // Check for door-based spawning
        if (GameStateManager.Instance.HasPendingDoorSpawn())
        {
            string doorID = GameStateManager.Instance.GetTargetDoorID();
            GameStateManager.Instance.SpawnPlayerNearDoor(doorID);
            GameStateManager.Instance.ClearPendingDoorSpawn();
        }
    }
}