using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Defines;

public class SelectCardInfo : MonoBehaviour
{
	public Sprite defaultSprite;
	public Image cardImage;
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI nameText;
	public Image[] starImages;
	public Image propertyImage;
	private int cardID = 0;

	public void ChangeCardId(int id)
	{
		cardID = id;
		if(id != 0)
		{
			var stringTable = StageDataManager.Instance.stringTable;
			var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(id);
			var data = CharacterManager.Instance.m_CharacterStorage[id];

			cardImage.sprite = Resources.Load<Sprite>(data.CharacterHead);
			levelText.SetText($"{data.CharacterLevel}");
			nameText.SetText(stringTable.GetString(info.CharacterNameStringID));

			propertyImage.sprite = info.CharacterProperty switch
			{
				(int)Property.Prime => Resources.Load<Sprite>("CharacterIcon/PrimeIcon"),
				(int)Property.Grieve => Resources.Load<Sprite>("CharacterIcon/GrieveIcon"),
				(int)Property.Edila => Resources.Load<Sprite>("CharacterIcon/EdilaIcon"),
				(int)Property.None => Resources.Load<Sprite>("CharacterIcon/NoneIcon"),
			};
		}
		else if(id == 0)
		{
			cardImage.sprite = default;
			levelText.SetText("");
			nameText.SetText("");
		}
	}

	public void ChangeFormationId(int id)
	{
		cardID = id;

		var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(id);
		if (info == null)
		{
			cardImage.sprite = defaultSprite;
			return;
		}

		cardImage.sprite = Resources.Load<Sprite>(info.ImagePath);
	}

	public int GetCardID()
	{
		return cardID;
	}
}
