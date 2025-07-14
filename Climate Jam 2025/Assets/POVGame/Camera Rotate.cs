using UnityEngine;

public class CameraViewTrigger : MonoBehaviour
{
    [Header("Camera to Check")]
    public Transform cameraTransform;  // 主相机 Transform
    public Transform target;           // 被观察的目标物体（如雕像、机关）

    [Header("Trigger Settings")]
    public float maxViewAngle = 10f;   // 夹角容忍度（越小越精确）

    [Header("Only Trigger Once")]
    public bool triggerOnce = true;
    private bool hasTriggered = false;

    [Header("Objects to Activate")]
    public GameObject[] objectsToActivate;

    [Header("Animator to Trigger")]
    public Animator targetAnimator;
    public string triggerName = "Activate";

    void Update()
    {
        if (cameraTransform == null || target == null || (triggerOnce && hasTriggered)) return;

        // 从目标朝向相机的方向向量
        Vector3 toCamera = (cameraTransform.position - target.position).normalized;
        Vector3 forward = target.forward;

        // 计算夹角
        float angle = Vector3.Angle(forward, toCamera);

        if (angle <= maxViewAngle)
        {
            foreach (var obj in objectsToActivate)
            {
                if (obj != null) obj.SetActive(true);
            }

            if (targetAnimator != null && !string.IsNullOrEmpty(triggerName))
            {
                targetAnimator.SetTrigger(triggerName);
            }

            if (triggerOnce) hasTriggered = true;
        }
    }
}