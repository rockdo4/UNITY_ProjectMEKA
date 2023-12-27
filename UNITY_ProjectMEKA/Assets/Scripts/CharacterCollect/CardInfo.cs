using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
	private Image cardImage;
	public TextMeshProUGUI cardText;
	private int cardID = 0;

	private void Awake()
	{
		cardImage = GetComponent<Image>();
	}

	public void ChangeCardId(int id)
	{
		cardID = id;
		var stringTable = StageDataManager.Instance.stringTable;
		var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(id);

		if (info != null)
		{

		}
		else
		{

		}
	}

	public int GetCardID()
	{
		return cardID;
	}
}
