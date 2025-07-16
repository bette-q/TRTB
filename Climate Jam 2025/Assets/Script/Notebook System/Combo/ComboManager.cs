using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Stores all valid combos and checks for them
public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Returns a matching combo recipe, or null if not found
    //public ComboData FindValidCombo(List<string> selectedIDs, EvidenceBlockType type)
    //{
    //    IReadOnlyList<ComboData> comboList = (type == EvidenceBlockType.Evidence)
    //        ? EvidenceDatabase.Instance.AllSecCombos
    //        : EvidenceDatabase.Instance.AllFinalCombos;

    //    foreach (var combo in comboList)
    //    {
    //        if (combo.comboOrder.Count == selectedIDs.Count &&
    //            combo.comboOrder.SequenceEqual(selectedIDs))
    //        {
    //            return combo;
    //        }
    //    }
    //    return null;
    //}
}
