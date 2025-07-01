using UnityEngine;
using System.Collections.Generic;

//all evidence related object defined here

public enum CharacterID
{
    one,
    two,
    three,
    four,
    Null
}

[System.Serializable]
public class EvidenceInfo
{
    [Tooltip("Unique string ID for this evidence. Must not duplicate any other evidence.")]
    public string id;

    [Tooltip("Display name shown in UI (e.g., notebook or popups).")]
    public string displayName;

    [Tooltip("Short title used as the headline for this evidence.")]
    public string title;

    [Tooltip("Detailed description or notes shown to the player.")]
    [TextArea]
    public string text;

    [Tooltip("Icon to visually represent this evidence.")]
    public Sprite icon;
}

[System.Serializable]
public class SpecialEvidenceInfo : EvidenceInfo
{
    public CharacterID characterID = CharacterID.Null;
}

[CreateAssetMenu(menuName = "Evidence/EvidenceData")]
public class EvidenceData : ScriptableObject
{
    public EvidenceInfo info;

    [Tooltip("Character-specific override. If set and the interacting character matches, this will be shown instead of the generic info.")]
    public SpecialEvidenceInfo specialEvidence;
}

public enum EvidenceBlockType
{
    Evidence,   // Regular collected evidence
    SecCombo,   //secondary combo
    FinalCombo  //final deduction -> used for ending eval
}

// Stored in Notebook ED -> EB
[System.Serializable]
public class EvidenceBlock
{
    public string id;
    public EvidenceInfo info;   
    public EvidenceBlockType blockType;
    

    public EvidenceBlock prev;
    public EvidenceBlock next;

    public EvidenceBlock(EvidenceInfo infoIn, EvidenceBlockType type = EvidenceBlockType.Evidence)
    {
        this.id = infoIn.id;
        this.info = infoIn;
        this.blockType = type;
    }
}



