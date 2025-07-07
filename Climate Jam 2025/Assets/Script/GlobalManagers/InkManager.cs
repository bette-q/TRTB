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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        commandHandlers["show_popup"] = args => UIManager.Instance.ShowPopup(args[0]);
        commandHandlers["enable"] = args => UIManager.Instance.EnablePanel(args[0]);
        commandHandlers["add_notebook"] = args => GameStateManager.Instance.AddEvidenceById(args[0]);
        commandHandlers["show"] = args =>
        {
            if (args[0].ToLower() == "item" && args.Count > 1)
                UIManager.Instance.ShowItem(args[1]);
        };
    }

    public void StartDialogue(string knot)
    {
        story = new Story(inkJSONAsset.text);
        story.ChoosePathString(knot);
        ContinueStory();
    }

    public void ContinueStory(int choiceIndex = -1)
    {
        if (choiceIndex >= 0) story.ChooseChoiceIndex(choiceIndex);

        while (story.canContinue)
        {
            string text = story.Continue().Trim();
            if (string.IsNullOrEmpty(text))
                continue;

            // Handle commands in #command(args) or #command arg1 arg2 format
            if (text.StartsWith("#"))
            {
                ParseAndDispatchCommand(text);
                continue;
            }
            if (text.StartsWith("==="))
                continue;

            OnLine?.Invoke(text);
        }

        if (story.currentChoices.Count > 0)
        {
            var choices = new List<string>();
            foreach (var c in story.currentChoices)
                choices.Add(c.text.Trim());
            OnChoices?.Invoke(choices);
        }
        else
        {
            OnDialogueEnd?.Invoke();
        }
    }

    void ParseAndDispatchCommand(string line)
    {
        // Handles #command(args); or #command arg1 arg2
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

        // Fallback for #show Item UnknownMail etc
        var matchSpace = Regex.Match(line, @"^#\s*(\w+)\s+(\w+)\s+(\S+)$");
        if (matchSpace.Success)
        {
            string command = matchSpace.Groups[1].Value.ToLower();
            string arg0 = matchSpace.Groups[2].Value;
            string arg1 = matchSpace.Groups[3].Value;
            if (commandHandlers.TryGetValue(command, out var handler))
                handler.Invoke(new List<string> { arg0, arg1 });
            else
                Debug.LogWarning($"[InkManager] Unrecognized #command: {command} ({arg0}, {arg1})");
        }
        else
        {
            Debug.LogWarning($"[InkManager] Failed to parse command: {line}");
        }
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
