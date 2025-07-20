using UnityEngine;
using System;

[Serializable]
public abstract class Prerequisite
{
    public abstract bool IsMet();
}

[Serializable]
public class StoryFlagPrerequisite : Prerequisite
{
    [SerializeField] private int chapter;
    [SerializeField] private int mission;
    [SerializeField] private string flag;
    //[SerializeField] private bool requiredValue = true;

    public override bool IsMet()
    {
        return GameStateManager.Instance.GetFlag(chapter, mission, flag);
    }
}
public class EvidencePrerequisite : Prerequisite
{
    [SerializeField] private string id;
    //[SerializeField] private bool requiredValue = true;

    public override bool IsMet()
    {
        return GameStateManager.Instance.HasBlock(id);
    }
}

public class CharacterPrerequisite : Prerequisite
{
    [SerializeField] private CharacterID id;
    //[SerializeField] private bool requiredValue = true;

    public override bool IsMet()
    {
        return GameStateManager.Instance.HasCharacter(id);
    }
}

