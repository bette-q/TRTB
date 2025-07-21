#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Prerequisite), true)]
public class PrerequisiteDrawer : PropertyDrawer
{
    private string[] prerequisiteTypeNames = new string[]
    {
        "Story Flag Prerequisite",
        "Evidence Prerequisite",
        "Character Prerequisite"
    };

    private System.Type[] prerequisiteTypes = new System.Type[]
    {
        typeof(StoryFlagPrerequisite),
        typeof(EvidencePrerequisite),
        typeof(CharacterPrerequisite)
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // If the managed reference is null, create a default instance
        if (property.managedReferenceValue == null)
        {
            property.managedReferenceValue = new StoryFlagPrerequisite();
        }

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float indent = 15f;

        // Draw the main label
        Rect labelRect = new Rect(position.x, position.y, position.width, lineHeight);
        EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

        float currentY = position.y + lineHeight + spacing;

        // Get current type index
        int currentTypeIndex = GetCurrentTypeIndex(property);

        // Type selection dropdown
        Rect typeRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
        int newTypeIndex = EditorGUI.Popup(typeRect, "Prerequisite Type", currentTypeIndex, prerequisiteTypeNames);

        // If type changed, create new instance
        if (newTypeIndex != currentTypeIndex)
        {
            property.managedReferenceValue = System.Activator.CreateInstance(prerequisiteTypes[newTypeIndex]);
        }

        currentY += lineHeight + spacing;

        // Draw fields based on current type
        DrawFieldsForCurrentType(position, property, currentY, indent);

        EditorGUI.EndProperty();
    }

    private int GetCurrentTypeIndex(SerializedProperty property)
    {
        if (property.managedReferenceValue == null) return 0;

        System.Type currentType = property.managedReferenceValue.GetType();
        for (int i = 0; i < prerequisiteTypes.Length; i++)
        {
            if (prerequisiteTypes[i] == currentType)
                return i;
        }
        return 0;
    }

    private void DrawFieldsForCurrentType(Rect position, SerializedProperty property, float startY, float indent)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float currentY = startY;

        if (property.managedReferenceValue is StoryFlagPrerequisite)
        {
            var chapterProp = property.FindPropertyRelative("chapter");
            var missionProp = property.FindPropertyRelative("mission");
            var flagProp = property.FindPropertyRelative("flag");

            Rect chapterRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
            EditorGUI.PropertyField(chapterRect, chapterProp, new GUIContent("Chapter"));
            currentY += lineHeight + spacing;

            Rect missionRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
            EditorGUI.PropertyField(missionRect, missionProp, new GUIContent("Mission"));
            currentY += lineHeight + spacing;

            Rect flagRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
            EditorGUI.PropertyField(flagRect, flagProp, new GUIContent("Flag"));
        }
        else if (property.managedReferenceValue is EvidencePrerequisite)
        {
            var idProp = property.FindPropertyRelative("id");

            Rect idRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
            EditorGUI.PropertyField(idRect, idProp, new GUIContent("Evidence ID"));
        }
        else if (property.managedReferenceValue is CharacterPrerequisite)
        {
            var idProp = property.FindPropertyRelative("id");

            Rect idRect = new Rect(position.x + indent, currentY, position.width - indent, lineHeight);
            EditorGUI.PropertyField(idRect, idProp, new GUIContent("Character"));
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float baseHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing; // Label + Type dropdown

        if (property.managedReferenceValue is StoryFlagPrerequisite)
        {
            return baseHeight + EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 3; // 3 fields
        }
        else if (property.managedReferenceValue is EvidencePrerequisite || property.managedReferenceValue is CharacterPrerequisite)
        {
            return baseHeight + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // 1 field
        }

        return baseHeight;
    }
}
#endif