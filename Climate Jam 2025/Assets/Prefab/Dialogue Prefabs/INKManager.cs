using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class LinearInkController : MonoBehaviour
{
    [Header("Ink JSON Asset")]
    public TextAsset inkJSONAsset;

    [Header("UI")]
    public TMP_Text nameText;      // 🟢 角色名字显示框
    public TMP_Text storyText;     // 🟢 对话内容显示框
    public Button continueButton;  // 🟢 点击继续按钮

    public CharacterManager characterManager; // 🟢 控制立绘切图的系统
    public SimpleSceneManager SceneManager;

    private Story story;
    private bool isWaitingForInput = true; // 新增字段，防止多次触发

    void Start()
    {
        story = new Story(inkJSONAsset.text);
        ShowNextLine();
        continueButton.onClick.AddListener(ShowNextLine);
    }

    void Update()
    {
        // 检测任意按键按下，并确保只触发一次
        if (Input.anyKeyDown && isWaitingForInput)
        {
            isWaitingForInput = false; // 标记为已处理输入
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (story.canContinue)
        {
            string text = story.Continue().Trim();

            // 🧠 提取说话人和对白
            if (text.Contains(":"))
            {
                string[] parts = text.Split(':');
                string speaker = parts[0].Trim();
                string dialogue = parts[1].Trim();

                nameText.text = speaker;
                storyText.text = dialogue;
            }
            else
            {
                // 没有冒号就认为是旁白
                nameText.text = "";
                storyText.text = text;
            }

            // 🎯 读取当前行的标签
            foreach (string tag in story.currentTags)
            {
                HandleTag(tag);
            }
        }
        else
        {
            if (SceneManager != null)
            {
                SceneManager.LoadNextScene();
            }
            continueButton.interactable = false;
        }

        isWaitingForInput = true; // 允许下一次输入
    }

    void HandleTag(string tag)
    {
        Debug.Log("收到标签：" + tag);
        string[] parts = tag.TrimStart('#', '^', '~').Trim().Split(' ');

        if (parts.Length == 3 && parts[0] == "show")
        {
            string characterName = parts[1];
            string spriteTag = parts[2];

            Debug.Log($"[Ink Tag] 显示角色：{characterName}（表情：{spriteTag}）");
            characterManager.ForceShowCharacter(characterName, spriteTag);
        }

        else if (parts.Length == 2 && parts[0] == "hide")
        {
            string characterName = parts[1];
            Debug.Log($"[Ink Tag] 隐藏角色：{characterName}");
            characterManager.HideCharacter(characterName);
        }

        else if (parts.Length == 2 && parts[0] == "hide" && parts[1] == "all")
        {
            foreach (var character in characterManager.characters)
            {
                character.image.gameObject.SetActive(false);
            }
        }
    }
}