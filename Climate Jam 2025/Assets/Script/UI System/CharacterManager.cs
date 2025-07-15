using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum CharacterID
{
    Player,
    Qiu,
    Ella,
    Mateo,
    Null
}

[System.Serializable]
public class CharacterSprite
{
    public string tagName;
    public Sprite sprite;
}

[System.Serializable]
public class CharacterData
{
    public string name;             // e.g. "Ella", "Mateo", "fd"
    public CharacterID characterID; // controllable, else Null for NPCs
    public GameObject prefab;       // UI prefab (with Image)
    public List<CharacterSprite> sprites;
}

public class CharacterManager : MonoBehaviour
{
    [Header("Character Setup")]
    public List<CharacterData> characters;

    [Header("Slots in UI")]
    public RectTransform leftCharacterSlot;
    public RectTransform rightCharacterSlot;

    private GameObject currentLeftCharacter;
    private GameObject currentRightCharacter;
    private string leftName;
    private string rightName;

    // Destroy any characters currently in slots
    public void HideAllCharacters()
    {
        if (currentLeftCharacter) { Destroy(currentLeftCharacter); currentLeftCharacter = null; leftName = ""; }
        if (currentRightCharacter) { Destroy(currentRightCharacter); currentRightCharacter = null; rightName = ""; }
    }

    // Instantiate left (controlled) and right (target/NPC) characters
    public void ArrangeForDialogue(string leftName, string rightName)
    {
        HideAllCharacters();

        // Left = controlled
        var leftData = characters.Find(c => c.name.ToLower() == leftName.ToLower());
        if (leftData != null && leftData.prefab != null)
        {
            currentLeftCharacter = Instantiate(leftData.prefab, leftCharacterSlot, false);
            SetupCharacterImage(currentLeftCharacter, leftCharacterSlot);
            this.leftName = leftData.name.ToLower();
        }

        // Right = NPC/target
        var rightData = characters.Find(c => c.name.ToLower() == rightName.ToLower());
        if (rightData != null && rightData.prefab != null)
        {
            currentRightCharacter = Instantiate(rightData.prefab, rightCharacterSlot, false);
            SetupCharacterImage(currentRightCharacter, rightCharacterSlot);
            this.rightName = rightData.name.ToLower();
        }

        if (currentLeftCharacter) currentLeftCharacter.SetActive(false);
        if (currentRightCharacter) currentRightCharacter.SetActive(false);
    }

    // Ensures image fits slot, preserves aspect
    void SetupCharacterImage(GameObject go, RectTransform slot)
    {
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.anchoredPosition = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;

        var img = go.GetComponent<Image>();
        if (img) img.preserveAspect = true;
    }

    // Shows only the speaking character; hides the other
    public void ShowSpeaker(string speakerName)
    {
        string s = speakerName.ToLower();
        bool leftIsSpeaker = !string.IsNullOrEmpty(leftName) && leftName == s;
        bool rightIsSpeaker = !string.IsNullOrEmpty(rightName) && rightName == s;

        if (currentLeftCharacter) currentLeftCharacter.SetActive(leftIsSpeaker);
        if (currentRightCharacter) currentRightCharacter.SetActive(rightIsSpeaker);

        // Debug
        Debug.Log($"[CharacterManager] ShowSpeaker: speaker={speakerName}, left={leftName}, right={rightName}, leftIsSpeaker={leftIsSpeaker}, rightIsSpeaker={rightIsSpeaker}");
    }



    // Optional: Change sprite for a character in slot
    public void ChangeSprite(string side, string tagName)
    {
        GameObject target = side == "left" ? currentLeftCharacter : currentRightCharacter;
        if (!target) return;
        var image = target.GetComponent<Image>();
        if (!image) return;

        CharacterData charData = characters.Find(c => c.prefab.name == target.name.Replace("(Clone)", "").Trim());
        if (charData != null)
        {
            var spriteData = charData.sprites.Find(s => s.tagName == tagName);
            if (spriteData != null && spriteData.sprite != null)
                image.sprite = spriteData.sprite;
        }
    }
}
