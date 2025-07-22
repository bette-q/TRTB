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
    //private string currentSpeakingTo;

    private List<string> currentSpeakers = new List<string>();

    public event Action<string> OnLine;
    public event Action<List<string>> OnChoices;
    public event Action OnDialogueEnd;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        commandHandlers["show_item"] = a => UIManager.Instance.ShowItem(a[0]);
        commandHandlers["show_popup"] = a => UIManager.Instance.ShowPopup(a[0]);
        commandHandlers["enable"] = a => UIManager.Instance.EnablePanel(a[0]);
        commandHandlers["speakers"] = a =>
        {
            currentSpeakers = a;
            string leftName = a.Count > 0 ? a[0] : "";
            string rightName = a.Count > 1 ? a[1] : "";
            UIManager.Instance.ArrangeCharacters(leftName, rightName);
        };
        commandHandlers["blackout"] = a =>
        {
            bool turnOn = a[0].ToLower() == "on" ? true : false;
            UIManager.Instance.SetBlackOut(turnOn);
        };

        commandHandlers["add_notebook"] = a => GameStateManager.Instance.AddEvidenceById(a[0]);
        commandHandlers["addtoparty"] = args => {
            if (args.Count == 0)
            {
                Debug.LogWarning("[InkManager] #AddToParty called with no name");
                return;
            }
            var name = args[0];
            if (!System.Enum.TryParse<CharacterID>(name, true, out var charID))
            {
                Debug.LogWarning($"[InkManager] #AddToParty: Unknown CharacterID '{name}'");
                return;
            }
            GameStateManager.Instance.AddSwitchableCharacter(charID);

            var npcGO = AddCharacterEventContext.CurrentSourceGameObject;
            if (npcGO != null)
            {
                Debug.Log($"[AddCharacterEventAction] Destroying NPC object: {npcGO.name}");
                UnityEngine.Object.Destroy(npcGO);
            }
            else
            {
                Debug.LogWarning("[AddCharacterEventAction] No NPC GameObject context found to destroy.");
            }
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

    void Start()
    {
        InitializeStory();
    }

    void InitializeStory()
    {
        if (inkJSONAsset != null)
        {
            story = new Story(inkJSONAsset.text);

            // Bind external functions once
            story.BindExternalFunction("SET_FLAG", (int chapterId, int missionId, string flagName) =>
            {
                GameStateManager.Instance.SetFlag(chapterId, missionId, flagName);
            });

            story.BindExternalFunction("GET_FLAG", (int chapterId, int missionId, string flagName) =>
            {
                return GameStateManager.Instance.GetFlag(chapterId, missionId, flagName);
            });
            story.BindExternalFunction("get_current_character", () =>
            {
                return GameStateManager.Instance.GetCurrentCharacter().ToString();
            });
        }
        else
        {
            Debug.LogError("InkJSONAsset is not assigned!");
        }
    }

    public void StartDialogue(string knot)
    {
        if (story == null)
        {
            Debug.LogError("Story not initialized! Make sure inkJSONAsset is assigned.");
            return;
        }

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
            //UIManager.Instance.SetBlackOut(false);
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

        string speakerID = speaker.Replace(" ", "").ToLower();
        UIManager.Instance.ShowSpeaker(speakerID); // <-- Only the correct one is shown
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
        var r = new Regex("\"([^\"]*)\"|(\\S+)");
        foreach (Match m in r.Matches(raw))
            list.Add(m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value);
        return list;
    }

}


