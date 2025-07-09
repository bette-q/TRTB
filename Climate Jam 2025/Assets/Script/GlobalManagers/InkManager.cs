using UnityEngine;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarterAssets;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance { get; private set; }

    [Header("Ink JSON Asset")]
    public TextAsset inkJSONAsset;
    private Story story;
    private readonly Dictionary<string, Action<List<string>>> commandHandlers = new();
    private bool isWaitingForInput;
    private string currentSpeakingTo;

    public event Action<string> OnLine;
    public event Action<List<string>> OnChoices;
    public event Action OnDialogueEnd;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        commandHandlers["show_item"] = a => UIManager.Instance.ShowItem(a[0]);
        commandHandlers["show_popup"] = a => UIManager.Instance.ShowPopup(a[0]);
        commandHandlers["enable"] = a => UIManager.Instance.EnablePanel(a[0]);
        commandHandlers["add_notebook"] = a => GameStateManager.Instance.AddEvidenceById(a[0]);
        commandHandlers["speaking_to"] = a =>
        {
            if (a.Count == 0) return;
            currentSpeakingTo = a[0];
            UIManager.Instance.ArrangeCharacters(
                GameStateManager.Instance.GetCurrentCharacter(),
                currentSpeakingTo
            );
        };
    }

    void Update()
    {
        if (isWaitingForInput && Input.GetMouseButtonDown(0)) // Left mouse button only
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
        ContinueByPlayer();
    }

    public void ContinueByPlayer()
    {
        UIManager.Instance.HideItem();
        UIManager.Instance.HideDialogue();

        if (isWaitingForInput || story == null) return;
        isWaitingForInput = true;

        if (!story.canContinue)
        {
            UIManager.Instance.HideItem();
            UIManager.Instance.HideDialogue();
            UIManager.Instance.HideCharacters();
            OnDialogueEnd?.Invoke();
            isWaitingForInput = false;
            return;
        }

        string raw = story.Continue().Trim();
        string line = raw.TrimStart();

        //parse tag to set up speaker
        if (story.currentTags != null)
            foreach (string tag in story.currentTags)
                ParseAndDispatchCommand(tag.StartsWith("#") ? tag : "#" + tag);

        // Extract speaker
        string speaker = "";
        string content = line;
        int colon = line.IndexOf(':');
        if (colon > 0 && colon < 20)
        {
            speaker = line[..colon].Trim();
            content = line[(colon + 1)..].Trim();
        }

        UIManager.Instance.ShowSpeaker(speaker); // <-- Only the correct one is shown
        UIManager.Instance.ShowDialogue(speaker, content);
        OnLine?.Invoke(line);
    }

    void ParseAndDispatchCommand(string line)
    {
        // #command(arg1,arg2) style
        var mParen = Regex.Match(line, @"^#\s*(\w+)\s*\(([^)]*)\)");
        if (mParen.Success)
        {
            ExecuteCommand(
                mParen.Groups[1].Value.ToLower(),
                ParseArguments(mParen.Groups[2].Value)
            );
            return;
        }

        // #command arg1 arg2 ... (with quoted args supported)
        var mSpace = Regex.Match(line, @"^#\s*(\w+)\s+(.+)$");
        if (mSpace.Success)
        {
            ExecuteCommand(
                mSpace.Groups[1].Value.ToLower(),
                ParseArguments(mSpace.Groups[2].Value.Trim())
            );
            return;
        }

        Debug.LogWarning($"[InkManager] Unrecognized command line: {line}");
    }


    void ExecuteCommand(string cmd, List<string> args)
    {
        cmd = cmd.ToLower();
        if (commandHandlers.TryGetValue(cmd, out var h)) h(args);
        else Debug.LogWarning($"[InkManager] No handler for #{cmd}");
    }

    List<string> ParseArguments(string raw)
    {
        var list = new List<string>();
        var r = new Regex("\"([^\"]*)\"|([^,]+)");
        foreach (Match m in r.Matches(raw))
            list.Add((m.Groups[1].Success ? m.Groups[1] : m.Groups[2]).Value.Trim());
        return list;
    }
}


