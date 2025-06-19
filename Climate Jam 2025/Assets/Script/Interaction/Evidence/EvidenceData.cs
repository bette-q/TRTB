using UnityEngine;
using System.Collections.Generic;

//all evidence related object defined here

public enum CharacterID
{
    one,
    two,
    three,
    four
}

[System.Serializable]
public class SpecialEvidenceBlock
{
    public CharacterID characterID;
    public string title;

    [TextArea]
    public string description;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "EvidenceData", menuName = "Game/Evidence Data")]
public class EvidenceData : ScriptableObject
{
    public string id;
    public string displayName;

    public string genericTitle;
    [TextArea]
    public string genericText;
    public Sprite genericIcon;

    public List<SpecialEvidenceBlock> specialBlocks;
}

// Stored in Notebook ED -> EB
[System.Serializable]
public class EvidenceBlock
{
    public string id;        // Source ED id
    public string title;     // Title to show in notebook
    public string text;      // Description
    public Sprite icon;      // Optional, for display

    public EvidenceBlock(string id, string title, string text, Sprite icon = null)
    {
        this.id = id;
        this.title = title;
        this.text = text;
        this.icon = icon;
    }
}
