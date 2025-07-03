using System.Collections.Generic;
using TMPro;
using UnityEngine;


//handles character switch
public class PlayerManager : MonoBehaviour
{
    public GameObject[] characters; // assign all CharacterN children of Player here in Inspector
    public CharacterID[] characterIDs = new CharacterID[4]; // Assign these in Inspector (0=Investigator, 1=Reporter, etc.)

    public TMP_Text profileText;
    private int activeIndex = 0;

    private List<CharacterID> playableList = new List<CharacterID>();

    void Start()
    {
        UpdatePlayableList();
        SetActiveCharacter(GameStateManager.Instance.GetCurrentCharacter());
    }

    void Update()
    {
        if (NotebookUIManager.IsOpen)
            return;

        // Refresh the playable list in case new characters have joined
        UpdatePlayableList();

        for (int i = 0; i < playableList.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetActiveCharacter(playableList[i]);
                break;
            }
        }
    }

    void SetActiveCharacter(CharacterID id)
    {
        int idx = System.Array.IndexOf(characterIDs, id);
        if (idx < 0) return;

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

    void UpdatePlayableList()
    {
        playableList = GameStateManager.Instance.GetSwitchableCharacters();
    }
    public int GetActiveIndex() => activeIndex;
}
