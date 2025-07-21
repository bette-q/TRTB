using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    void Awake()
    {
        // Basic singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Entry point for triggering event sequences
    public void Execute(EventSequence sequence)
    {
        if (sequence == null)
        {
            Debug.LogWarning("EventManager: Tried to execute a null sequence.");
            return;
        }

        // Check if already triggered
        if (GameStateManager.Instance.HasTriggered(sequence))
        {
            Debug.Log($"EventSequence {sequence.name} already triggered.");
            return;
        }

        // Check prerequisites
        if (!sequence.ArePrerequisitesMet())
        {
            Debug.Log($"EventSequence {sequence.name} prerequisites not met.");
            return;
        }

        if (!sequence.IsRepeat())
        {
            GameStateManager.Instance.MarkAsTriggered(sequence);
        }
        sequence.Execute();
    }

    // Overload: Execute a single EventAction
    public void Execute(EventAction action)
    {
        if (action == null)
        {
            Debug.LogWarning("EventManager: Tried to execute a null EventAction.");
            return;
        }

        // Check if already triggered
        if (GameStateManager.Instance.HasTriggered(action))
        {
            Debug.Log($"EventAction {action.name} already triggered.");
            return;
        }

        // Check prerequisites
        if (!action.ArePrerequisitesMet())
        {
            Debug.Log($"EventAction {action.name} prerequisites not met.");
            return;
        }

        if (!action.IsRepeat())
        {
            GameStateManager.Instance.MarkAsTriggered(action);
        }
        action.Execute(); // Assuming your EventAction has an Execute() method
    }
}
