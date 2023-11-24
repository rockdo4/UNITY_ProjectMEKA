using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInfoText : MonoBehaviour
{
	public TextMeshProUGUI textInfo;

	public void SetText (TestCharacterInfo info)
	{
		textInfo.SetText($"ID : {info.ID}\nName : {info.Name}\nRare : {info.Rare}" +
			$"\nLevel : {info.Level}\nWeight : {info.Weight}\nCount : {info.count}");
	}
}
