using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboData", menuName = "Evidence/ComboData", order = 1)]
public class ComboData : ScriptableObject
{
    public List<string> comboOrder; // Ordered IDs required for combo
    public EvidenceInfo resultEvidence; // Output combo's EvidenceData
}
