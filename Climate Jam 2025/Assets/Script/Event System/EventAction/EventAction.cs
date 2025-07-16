using UnityEngine;
using System.Collections.Generic;

public abstract class EventAction : ScriptableObject
{
    [SerializeReference] private List<Prerequisite> prerequisites = new List<Prerequisite>();

    public bool ArePrerequisitesMet()
    {
        foreach (var prereq in prerequisites)
        {
            if (prereq != null && !prereq.IsMet())
                return false;
        }
        return true;
    }

    public abstract void Execute();
}