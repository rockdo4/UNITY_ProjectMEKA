using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetPortrait : MonoBehaviour
{
    public Image characterImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterLevel;
    public Image characterClass;
    public Image characterProperty;
    public Image[] characterSynchro;

    private Character character;
    private CharacterTable characterTable;

    public void SetCharacterInfo(Character character)
    {
        this.character = character;
        if(characterTable == null)
        {
            characterTable = DataTableMgr.GetTable<CharacterTable>();
        }

        UpdateCharacter();
    }

    public void UpdateCharacter()
    {
        var characterData = characterTable.GetCharacterData(character.CharacterID);
        var stringTable = StageDataManager.Instance.stringTable;

        characterImage.sprite = Resources.Load<Sprite>(characterData.CharacterHead);
        characterName.SetText(stringTable.GetString(characterData.CharacterNameStringID));
        characterLevel.SetText(character.CharacterLevel.ToString());
        //characterClass.sprite = Resources.Load<Sprite>(characterData.ClassIconPath);
        //characterProperty.sprite = Resources.Load<Sprite>(characterData.PropertyIconPath);

        for (int i = 0; i < characterSynchro.Length; i++)
        {
            if (i + 2 == this.character.CharacterGrade)
            {
                characterSynchro[i].gameObject.SetActive(true);
            }
            else
            {
                characterSynchro[i].gameObject.SetActive(false);
            }
        }
    }

    public int GetCardID()
    {
        return character.CharacterID;
    }

    public void SetLock()
    {
        characterImage.color = Color.gray;
    }

    public void SetUnLock()
    {
        characterImage.color = Color.white;
    }
}
