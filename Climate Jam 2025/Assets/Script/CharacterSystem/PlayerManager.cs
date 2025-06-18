using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] characters; // assign all CharacterN children of Player here in Inspector
    public TMP_Text profileText;
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

    void SetActiveCharacter(int idx)
    {
        profileText.text = $"{idx + 1}";

        for (int i = 0; i < characters.Length; i++)
        {
            // Only enable the MeshRenderer of the selected character
            var renderer = characters[i].GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = (i == idx);
        }
        activeIndex = idx;
    }

    public int GetActiveIndex() => activeIndex;
}
