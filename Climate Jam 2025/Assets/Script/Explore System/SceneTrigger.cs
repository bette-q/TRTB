///说明：在接触碰撞箱后立刻切换场景

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class SceneTrigger : MonoBehaviour
{
    [Header("要加载的场景名")]
    [Tooltip("在 Build Settings（File > Build Settings…）里确保此场景已被加到场景列表")]
    public string sceneName;

    [Header("触发者 Tag")]
    [Tooltip("只有带此 Tag 的物体进入 Trigger 时才会切换场景")]
    public string playerTag = "Player";

    void Awake()
    {
        // 确保 Collider 被设置为 Trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning($"[{nameof(SceneTrigger)}] 自动将 Collider 设置为 isTrigger = true");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 只对指定 Tag 响应
        if (!other.CompareTag(playerTag)) return;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[{nameof(SceneTrigger)}] 未指定 sceneName，无法加载场景");
            return;
        }

        // 立即加载指定场景
        SceneManager.LoadScene(sceneName);
    }
}