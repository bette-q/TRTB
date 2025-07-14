using UnityEngine;

public class Orbit360 : MonoBehaviour
{
    public Transform cameraPivot;    // OrbitRoot
    public Transform cameraRig;      // 相机本身

    public float distance = 5.0f;
    public float xSpeed = 100f; // 水平
    public float ySpeed = 80f;  // 垂直

    public float yMin = -30f;
    public float yMax = 80f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Vector3 angles = cameraRig.localEulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        // 初始相机位置
        cameraRig.localPosition = new Vector3(0, 0, -distance);
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * xSpeed * Time.deltaTime;
        pitch -= mouseY * ySpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, yMin, yMax);

        cameraPivot.rotation = Quaternion.Euler(0, yaw, 0);
        cameraRig.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}