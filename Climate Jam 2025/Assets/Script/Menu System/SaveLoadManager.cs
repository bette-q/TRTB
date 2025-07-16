using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveLoadManager : MonoBehaviour
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    // ---- SAVE ----
    public void SaveGame()
    {
        var gsm = GameStateManager.Instance;
        var save = new GameSaveData
        {
            currentCharacter = gsm.currentCharacter,
            partyMembers = new List<CharacterID>(gsm.GetPartyMembers()),
            playerPosition = gsm.playerPosition,
            playerRotationY = gsm.playerRotationY,
            availableBlocks = new List<EvidenceBlock>(gsm.GetAvailableBlocks()),
            currentChapter = gsm.currentChapter,
            completedChapters = new List<string>(gsm.completedChapters),
            chapters = new Dictionary<int, ChapterProgress>(gsm.GetChapters()),
            triggeredActions = new List<string>(),
            triggeredSequences = new List<string>()
        };

#if UNITY_EDITOR
        foreach (var ea in gsm.GetTriggeredActions())
            if (ea) save.triggeredActions.Add(UnityEditor.AssetDatabase.GetAssetPath(ea));
        foreach (var es in gsm.GetTriggeredSequences())
            if (es) save.triggeredSequences.Add(UnityEditor.AssetDatabase.GetAssetPath(es));
#else
        // TODO: Use your own unique string ID system for events in builds
#endif

        var json = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game saved to " + SavePath);
    }

    // ---- LOAD ----
    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found at " + SavePath);
            return;
        }

        var gsm = GameStateManager.Instance;
        var json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<GameSaveData>(json);

        gsm.SetCurrentCharacter(save.currentCharacter);
        gsm.SetPartyMembers(save.partyMembers);
        gsm.playerPosition = save.playerPosition;
        gsm.playerRotationY = save.playerRotationY;
        gsm.SetAvailableBlocks(save.availableBlocks);
        gsm.currentChapter = save.currentChapter;
        gsm.completedChapters = new List<string>(save.completedChapters);
        gsm.SetChapters(save.chapters);

        var loadedActions = new HashSet<EventAction>();
        var loadedSequences = new HashSet<EventSequence>();
#if UNITY_EDITOR
        foreach (var path in save.triggeredActions)
        {
            var ea = UnityEditor.AssetDatabase.LoadAssetAtPath<EventAction>(path);
            if (ea) loadedActions.Add(ea);
        }
        foreach (var path in save.triggeredSequences)
        {
            var es = UnityEditor.AssetDatabase.LoadAssetAtPath<EventSequence>(path);
            if (es) loadedSequences.Add(es);
        }
#else
        // TODO: Use your own unique ID system for build
#endif
        gsm.SetTriggeredActions(loadedActions);
        gsm.SetTriggeredSequences(loadedSequences);

        Debug.Log("Game loaded from " + SavePath);
    }
}
