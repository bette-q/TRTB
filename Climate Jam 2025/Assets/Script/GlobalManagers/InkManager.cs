//using UnityEngine;
//using Ink.Runtime;
//using System;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using System.Linq;

//public class InkManager : MonoBehaviour
//{
//    public static InkManager Instance { get; private set; }

//    [Header("Ink JSON Asset")]
//    public TextAsset inkJSONAsset;

//    private Story story;

//    // Events for UI hookup
//    public event Action<string> OnLine;              // Called for dialogue/narration lines
//    public event Action<List<string>> OnChoices;     // Called to display player choices
//    public event Action OnDialogueEnd;               // Called when dialogue is finished

//    // #command dispatcher
//    private Dictionary<string, Action<List<string>>> commandHandlers = new Dictionary<string, Action<List<string>>>();

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        // Register default #command handlers
//        RegisterDefaultHandlers();
//    }

//    void RegisterDefaultHandlers()
//    {
//        // Example mappings, expand as needed
//        commandHandlers["add_notebook"] = args => GameStateManager.Instance.AddEvidence(args[0]);
//        commandHandlers["update_flag"] = args => SyncFlag(args[0], args[1]);
//        commandHandlers["play_sfx"] = args => AudioManager.Instance.PlaySfx(args[0]);
//        commandHandlers["advance_phase"] = args => GSM.Instance.AdvancePhase(args[0], args[1]);
//        commandHandlers["show_item"] = args => UIManager.Instance.ShowItem(args[0]);
//        commandHandlers["enable_ui"] = args => UIManager.Instance.EnablePanel(args[0]);
//        commandHandlers["show_popup"] = args => UIManager.Instance.ShowPopup(args[0]);
//        commandHandlers["add_notebook_block"] = args => NotebookManager.Instance.AddInfoBlock(args[0]);
//        // Add other custom handlers here...
//    }

//    // Example: For syncing flags between Ink and GSM
//    void SyncFlag(string flagName, string value)
//    {
//        bool boolValue = value.ToLower() == "true";
//        GSM.Instance.SetFlag(flagName, boolValue);
//        if (story != null)
//        {
//            // Also sync to Ink global variables if they exist
//            if (story.variablesState.Contains(flagName))
//                story.variablesState[flagName] = boolValue;
//        }
//    }

//    // Allow external registration of new #command handlers if needed
//    public void RegisterCommand(string command, Action<List<string>> handler)
//    {
//        commandHandlers[command.ToLower()] = handler;
//    }

//    public void StartDialogue(string knot)
//    {
//        story = new Story(inkJSONAsset.text);

//        // Register external functions for querying only
//        story.BindExternalFunction("HAS_FLAG", (string flagName) => GSM.Instance.GetFlag(flagName));
//        story.BindExternalFunction("HAS_EVIDENCE", (string id) => NotebookManager.Instance.HasEvidence(id));
//        story.BindExternalFunction("GET_PHASE", (string missionID) => GSM.Instance.GetPhase(missionID));
//        story.BindExternalFunction("GET_ACTIVE_CHARACTER", () => PlayerManager.Instance.CurrentCharacterID);
//        // Register other query functions as needed...

//        story.ChoosePathString(knot);
//        ContinueStory();
//    }

//    public void ContinueStory(int choiceIndex = -1)
//    {
//        if (choiceIndex >= 0) story.ChooseChoiceIndex(choiceIndex);

//        while (story.canContinue)
//        {
//            string text = story.Continue().Trim();

//            if (string.IsNullOrEmpty(text))
//                continue;

//            // Parse #command lines
//            if (text.StartsWith("#"))
//            {
//                ParseAndDispatchCommand(text);
//                continue;
//            }

//            // Section headers (optional to skip)
//            if (text.StartsWith("==="))
//                continue;

//            // Otherwise, dispatch dialogue/narration to UI
//            OnLine?.Invoke(text);
//        }

//        // Present choices, if any
//        if (story.currentChoices.Count > 0)
//        {
//            var choices = new List<string>();
//            foreach (var c in story.currentChoices)
//                choices.Add(c.text.Trim());
//            OnChoices?.Invoke(choices);
//        }
//        else
//        {
//            OnDialogueEnd?.Invoke();
//        }
//    }

//    void ParseAndDispatchCommand(string line)
//    {
//        // Regex: #command param1 param2 ... (quoted string parameters allowed)
//        var match = Regex.Match(line, @"^#(\w+)\s*(.*)$");
//        if (!match.Success)
//        {
//            Debug.LogWarning($"[InkManager] Could not parse command line: {line}");
//            return;
//        }

//        string command = match.Groups[1].Value.ToLower(); // e.g. add_notebook
//        string paramStr = match.Groups[2].Value;

//        // Parse parameters, supporting quoted strings and normal words
//        var paramList = ParseParameters(paramStr);

//        if (commandHandlers.TryGetValue(command, out var handler))
//        {
//            handler?.Invoke(paramList);
//        }
//        else
//        {
//            Debug.LogWarning($"[InkManager] Unrecognized #command: {command} ({string.Join(", ", paramList)})");
//        }
//    }

//    // Parse parameters: supports "quoted string" and bare words
//    List<string> ParseParameters(string paramStr)
//    {
//        var matches = Regex.Matches(paramStr, @"""([^""]+)""|(\S+)");
//        var list = new List<string>();
//        foreach (Match m in matches)
//        {
//            if (m.Groups[1].Success)
//                list.Add(m.Groups[1].Value);
//            else
//                list.Add(m.Groups[2].Value);
//        }
//        return list;
//    }
//}
