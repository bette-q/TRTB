using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum CharacterID
{
    Main,
    Qiu,
    Ella,
    Mateo,
    Null
}

[System.Serializable]
public class CharacterSprite
{
    public string tagName;     // e.g. "default", "angry"
    public Sprite sprite;
}

[System.Serializable]
public class CharacterData
{
    public string name;                 // e.g. "Mateo", "Qiu", "fd"
    public CharacterID characterID;     // For controllable, else Null for NPCs
    public GameObject prefab;           // UI prefab (with Image)
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

    /// <summary>
    /// Places controlled character (left) and target (right) by name.
    /// Destroys previous instances for each slot.
    /// </summary>
    public void ArrangeForDialogue(CharacterID controlled, string targetName)
    {
        // Destroy previous
        if (currentLeftCharacter) Destroy(currentLeftCharacter);
        if (currentRightCharacter) Destroy(currentRightCharacter);

        // Controlled (left)
        var leftData = characters.Find(c => c.characterID == controlled);
        if (leftData != null && leftData.prefab != null)
        {
            currentLeftCharacter = Instantiate(leftData.prefab, leftCharacterSlot, false);
            currentLeftCharacter.SetActive(true);
        }

        // Target/NPC (right)
        var rightData = characters.Find(c => c.name == targetName);
        if (rightData != null && rightData.prefab != null)
        {
            currentRightCharacter = Instantiate(rightData.prefab, rightCharacterSlot, false);
            currentRightCharacter.SetActive(true);
        }
    }

    /// <summary>
    /// Change sprite for character on a side ("left"/"right") by tagName
    /// </summary>
    public void ChangeSprite(string side, string tagName)
    {
        GameObject target = side == "left" ? currentLeftCharacter : currentRightCharacter;
        if (!target) return;

        var image = target.GetComponent<Image>();
        if (!image) return;

        // Find character data
        CharacterData charData = null;
        string prefabName = target.name.Replace("(Clone)", "").Trim();
        if (side == "left")
            charData = characters.Find(c => c.prefab.name == prefabName);
        else
            charData = characters.Find(c => c.prefab.name == prefabName);

        if (charData != null)
        {
            var spriteData = charData.sprites.Find(s => s.tagName == tagName);
            if (spriteData != null && spriteData.sprite != null)
                image.sprite = spriteData.sprite;
        }
    }
}
