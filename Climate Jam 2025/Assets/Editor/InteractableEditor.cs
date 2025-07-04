using UnityEditor;
using UnityEngine;

// Make sure this applies to all Interactable subclasses
[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the target as Interactable
        Interactable interactable = (Interactable)target;

        EditorGUILayout.HelpBox("Assign EITHER an EventSequence OR a single EventAction.\n(If you assign one, the other will be hidden in the inspector.)", MessageType.Info);

        // Only allow one to be assigned at a time through the inspector
        if (interactable.eventSeq == null)
        {
            interactable.eventAction = (EventAction)EditorGUILayout.ObjectField(
                "Event Action", interactable.eventAction, typeof(EventAction), false);
        }
        if (interactable.eventAction == null)
        {
            interactable.eventSeq = (EventSequence)EditorGUILayout.ObjectField(
                "Event Sequence", interactable.eventSeq, typeof(EventSequence), false);
        }

        // Show a warning if both are set (possible via script/serialization)
        if (interactable.eventSeq != null && interactable.eventAction != null)
        {
            EditorGUILayout.HelpBox("Both EventSequence and EventAction are assigned! Only one should be used.", MessageType.Error);
        }

        // Ensure changes are saved
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
