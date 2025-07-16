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