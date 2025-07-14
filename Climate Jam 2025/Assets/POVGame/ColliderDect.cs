using UnityEngine;

public class PlayerTriggerActivator : MonoBehaviour
{
    [Header("Target Animator")]
    public Animator targetAnimator;             // 目标Animator
    public string triggerName = "Activate";     // 要触发的Trigger参数名

    [Header("Objects to Toggle")]
    public GameObject[] objectsToEnable;        // 需要打开的物体
    public GameObject[] objectsToDisable;       // 需要关闭的物体

    [Header("Player Tag")]
    public string playerTag = "Player";         // 玩家物体的Tag

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger] 检测到碰撞物体: {other.name}");

        if (!hasTriggered && other.CompareTag(playerTag))
        {
            hasTriggered = true;
            Debug.Log("[Trigger] 符合触发条件，开始执行动作");

            if (targetAnimator != null && !string.IsNullOrEmpty(triggerName))
            {
                targetAnimator.SetTrigger(triggerName);
                Debug.Log($"[Animator] 向 Animator '{targetAnimator.name}' 发送 Trigger: {triggerName}");
            }
            else
            {
                Debug.LogWarning("[Animator] Animator 或 TriggerName 未正确设置");
            }

            foreach (GameObject obj in objectsToEnable)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    Debug.Log($"[Enable] 启用物体: {obj.name}");
                }
                else
                {
                    Debug.LogWarning("[Enable] 检测到空引用");
                }
            }

            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    Debug.Log($"[Disable] 禁用物体: {obj.name}");
                }
                else
                {
                    Debug.LogWarning("[Disable] 检测到空引用");
                }
            }
        }
        else
        {
            Debug.Log("[Trigger] 未触发条件 或 已触发过一次");
        }
    }
}