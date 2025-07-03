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

    //private IEnumerator ExecuteSequence(EventSequence sequence)
    //{
    //    // TEMP: Just log the sequence name, since EventSequence is not yet defined
    //    //Debug.Log($"[EventManager] Executing sequence: {sequence.name}");
    //    // TODO: Loop through actions
    //    yield return null;
    //}
}
