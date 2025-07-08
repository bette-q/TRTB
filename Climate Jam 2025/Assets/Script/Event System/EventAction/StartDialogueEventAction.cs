using UnityEngine;

[CreateAssetMenu(menuName = "EventSystem/Actions/DialogueEventAction")]
public class StartDialogueEventAction : EventAction
{
    [Header("Ink Dialogue Knot")]
    public string inkKnot;

    public override void Execute()
    {
        // No "speaking_to" or target assignment here¡ªInk script handles it.
        InkManager.Instance.StartDialogue(inkKnot);

        // (Optional) You can subscribe to OnDialogueEnd for follow-up EAs.
        // InkManager.Instance.OnDialogueEnd += OnDialogueComplete;
    }

    // Uncomment if you want to handle follow-up logic or chaining:
    // private void OnDialogueComplete()
    // {
    //     InkManager.Instance.OnDialogueEnd -= OnDialogueComplete;
    //     // Trigger next event, etc.
    // }
}
