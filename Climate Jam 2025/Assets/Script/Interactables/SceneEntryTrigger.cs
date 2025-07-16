using UnityEngine;

public class SceneEntryTrigger : MonoBehaviour
{
    [SerializeField] private EventAction enterSceneDialogueAction;
    [SerializeField] private EventSequence enterSceneSequence;

    void Start()
    {
        // Try to execute the enter scene dialogue action
        if (enterSceneDialogueAction != null)
        {
            EventManager.Instance.Execute(enterSceneDialogueAction);
        }

        // Or try to execute the enter scene sequence
        if (enterSceneSequence != null)
        {
            EventManager.Instance.Execute(enterSceneSequence);
        }
    }
}