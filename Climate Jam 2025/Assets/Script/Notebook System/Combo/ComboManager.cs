using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//stores all valid combos and checks for them
public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    public List<ComboData> secCombos;
    public List<ComboData> finalCombos;

    void Awake()
    {
        Instance = this;
    }

    // Returns a matching combo recipe, or null if not found
    public ComboData FindValidCombo(List<string> selectedIDs, EvidenceBlockType type)
    {
        var comboList = (type == EvidenceBlockType.Evidence)
            ? secCombos        // List<ComboData> for EB¡úSecCombo
            : finalCombos;     // List<ComboData> for SecCombo¡úFinalCombo

        foreach (var combo in comboList)
        {
            if (combo.comboOrder.Count == selectedIDs.Count &&
                combo.comboOrder.SequenceEqual(selectedIDs))
            {
                return combo;
            }
        }
        return null;
    }

}
