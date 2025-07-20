using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;
using TMPro;

[ExecuteAlways]
public class PerspectiveAlignmentManager : MonoBehaviour
{
    [Header("【必填】用于检测视角的摄像机（正交）")]
    public Camera detectionCamera;

    [Header("【必填】所有正确视点的 Transform 列表")]
    public Transform[] correctViewpoints;

    [Header("允许的位置偏差（单位：米）")]
    public float distanceThreshold = 0.5f;

    [Serializable]
    public class IndexEvent : UnityEvent<int> { }

    [Header("对齐后触发，参数：当前视点索引")]
    public IndexEvent onAligned;

    // 内部记录每个点是否已触发过
    private bool[] _hasAligned;

    // ========== 新增：UI和控制 ==========
    [Header("Solved UI")]
    public GameObject popUpPanel;    
    public TextMeshProUGUI solvedText;           // 唯一文本框
    public Button returnButton;

    public MonoBehaviour cameraOrbitScript;


    void Awake()
    {
        InitState();
        if (popUpPanel) popUpPanel.SetActive(false);
    }

    void OnValidate()
    {
        // 编辑器下也保持状态数组长度一致
        InitState();
    }

    private void InitState()
    {
        if (correctViewpoints != null)
            _hasAligned = new bool[correctViewpoints.Length];
    }

    void Update()
    {
        if (detectionCamera == null || correctViewpoints == null)
            return;

        for (int i = 0; i < correctViewpoints.Length; i++)
        {
            if (_hasAligned[i])
                continue;

            var target = correctViewpoints[i];
            if (target == null)
                continue;

            // —— 改为位置检测 —— 
            float dist = Vector3.Distance(detectionCamera.transform.position, target.position);
            bool isAligned = dist <= distanceThreshold;

            // 每帧输出调试信息
            //Debug.Log($"[AlignDebug] idx={i}  dist={dist:F2}m  threshold={distanceThreshold}m  aligned={isAligned}");

            if (isAligned)
            {
                _hasAligned[i] = true;
                Debug.Log($"[AlignDebug] 🎯 Position Alignment SUCCESS for idx={i}");
                onAligned.Invoke(i);
                OnPuzzleSolved();
            }
        }
    }

    /// <summary>
    /// 重置指定索引的触发状态，使其可再次触发 onAligned
    /// </summary>
    public void ResetAlignment(int index)
    {
        if (_hasAligned == null || index < 0 || index >= _hasAligned.Length)
            return;
        _hasAligned[index] = false;
    }

    /// <summary>
    /// 重置所有视点的触发状态
    /// </summary>
    public void ResetAllAlignments()
    {
        if (_hasAligned == null)
            return;
        for (int i = 0; i < _hasAligned.Length; i++)
            _hasAligned[i] = false;
    }

    void OnDrawGizmos()
    {
        if (correctViewpoints == null || detectionCamera == null)
            return;

        for (int i = 0; i < correctViewpoints.Length; i++)
        {
            var target = correctViewpoints[i];
            if (target == null)
                continue;

            float dist = Vector3.Distance(detectionCamera.transform.position, target.position);

            // 距离小于阈值时用绿色，否则红色
            Gizmos.color = dist <= distanceThreshold ? Color.green : Color.red;
            Gizmos.DrawWireSphere(target.position, 0.5f);
            Gizmos.DrawLine(target.position, detectionCamera.transform.position);
        }
    }

    void OnPuzzleSolved()
    {
        // 锁定摄像机控制
        if (cameraOrbitScript) cameraOrbitScript.enabled = false;

        // 展示UI和文本
        if (popUpPanel) popUpPanel.SetActive(true);

        // 拼接文本：“证物名 solved!”
        string evidenceName = GameStateManager.Instance.GetCurEvidence();
        if (solvedText) solvedText.text = $"{evidenceName} solved!";

        // 按钮监听
        if (returnButton)
        {
            returnButton.onClick.RemoveAllListeners();
            returnButton.onClick.AddListener(OnReturnToGame);
        }
    }
    void OnReturnToGame()
    {
        //SceneManager.LoadScene("YourGameScene");
        Debug.Log("Return to main game scene!");
    }
}