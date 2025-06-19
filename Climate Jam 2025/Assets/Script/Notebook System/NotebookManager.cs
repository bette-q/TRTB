using UnityEngine;
using System.Collections.Generic;

// handles notebook ? tbd
public class NotebookManager : MonoBehaviour
{
    public static NotebookManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Add evidence via GSM
    public void AddEvidence(EvidenceBlock block)
    {
        // Call the authority in GameStateManager
        if (GameStateManager.Instance.AddEvidence(block))
        {
            Debug.Log($"Collected evidence: {block.title} ({block.text})");
            // Optionally, update notebook UI here
        }
        // If evidence already exists, nothing happens
    }

    // To display notebook, always fetch from GSM
    public IReadOnlyList<EvidenceBlock> GetCollectedBlocks()
    {
        return GameStateManager.Instance.GetCollectedBlocks();
    }
}
