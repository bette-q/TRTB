using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class TriggerActivateAndFadeLight : MonoBehaviour
{
    [Header("Player Detection")]
    [Tooltip("Tag for the player GameObject")]
    public string playerTag = "Player";

    [Header("Objects to Toggle on Trigger")]
    [Tooltip("玩家在触发器内时要显示，离开时隐藏的物体；按 E 后不会再显示")]
    public GameObject[] objectsToToggle;

    [Header("Light Fade Settings")]
    [Tooltip("要渐变的 Light 组件")]
    public Light targetLight;
    [Tooltip("从 0 到目标强度的持续时间（秒）")]
    public float fadeDuration = 1.0f;
    [Tooltip("渐变结束时的最终强度")]
    public float targetIntensity = 2f;

    // 内部状态
    private bool playerInRange = false;
    private bool isFading = false;
    private bool actionTaken = false; // 一旦按下 E 处理完毕，就不再触发后续逻辑

    void Awake()
    {
        // 确保 Collider 设置为 Trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"[{nameof(TriggerActivateAndFadeLight)}] Collider 不是 Trigger，已自动将其设置为 Trigger。");
            col.isTrigger = true;
        }

        // 一开始先隐藏所有 objectsToToggle
        if (objectsToToggle != null)
        {
            foreach (var go in objectsToToggle)
            {
                if (go != null)
                    go.SetActive(false);
            }
        }

        // 如果指定了 Light，将其初始强度设为 0
        if (targetLight != null)
        {
            targetLight.intensity = 0f;
        }
        else
        {
            Debug.LogWarning($"[{nameof(TriggerActivateAndFadeLight)}] 未指定 targetLight，按下 E 时不会有渐变效果。");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (actionTaken) return; // 已经执行过按键动作，不再响应

        // 玩家进入触发时
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;

            // 显示所有指定物体
            if (objectsToToggle != null)
            {
                foreach (var go in objectsToToggle)
                {
                    if (go != null)
                        go.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (actionTaken) return; // 已经执行过按键动作，不再响应

        // 玩家离开时
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;

            // 隐藏所有指定物体
            if (objectsToToggle != null)
            {
                foreach (var go in objectsToToggle)
                {
                    if (go != null)
                        go.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        if (actionTaken) return; // 如果已经按过 E 并完成动作，就完全不再触发

        // 如果玩家在范围内并且按下 E 键，且当前没有在渐变中
        if (playerInRange && !isFading && Input.GetKeyDown(KeyCode.E))
        {
            actionTaken = true;

            // 隐藏所有指定物体（按下 E 时，不再显示）
            if (objectsToToggle != null)
            {
                foreach (var go in objectsToToggle)
                {
                    if (go != null)
                        go.SetActive(false);
                }
            }

            // 如果有光源，启动渐变协程
            if (targetLight != null)
            {
                StartCoroutine(FadeLightIntensity(0f, targetIntensity, fadeDuration));
            }
        }
    }

    /// <summary>
    /// 将 targetLight 从 start 强度过度到 end 强度，持续 time 秒
    /// </summary>
    private IEnumerator FadeLightIntensity(float start, float end, float time)
    {
        if (targetLight == null) yield break;

        isFading = true;

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            targetLight.intensity = Mathf.Lerp(start, end, t);
            yield return null;
        }

        // 确保最终值
        targetLight.intensity = end;
        isFading = false;
    }
}