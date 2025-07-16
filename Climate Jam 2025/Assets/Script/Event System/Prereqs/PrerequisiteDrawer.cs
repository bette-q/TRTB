#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Prerequisite), true)]
public class PrerequisiteDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // If the managed reference is null, create a new StoryFlagPrerequisite instance
        if (property.managedReferenceValue == null)
        {
            property.managedReferenceValue = new StoryFlagPrerequisite();
        }

        // Draw the label
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

        // Draw the fields for StoryFlagPrerequisite
        DrawStoryFlagFields(position, property);

        EditorGUI.EndProperty();
    }

    private void DrawStoryFlagFields(Rect position, SerializedProperty property)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float indent = 15f;

        // Find the serialized properties for StoryFlagPrerequisite
        var chapterProp = property.FindPropertyRelative("chapter");
        var missionProp = property.FindPropertyRelative("mission");
        var flagProp = property.FindPropertyRelative("flag");

        float currentY = position.y + lineHeight + spacing;

        // Create rects for each field with slight indentation
        Rect chapterRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
        currentY += lineHeight + spacing;

        Rect missionRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
        currentY += lineHeight + spacing;

        Rect flagRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);

        // Draw the property fields
        EditorGUI.PropertyField(chapterRect, chapterProp, new GUIContent("Chapter"));
        EditorGUI.PropertyField(missionRect, missionProp, new GUIContent("Mission"));
        EditorGUI.PropertyField(flagRect, flagProp, new GUIContent("Flag"));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Label height + 3 fields + spacing between them
        return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
    }
}
#endif