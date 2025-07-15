using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EvidenceDatabase : MonoBehaviour
{
    public static EvidenceDatabase Instance { get; private set; }

    [Header("All EvidenceData (auto-filled in editor)")]
    public List<EvidenceData> allEvidenceData = new List<EvidenceData>();

    [Header("All SecComboData (auto-filled in editor)")]
    public List<ComboData> allSecComboData = new List<ComboData>();

    [Header("All FinalComboData (auto-filled in editor)")]
    public List<ComboData> allFinalComboData = new List<ComboData>();

    [Header("All registered EvidenceInfo (runtime view only)")]
    [SerializeField]
    private List<EvidenceInfo> allEvidenceInfoList = new List<EvidenceInfo>();

    // Lookup by evidence id (generic, special, combo/virtual)
    public Dictionary<string, EvidenceInfo> evidenceInfoDict = new Dictionary<string, EvidenceInfo>();
    private Dictionary<string, ComboData> secComboDict = new Dictionary<string, ComboData>();
    private Dictionary<string, ComboData> finalComboDict = new Dictionary<string, ComboData>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        evidenceInfoDict.Clear();

        // Register all EvidenceData (SO) and their special evidence
        foreach (var ed in allEvidenceData)
        {
            if (ed == null) continue;
            // Register generic info
            if (ed.info != null && !string.IsNullOrEmpty(ed.info.id))
            {
                if (!evidenceInfoDict.ContainsKey(ed.info.id))
                    evidenceInfoDict.Add(ed.info.id, ed.info);
                else
                    Debug.LogWarning($"Duplicate EvidenceData ID: {ed.info.id}");
            }
            // Register special evidence if present
            if (ed.specialEvidence != null && !string.IsNullOrEmpty(ed.specialEvidence.id))
            {
                if (!evidenceInfoDict.ContainsKey(ed.specialEvidence.id))
                    evidenceInfoDict.Add(ed.specialEvidence.id, ed.specialEvidence);
                else
                    Debug.LogWarning($"Duplicate SpecialEvidence ID: {ed.specialEvidence.id}");
            }
        }

        // Register all ComboData virtual evidence (use resultEvidence)
        foreach (var cd in allFinalComboData.Concat(allSecComboData))
        {
            if (cd != null && cd.resultEvidence != null && !string.IsNullOrEmpty(cd.resultEvidence.id))
            {
                if (!evidenceInfoDict.ContainsKey(cd.resultEvidence.id))
                    evidenceInfoDict.Add(cd.resultEvidence.id, cd.resultEvidence);
                else
                    Debug.LogWarning($"Duplicate Combo resultEvidence ID: {cd.resultEvidence.id}");
            }
        }

        allEvidenceInfoList = evidenceInfoDict.Values.ToList();


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
    public EvidenceData GetEvidenceData(string id)
    {
        foreach (var ed in allEvidenceData)
        {
            if (ed != null)
            {
                if (ed.info != null && ed.info.id == id)
                    return ed;
                if (ed.specialEvidence != null && ed.specialEvidence.id == id)
                    return ed;
            }
        }
        return null;
    }

    // Lookup ANY EvidenceInfo by id (generic, special, or combo/virtual)
    public EvidenceInfo GetEvidenceInfo(string id)
    {
        evidenceInfoDict.TryGetValue(id, out var info);
        return info;
    }

    public ComboData GetFinalComboByResult(string resultId)
    {
        finalComboDict.TryGetValue(resultId, out var cd);
        return cd;
    }

    public ComboData GetSecComboByResult(string resultId)
    {
        secComboDict.TryGetValue(resultId, out var cd);
        return cd;
    }

   // public IReadOnlyList<EvidenceData> AllEvidence => allEvidenceData;
    public IReadOnlyList<ComboData> AllFinalCombos => allFinalComboData;
    public IReadOnlyList<ComboData> AllSecCombos => allSecComboData;
}
