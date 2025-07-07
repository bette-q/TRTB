using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(EvidenceDatabase))]
public class EvidenceDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Auto-Fill Evidence & Separate Combos"))
        {
            var db = (EvidenceDatabase)target;

            // EvidenceData
            string evidencePath = "Assets/ScriptableData/Evidence";
            string[] evidenceGuids = AssetDatabase.FindAssets("t:EvidenceData", new[] { evidencePath });
            db.allEvidenceData = evidenceGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<EvidenceData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(ed => ed != null)
                .ToList();

            // FinalComboData
            string finalComboPath = "Assets/ScriptableData/FinalCombo";
            string[] finalComboGuids = AssetDatabase.FindAssets("t:ComboData", new[] { finalComboPath });
            db.allFinalComboData = finalComboGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<ComboData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(cd => cd != null)
                .ToList();

            // SecComboData
            string secComboPath = "Assets/ScriptableData/SecCombo";
            string[] secComboGuids = AssetDatabase.FindAssets("t:ComboData", new[] { secComboPath });
            db.allSecComboData = secComboGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<ComboData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(cd => cd != null)
                .ToList();

            EditorUtility.SetDirty(db);
            Debug.Log($"[EvidenceDatabase] Evidence: {db.allEvidenceData.Count}, FinalCombos: {db.allFinalComboData.Count}, SecCombos: {db.allSecComboData.Count}");
        }
    }
}
