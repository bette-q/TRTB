using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] characters; // assign all CharacterN children of Player here in Inspector
    private int activeIndex = 0;

    void Start()
    {
        SetActiveCharacter(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) SetActiveCharacter(0);
        if (Input.GetKeyDown(KeyCode.Keypad2)) SetActiveCharacter(1);
        if (Input.GetKeyDown(KeyCode.Keypad3)) SetActiveCharacter(2);
        if (Input.GetKeyDown(KeyCode.Keypad4)) SetActiveCharacter(3);
    }

    void SetActiveCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            // Only enable the MeshRenderer of the selected character
            var renderer = characters[i].GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = (i == index);
        }
        activeIndex = index;
    }

    public int GetActiveIndex() => activeIndex;
}
