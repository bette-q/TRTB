using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EventSystem/EventSequence")]
public class EventSequence : ScriptableObject
{
    public List<EventAction> actions = new List<EventAction>();

    public void Execute()
    {
        foreach (var action in actions)
        {
            if (action != null)
                action.Execute();
        }
    }
}
