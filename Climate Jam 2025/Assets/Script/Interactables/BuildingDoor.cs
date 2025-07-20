using UnityEngine;

public class BuildingDoor : Interactable
{
    [Header("Building Door Settings")]
    [Header("Paired Door System")]
    [Tooltip("Unique ID for this door (should match the door in target scene)")]
    public string doorID;

    [Tooltip("Distance to spawn player from door")]
    public float spawnOffset = 2f;

   // Method to get the door ID
    public string GetDoorID()
    {
        return doorID;
    }

    // Method to get spawn position
    public Vector3 GetSpawnPosition()
    {
        return transform.position - transform.forward * spawnOffset;
    }

    // Optional: Visual debugging
    void OnDrawGizmos()
    {
        // Show where player will spawn (away from door)
        Gizmos.color = Color.green;
        Vector3 spawnPos = GetSpawnPosition();
        Gizmos.DrawWireSphere(spawnPos, 0.5f);

        // Show door forward direction
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}