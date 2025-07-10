using UnityEngine;

public class LockCursorOnPlay : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 锁定鼠标到屏幕中央
        Cursor.visible = false;                     // 隐藏鼠标指针
        Debug.Log("[Cursor] 鼠标已锁定并隐藏");
    }

    void Update()
    {
        // 按 Esc 退出锁定（可选）
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("[Cursor] 鼠标已释放并显示");
        }
    }
}