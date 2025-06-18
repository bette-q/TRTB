using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("目标设置")]
    [SerializeField] private string targetTag = "Player"; // 目标标签
    [SerializeField] private Transform targetOverride; // 可选：直接指定目标对象

    [Header("跟随参数")]
    [SerializeField] private float followSpeed = 5f; // 跟随速度
    [SerializeField] private bool followRotation = false; // 是否跟随目标旋转

    [Header("位置参数")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 5, -10); // 位置偏移
    [SerializeField] private bool useLocalOffset = true; // 是否使用局部坐标系偏移

    [Header("旋转参数")]
    [SerializeField] private Vector3 fixedRotation = new Vector3(30, 0, 0); // 固定旋转角度
    [SerializeField] private bool lookAtTarget = false; // 是否始终看向目标

    private Transform target;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 initialOffset;

    void Start()
    {
        // 查找目标对象
        FindTarget();

        // 记录初始偏移量
        if (target != null)
            initialOffset = transform.position - target.position;
    }

    void FindTarget()
    {
        // 优先使用直接指定的目标
        if (targetOverride != null)
        {
            target = targetOverride;
            return;
        }

        // 否则通过标签查找
        GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
        else
        {
            Debug.LogError($"找不到标签为 '{targetTag}' 的游戏对象!");
        }
    }

    void LateUpdate()
    {
        // 如果没有目标，尝试重新查找
        if (target == null)
        {
            FindTarget();
            return;
        }

        // 计算目标位置
        Vector3 targetPosition;

        if (useLocalOffset)
        {
            // 使用局部坐标系偏移（原始代码的核心逻辑）
            targetPosition = target.position + target.TransformDirection(positionOffset);
        }
        else
        {
            // 使用世界坐标系偏移
            targetPosition = target.position + positionOffset;
        }

        // 平滑移动相机到目标位置
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            1f / followSpeed
        );

        // 设置相机旋转
        if (lookAtTarget)
        {
            // 看向目标
            transform.LookAt(target);
        }
        else if (!followRotation)
        {
            // 使用固定旋转角度
            transform.rotation = Quaternion.Euler(fixedRotation);
        }
    }
}
