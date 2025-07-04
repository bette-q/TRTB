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
        action.Execute(); // Assuming your EventAction has an Execute() method
    }
}
