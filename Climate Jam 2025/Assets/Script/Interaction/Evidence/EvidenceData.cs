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

public enum EvidenceBlockType
{
    Evidence,   // Regular collected evidence
    ComboBlock  // Result from a combo/deduction
}

// Stored in Notebook ED -> EB
[System.Serializable]
public class EvidenceBlock
{
    public string id;       
    public string title;    
    public string text;     
    public Sprite icon;     
    public EvidenceBlockType blockType;

    public EvidenceBlock(string id, string title, string text, Sprite icon = null, EvidenceBlockType type = EvidenceBlockType.Evidence)
    {
        this.id = id;
        this.title = title;
        this.text = text;
        this.icon = icon;
        this.blockType = type;
    }
}
