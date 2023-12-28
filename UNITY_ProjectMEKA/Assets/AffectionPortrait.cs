using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AffectionPortrait : MonoBehaviour
{
    public Image characterImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI affectionLevel;

    private Character character;

    public void SetCharacter(Character character)
    {
        this.character = character;

        UpdatePortrait();
    }

    public void UpdatePortrait()
    {
        characterImage.sprite = Resources.Load<Sprite>(character.CharacterHead);
        characterName.SetText(character.Name);
        affectionLevel.SetText($"{character.affection.AffectionLevel}");
        

        LayoutRebuilder.ForceRebuildLayoutImmediate(characterName.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(affectionLevel.rectTransform);
    }
}
