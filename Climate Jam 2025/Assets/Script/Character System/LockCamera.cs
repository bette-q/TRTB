using UnityEngine;

// Attach this script to your PlayerCameraRoot object
public class LockToWorldRotation : MonoBehaviour
{
    void LateUpdate()
    {
        // Always keep this object world-aligned
        transform.rotation = Quaternion.identity;
    }
}
