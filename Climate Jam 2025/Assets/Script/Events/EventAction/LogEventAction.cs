using UnityEngine;

[CreateAssetMenu(menuName = "EventSystem/Actions/LogEventAction")]
public class LogEventAction : EventAction
{
    [TextArea] public string message;

    public override void Execute()
    {
        Debug.Log("[EventAction] " + message);
    }
}
