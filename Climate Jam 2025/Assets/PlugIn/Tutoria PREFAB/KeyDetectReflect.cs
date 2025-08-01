using UnityEngine;

/// <summary>
/// 当按下指定按键时，依次向多个 Animator 发送触发器，并激活多个指定的 GameObject。
/// </summary>
public class TriggerAndActivate : MonoBehaviour
{
    [Header("触发按键")]
    [Tooltip("按下此键时，会向 animators 发送 trigger，并激活 objectsToEnable")]
    public KeyCode activationKey = KeyCode.T;

    [Header("Animator 列表")]
    [Tooltip("会依次向这些 Animator 发送 Trigger")]
    public Animator[] animators;

    [Header("触发器名称")]
    [Tooltip("要在以上 Animator 上调用的 Trigger 参数名")]
    public string triggerName = "Play";

    [Header("要激活的物体列表")]
    [Tooltip("按下按键后，会将这些物体 SetActive(true)")]
    public GameObject[] objectsToEnable;

    // Update 在每帧执行一次
    void Update()
    {
        // 检测按下的是否为 activationKey
        if (Input.GetKeyDown(activationKey))
        {
            // 1. 向所有 Animator 发射 Trigger
            if (animators != null && animators.Length > 0)
            {
                foreach (var anim in animators)
                {
                    if (anim != null && !string.IsNullOrEmpty(triggerName))
                    {
                        anim.SetTrigger(triggerName);
                    }
                }
            }

            // 2. 激活所有指定的 GameObject
            if (objectsToEnable != null && objectsToEnable.Length > 0)
            {
                foreach (var go in objectsToEnable)
                {
                    if (go != null)
                    {
                        go.SetActive(true);
                    }
                }
            }
        }
    }
}