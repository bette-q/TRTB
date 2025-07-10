using UnityEngine;
using UnityEngine.UI;

public class AutoDisableOnAlphaZero : MonoBehaviour
{
    public float checkInterval = 0.1f;  // 检查频率（秒）
    public float alphaThreshold = 0.01f; // 判定为“透明”的容差

    private Graphic graphic;
    private CanvasGroup canvasGroup;

    void Start()
    {
        // 尝试获取 Graphic 组件（Image, Text, etc.）
        graphic = GetComponent<Graphic>();
        canvasGroup = GetComponent<CanvasGroup>();

        // 开始周期检测
        InvokeRepeating(nameof(CheckAlpha), 0f, checkInterval);
    }

    void CheckAlpha()
    {
        float alpha = 1f;

        if (canvasGroup != null)
        {
            alpha = canvasGroup.alpha;
        }
        else if (graphic != null)
        {
            alpha = graphic.color.a;
        }

        if (alpha <= alphaThreshold)
        {
            gameObject.SetActive(false);
        }
    }
}