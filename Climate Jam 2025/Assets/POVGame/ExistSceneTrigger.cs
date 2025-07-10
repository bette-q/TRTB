using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // 用于 UI Image

public class SceneSwitchOnAlpha : MonoBehaviour
{
    public string sceneToLoad;
    public float threshold = 0.99f;
    public float checkInterval = 0.1f;

    private Image uiImage;
    private SpriteRenderer spriteRenderer;
    private bool triggered = false;

    void Start()
    {
        // 尝试获取 UI Image 或 SpriteRenderer
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (uiImage == null && spriteRenderer == null)
        {
            Debug.LogError("[SceneSwitchOnAlpha] ❌ 本物体上没有 Image 或 SpriteRenderer 组件！");
            enabled = false;
            return;
        }

        InvokeRepeating(nameof(CheckAlpha), 0f, checkInterval);
    }

    void CheckAlpha()
    {
        float currentAlpha = -1f;

        if (uiImage != null)
        {
            currentAlpha = uiImage.color.a;
        }
        else if (spriteRenderer != null)
        {
            currentAlpha = spriteRenderer.color.a;
        }

        if (!triggered && currentAlpha >= threshold)
        {
            triggered = true;
            Debug.Log($"[SceneSwitchOnAlpha] Alpha = {currentAlpha}, loading scene: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}