using UnityEngine;
using System.Collections.Generic;

public class EvidenceDatabase : MonoBehaviour
{
    public static EvidenceDatabase Instance { get; private set; }

    [Header("All EvidenceData (auto-filled in editor)")]
    public List<EvidenceData> allEvidenceData = new List<EvidenceData>();

    [Header("All SecComboData (auto-filled in editor)")]
    public List<ComboData> allSecComboData = new List<ComboData>();

    [Header("All FinalComboData (auto-filled in editor)")]
    public List<ComboData> allFinalComboData = new List<ComboData>();


    // Fast lookup dictionaries (ID to object)
    private Dictionary<string, EvidenceData> evidenceDict = new Dictionary<string, EvidenceData>();
    private Dictionary<string, ComboData> finalComboDict = new Dictionary<string, ComboData>();
    private Dictionary<string, ComboData> secComboDict = new Dictionary<string, ComboData>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build evidence dictionary
        evidenceDict.Clear();
        foreach (var ed in allEvidenceData)
        {
            if (ed != null && ed.info != null && !string.IsNullOrEmpty(ed.info.id))
            {
                if (!evidenceDict.ContainsKey(ed.info.id))
                    evidenceDict.Add(ed.info.id, ed);
                else
                    Debug.LogWarning($"Duplicate EvidenceData ID: {ed.info.id}");
            }
        }

        // Build final combo dictionary (by result evidence id)
        finalComboDict.Clear();
        foreach (var cd in allFinalComboData)
        {
            if (cd != null && cd.resultEvidence != null && !string.IsNullOrEmpty(cd.resultEvidence.id))
            {
                if (!finalComboDict.ContainsKey(cd.resultEvidence.id))
                    finalComboDict.Add(cd.resultEvidence.id, cd);
                else
                    Debug.LogWarning($"Duplicate FinalComboData Result ID: {cd.resultEvidence.id}");
            }
        }

        // Build sec combo dictionary (by result evidence id)
        secComboDict.Clear();
        foreach (var cd in allSecComboData)
        {
            if (cd != null && cd.resultEvidence != null && !string.IsNullOrEmpty(cd.resultEvidence.id))
            {
                if (!secComboDict.ContainsKey(cd.resultEvidence.id))
                    secComboDict.Add(cd.resultEvidence.id, cd);
                else
                    Debug.LogWarning($"Duplicate SecComboData Result ID: {cd.resultEvidence.id}");
            }
        }
    }

    // Lookup Evidence by id
    public EvidenceData GetEvidence(string id)
    {
        evidenceDict.TryGetValue(id, out var ed);
        return ed;
    }

    // Lookup FinalCombo by result id
    public ComboData GetFinalComboByResult(string resultId)
    {
        finalComboDict.TryGetValue(resultId, out var cd);
        return cd;
    }

    // Lookup SecCombo by result id
    public ComboData GetSecComboByResult(string resultId)
    {
        secComboDict.TryGetValue(resultId, out var cd);
        return cd;
    }

    // Optionally: Expose lists as read-only for UI/iteration
    public IReadOnlyList<EvidenceData> AllEvidence => allEvidenceData;
    public IReadOnlyList<ComboData> AllFinalCombos => allFinalComboData;
    public IReadOnlyList<ComboData> AllSecCombos => allSecComboData;
}
