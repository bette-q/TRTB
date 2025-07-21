using UnityEngine;
using System.Collections;
using System.Reflection;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineInputAxisController))]
[RequireComponent(typeof(CinemachineOrbitalFollow))]
public class RightMouseOrbitToggle : MonoBehaviour
{
    private CinemachineInputAxisController _axisCtrl;
    private CinemachineOrbitalFollow _orbital;

    void Awake()
    {
        _axisCtrl = GetComponent<CinemachineInputAxisController>();
        _orbital = GetComponent<CinemachineOrbitalFollow>();
    }

    void OnEnable()
    {
        StartCoroutine(RandomiseNextFrame());
    }

    IEnumerator RandomiseNextFrame()
    {
        yield return null; // Wait for Cinemachine to initialize

        // Set random horizontal orbit (yaw)
        _orbital.HorizontalAxis.Value = Random.Range(0f, 360f);

        // Use reflection to get min/max for the vertical axis
        var vertAxis = _orbital.VerticalAxis;
        float min = GetAxisLimit(vertAxis, "minValue", -45f);
        float max = GetAxisLimit(vertAxis, "maxValue", 45f);

        if (Mathf.Approximately(min, max))
        {
            min = -45f; max = 45f;
        }
        _orbital.VerticalAxis.Value = Random.Range(min, max);
    }

    void Update()
    {
        // Only allow orbit when RMB is held
        _axisCtrl.enabled = Input.GetMouseButton(1);
    }

    // Helper to get private min/max values from InputAxis via reflection
    float GetAxisLimit(object axis, string fieldName, float fallback)
    {
        var field = axis.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        return (field != null && field.FieldType == typeof(float)) ? (float)field.GetValue(axis) : fallback;
    }
}
