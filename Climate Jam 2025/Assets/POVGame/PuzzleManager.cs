using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("All puzzle prefabs (name must match evidence name)")]
    public List<GameObject> puzzlePrefabs;

    [Header("Single placeholder where the puzzle will appear")]
    public Transform placeholder;

    private GameObject _activePuzzleInstance;

    void Start()
    {
        string puzzleName = GameStateManager.Instance.GetCurEvidence();
        PlacePuzzleByName(puzzleName);
    }

    public void PlacePuzzleByName(string name)
    {
        // Remove previous puzzle if exists
        if (_activePuzzleInstance)
            Destroy(_activePuzzleInstance);

        // Find prefab by GameObject.name
        GameObject match = puzzlePrefabs.Find(prefab => prefab != null && prefab.name == name);
        if (match == null)
        {
            Debug.LogError("[PuzzleManager] No puzzle prefab found with name: " + name);
            return;
        }

        // Instantiate at the placeholder
        _activePuzzleInstance = Instantiate(
            match,
            placeholder.position,
            placeholder.rotation,
            placeholder // parent to placeholder (optional)
        );
    }
}
