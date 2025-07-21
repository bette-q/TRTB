using UnityEngine;
using System.Collections;

public class SceneEntryTrigger : MonoBehaviour
{
    [SerializeField] private EventAction enterSceneDialogueAction;
    [SerializeField] private EventSequence enterSceneSequence;

    void OnEnable()
    {
        StartCoroutine(DelayedTrigger());
    }

    IEnumerator DelayedTrigger()
    {
        // Wait for 1 frame (you can increase to 2¨C3 if needed)
        yield return null;

        if (enterSceneDialogueAction != null)
            EventManager.Instance.Execute(enterSceneDialogueAction);

        if (enterSceneSequence != null)
            EventManager.Instance.Execute(enterSceneSequence);
    }


}