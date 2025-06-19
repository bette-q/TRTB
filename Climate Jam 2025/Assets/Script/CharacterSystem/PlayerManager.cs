using TMPro;
using UnityEngine;

//handles character switch
public class PlayerManager : MonoBehaviour
{
    public GameObject[] characters; // assign all CharacterN children of Player here in Inspector
    public CharacterID[] characterIDs = new CharacterID[4]; // Assign these in Inspector (0=Investigator, 1=Reporter, etc.)

    public TMP_Text profileText;
    private int activeIndex = 0;



    void Start()
    {
        SetActiveCharacter(0);
    }

    void Update()
    {
        if (NotebookUIManager.IsOpen)
            return;

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
            var renderer = characters[i].GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = (i == idx);
        }
        activeIndex = idx;

        // Update GameStateManager with new active character
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SwitchCharacter(characterIDs[idx]);
    }


    public int GetActiveIndex() => activeIndex;
}
