using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.LightingExplorerTableColumn;

//all evidence related object defined here
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

[CreateAssetMenu(fileName = "EvidenceData", menuName = "Evidence/EvidenceData", order = 0)]
public class EvidenceData : ScriptableObject
{
    public EvidenceInfo info;

    [Tooltip("Character-specific override. If set and the interacting character matches, this will be shown instead of the generic info.")]
    public SpecialEvidenceInfo specialEvidence;

    [Tooltip("Info: not in deduction, Evidence: will be able to link in deduction")]
    public EvidenceDataType type = EvidenceDataType.Null;
}
public enum EvidenceDataType
{
    Evidence,
    Info,
    Null
}

public enum EvidenceBlockType
{
    Info, //non-deduction block
    Evidence,   // Regular collected evidence
    SecCombo,   //secondary combo
    FinalCombo  //final deduction -> used for ending eval
}

// Stored in Notebook ED -> EB
[System.Serializable]
public class EvidenceBlock
{
    public string id;
    //public EvidenceInfo info;   
    public EvidenceBlockType blockType;

    public bool missionFinished;

    public EvidenceBlock prev;
    public EvidenceBlock next;

    public EvidenceInfo info => EvidenceDatabase.Instance.GetEvidenceInfo(id);

    public EvidenceBlock(string infoId, EvidenceDataType type)
    {
        this.id = infoId;
        //this.info = infoIn;
        this.blockType = (type == EvidenceDataType.Info)
                            ? EvidenceBlockType.Info
                            : EvidenceBlockType.Evidence; ;
        missionFinished = false;
    }

    public EvidenceBlock(string infoId, EvidenceBlockType type)
    {
        this.id = infoId;
        //this.info = infoIn;
        this.blockType = type;
        missionFinished = false;
    }
}



