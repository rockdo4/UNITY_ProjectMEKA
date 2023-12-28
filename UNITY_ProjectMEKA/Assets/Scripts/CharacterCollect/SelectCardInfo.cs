using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardInfo : MonoBehaviour
{
	public Sprite defaultSprite;
	public Image cardImage;
	public TextMeshProUGUI levelText;
	public TextMeshProUGUI nameText;
	public Image[] starImages;
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
