using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ComboBlock
{
    public List<string> ingredientIDs;     // Ordered list of IDs required for combo
    public EvidenceData resultEvidence;    // Deduction block ED asset
}

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    public List<ComboBlock> allCombos;

    void Awake()
    {
        Instance = this;
    }

    // Returns a matching combo recipe, or null if not found
    public ComboBlock FindValidCombo(List<string> selectedIDs)
    {
        foreach (var combo in allCombos)
        {
            if (combo.ingredientIDs.Count == selectedIDs.Count &&
                combo.ingredientIDs.SequenceEqual(selectedIDs))
            {
                return combo;
            }
        }
        return null;
    }
}
