using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public abstract class EventAction : ScriptableObject
{
    [SerializeReference] private List<Prerequisite> prerequisites = new List<Prerequisite>();
    [SerializeField] private bool repeat;

    public bool ArePrerequisitesMet()
    {
        foreach (var prereq in prerequisites)
        {
            if (prereq != null && !prereq.IsMet())
                return false;
        }
        return true;
    }
    public bool IsRepeat()
    { return repeat; }

    public abstract void Execute();
}