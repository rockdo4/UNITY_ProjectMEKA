using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoText : MonoBehaviour
{
	public TextMeshProUGUI textInfo;
	public Button enhanceButton;
	public EnhancePanel panel;

	[HideInInspector]
	public Character character;

	private void SetListener()
	{
		enhanceButton.onClick.RemoveAllListeners();
		enhanceButton.onClick.AddListener(() =>
		{
			panel.gameObject.SetActive(true);
			panel.SetCharacter(character);
		});
	}

	public void SetText (Character data)
	{
		character = data;
		SetListener();

		var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(data.CharacterID);

		textInfo.SetText(
			$"캐릭터 아이디 : {info.CharacterID}\n" +
			$"캐릭터 이름 : {info.CharacterName}\n" +
			$"캐릭터 속성 : {(Defines.Property)info.CharacterProperty}\n" +
			$"캐릭터 직업 : {(Defines.Occupation)info.CharacterOccupation}\n" +
			$"캐릭터 레벨 : {data.CharacterLevel}\n" +
			$"캐릭터 등급 : {data.CharacterGrade}\n" +
			$"캐릭터 해금 : {data.IsUnlock}");
	}

	public void UpdateText()
	{
		var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(character.CharacterID);

		textInfo.SetText(
			$"캐릭터 아이디 : {info.CharacterID}\n" +
			$"캐릭터 이름 : {info.CharacterName}\n" +
			$"캐릭터 속성 : {(Defines.Property)info.CharacterProperty}\n" +
			$"캐릭터 직업 : {(Defines.Occupation)info.CharacterOccupation}\n" +
			$"캐릭터 레벨 : {character.CharacterLevel}\n" +
			$"캐릭터 등급 : {character.CharacterGrade}\n" +
			$"캐릭터 해금 : {character.IsUnlock}");
	}
}
