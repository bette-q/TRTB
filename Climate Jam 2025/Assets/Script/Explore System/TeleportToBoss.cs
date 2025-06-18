///说明：可用于黑屏传送，传送坐标使用空物体定位

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TeleportWithDelayTrigger : MonoBehaviour
{
    [Header("玩家 Tag")]
    public string playerTag = "Player";

    [Header("激活物体")]
    public GameObject objectToActivate;

    [Header("传送目标（空物体）")]
    public Transform teleportTarget;

    [Header("延迟时长")]
    public float delaySeconds = 3f;

    bool _triggered = false;

    void Awake()
    {
        var col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning($"[{nameof(TeleportWithDelayTrigger)}] 自动将 Collider 设为 Trigger");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag(playerTag)) return;
        if (teleportTarget == null)
        {
            Debug.LogError($"[{nameof(TeleportWithDelayTrigger)}] 没有指定 teleportTarget");
            return;
        }

        _triggered = true;
        Debug.Log("[TeleportWithDelayTrigger] 触发，激活物体并开始延时传送");

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        StartCoroutine(TeleportAfterDelay(other.gameObject));
    }

    IEnumerator TeleportAfterDelay(GameObject player)
    {
        Debug.Log($"[TeleportWithDelayTrigger] 等待 {delaySeconds} 秒");
        yield return new WaitForSeconds(delaySeconds);
        Debug.Log("[TeleportWithDelayTrigger] 时间到，开始传送");

        // 尝试禁用 CharacterController 防止瞬移时卡住
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // 瞬移
        player.transform.position = teleportTarget.position;
        Debug.Log($"[TeleportWithDelayTrigger] 已将玩家传送到 {teleportTarget.position}");

        // 恢复 CharacterController
        if (cc != null) cc.enabled = true;
    }
}