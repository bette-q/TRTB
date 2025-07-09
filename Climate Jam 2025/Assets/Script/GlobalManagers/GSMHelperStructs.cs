using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    public Vector3 ToVector3() => new Vector3(x, y, z);
}

[System.Serializable]
public class MissionProgress
{
    public Dictionary<string, bool> flags = new(); // flagName ¡ú value
}

[System.Serializable]
public class ChapterProgress
{
    public Dictionary<string, MissionProgress> missions = new(); // missionName ¡ú MissionProgress
}

[System.Serializable]
public class GameSaveData
{
    // Player
    public CharacterID currentCharacter;
    public List<CharacterID> partyMembers = new();
    public SerializableVector3 playerPosition;
    public float playerRotationY;

    // Notebook
    public List<EvidenceBlock> availableBlocks = new();

    // Progress
    public string currentChapter;
    public List<string> completedChapters = new();
    public Dictionary<string, ChapterProgress> chapters = new();

    // Triggers (use string IDs for build compatibility, see notes)
    public List<string> triggeredActions = new();
    public List<string> triggeredSequences = new();
}