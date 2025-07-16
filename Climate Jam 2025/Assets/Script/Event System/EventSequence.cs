using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EventSystem/EventSequence")]
public class EventSequence : ScriptableObject
{
    public List<EventAction> actions = new List<EventAction>();
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

    public void Execute()
    {
        foreach (var action in actions)
        {
            if (action != null)
                action.Execute();
        }
    }
}