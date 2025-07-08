using UnityEngine;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance { get; private set; }

    [Header("Ink JSON Asset")]
    public TextAsset inkJSONAsset;
    private Story story;

    public event Action<string> OnLine;
    public event Action<List<string>> OnChoices;
    public event Action OnDialogueEnd;

    private Dictionary<string, Action<List<string>>> commandHandlers = new Dictionary<string, Action<List<string>>>();
    private bool isWaitingForInput = false;
    private string currentSpeakingTo = null; // Target character (right slot)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Register #command handlers
        commandHandlers["show_item"] = args => UIManager.Instance.ShowItem(args[0]);
        commandHandlers["show_popup"] = args => UIManager.Instance.ShowPopup(args[0]);
        commandHandlers["enable"] = args => UIManager.Instance.EnablePanel(args[0]);
        commandHandlers["add_notebook"] = args => GameStateManager.Instance.AddEvidenceById(args[0]);
        commandHandlers["speaking_to"] = args =>
        {
            if (args.Count > 0)
            {
                currentSpeakingTo = args[0];
                // Controlled character always on left, targetName (even NPC) on right
                UIManager.Instance.ArrangeCharacters(
                    GameStateManager.Instance.GetCurrentCharacter(),
                    currentSpeakingTo
                );
            }
        };
        //// (Optional) Sprite changing for later:
        //commandHandlers["sprite"] = args => {
        //    if (args.Count == 2)
        //        UIManager.Instance.ChangeCharacterSprite(args[0], args[1]);
        //};
    }

    void Start()
    {
        // You can auto-start here or call StartDialogue elsewhere
        // StartDialogue("start");
    }

    void Update()
    {
        // Advance dialogue on any key press when waiting for input
        if (isWaitingForInput && Input.anyKeyDown)
        {
            isWaitingForInput = false;
            ContinueByPlayer();
        }
    }

    public void StartDialogue(string knot)
    {
        story = new Story(inkJSONAsset.text);
        story.ChoosePathString(knot);
        isWaitingForInput = false;
        ContinueByPlayer(); // Show first line
    }

    public void ContinueByPlayer()
    {
        // Always hide item and dialogue before advancing
        UIManager.Instance.HideItem();
        UIManager.Instance.HideDialogue();

        if (isWaitingForInput) return;
        isWaitingForInput = true;

        if (story == null) return;

        // Handle choices
        if (story.currentChoices.Count > 0)
        {
            var choices = new List<string>();
            foreach (var c in story.currentChoices)
                choices.Add(c.text.Trim());
            OnChoices?.Invoke(choices);
            isWaitingForInput = false;
            return;
        }

        if (story.canContinue)
        {
            string text = story.Continue().Trim();

            // Handle #commands
            if (!string.IsNullOrEmpty(text) && text.StartsWith("#"))
            {
                ParseAndDispatchCommand(text);
                isWaitingForInput = false;
                return;
            }
            if (!string.IsNullOrEmpty(text) && text.StartsWith("==="))
            {
                isWaitingForInput = false;
                return;
            }

            UIManager.Instance.ShowDialogue(text);
            OnLine?.Invoke(text);
        }
        else
        {
            UIManager.Instance.HideItem();
            UIManager.Instance.HideDialogue();
            OnDialogueEnd?.Invoke();
            isWaitingForInput = false;
        }
    }

    void ParseAndDispatchCommand(string line)
    {
        // #command(arg1, arg2) style
        var matchParen = Regex.Match(line, @"^#\s*(\w+)\s*\(([^)]*)\)");
        if (matchParen.Success)
        {
            string command = matchParen.Groups[1].Value.ToLower();
            string argsRaw = matchParen.Groups[2].Value;
            var args = ParseArguments(argsRaw);
            if (commandHandlers.TryGetValue(command, out var handler))
                handler.Invoke(args);
            else
                Debug.LogWarning($"[InkManager] Unrecognized #command: {command} ({string.Join(", ", args)})");
            return;
        }

        // #command arg1 arg2 style
        var matchSpace = Regex.Match(line, @"^#\s*(\w+)\s+(.+)$");
        if (matchSpace.Success)
        {
            string command = matchSpace.Groups[1].Value.ToLower();
            var args = new List<string>(matchSpace.Groups[2].Value.Trim().Split(' '));
            if (commandHandlers.TryGetValue(command, out var handler))
                handler.Invoke(args);
            else
                Debug.LogWarning($"[InkManager] Unrecognized #command: {command} ({string.Join(", ", args)})");
            return;
        }

        Debug.LogWarning($"[InkManager] Failed to parse command: {line}");
    }

    List<string> ParseArguments(string argsRaw)
    {
        var results = new List<string>();
        var regex = new Regex("\"([^\"]*)\"|([^,]+)");
        var matches = regex.Matches(argsRaw);
        foreach (Match m in matches)
        {
            if (m.Groups[1].Success)
                results.Add(m.Groups[1].Value.Trim());
            else if (m.Groups[2].Success)
                results.Add(m.Groups[2].Value.Trim());
        }
        return results;
    }
}
