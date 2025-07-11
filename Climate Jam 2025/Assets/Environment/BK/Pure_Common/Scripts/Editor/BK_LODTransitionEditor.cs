using UnityEngine;
using UnityEditor;

namespace BKPureNature
{
    public class LODTransitionEditor
{
    [MenuItem("Tools/BK/Apply LOD Transitions/0.00")]
    public static void ApplyFadeWidth_0_00()
    {
        ApplyToSelected(0.00f);
    }

    [MenuItem("Tools/BK/Apply LOD Transitions/0.25")]
    public static void ApplyFadeWidth_0_25()
    {
        ApplyToSelected(0.25f);
    }

    [MenuItem("Tools/BK/Apply LOD Transitions/0.50")]
    public static void ApplyFadeWidth_0_50()
    {
        ApplyToSelected(0.5f);
    }

    [MenuItem("Tools/BK/Apply LOD Transitions/0.75")]
    public static void ApplyFadeWidth_0_75()
    {
        ApplyToSelected(0.75f);
    }

    [MenuItem("Tools/BK/Apply LOD Transitions/1.00")]
    public static void ApplyFadeWidth_1_00()
    {
        ApplyToSelected(1.00f);
    }

    // You can add more predefined values as needed.

    private static void ApplyToSelected(float fadeValue)
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            LODGroup lodGroup = go.GetComponent<LODGroup>();
            if (lodGroup)
            {
                var lods = lodGroup.GetLODs();
                for (int i = 0; i < lods.Length; i++)
                {
                    lods[i].fadeTransitionWidth = fadeValue;
                }
                lodGroup.SetLODs(lods);
            }
        }
    }
}
}
