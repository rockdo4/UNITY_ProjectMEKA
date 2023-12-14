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
		cardText = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void ChangeCardId(int id)
	{
		cardID = id;
		var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(id);
	}

	public int GetCardID()
	{
		return cardID;
	}
}
