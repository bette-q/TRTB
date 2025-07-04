using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("Assign EITHER an EventSequence OR a single EventAction.", MessageType.Info);

        var eventSeqProp = serializedObject.FindProperty("eventSeq");
        var eventActionProp = serializedObject.FindProperty("eventAction");

        // Disable EventSequence if EventAction is assigned
        using (new EditorGUI.DisabledScope(eventActionProp.objectReferenceValue != null))
        {
            EditorGUILayout.PropertyField(eventSeqProp);
        }
        // Disable EventAction if EventSequence is assigned
        using (new EditorGUI.DisabledScope(eventSeqProp.objectReferenceValue != null))
        {
            EditorGUILayout.PropertyField(eventActionProp);
        }

        // Error warning if both are set (shouldn't be possible from Inspector)
        if (eventSeqProp.objectReferenceValue != null && eventActionProp.objectReferenceValue != null)
        {
            EditorGUILayout.HelpBox("Both EventSequence and EventAction are assigned! Only one should be used.", MessageType.Error);
        }

        // Draw the rest of the inspector (other fields from child classes)
        DrawPropertiesExcluding(serializedObject, "eventSeq", "eventAction", "m_Script");

        serializedObject.ApplyModifiedProperties();
    }
}
