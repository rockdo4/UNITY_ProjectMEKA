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
			$"ĳ���� ���̵� : {info.CharacterID}\n" +
			$"ĳ���� �̸� : {info.CharacterName}\n" +
			$"ĳ���� �Ӽ� : {(Defines.Property)info.CharacterProperty}\n" +
			$"ĳ���� ���� : {(Defines.Occupation)info.CharacterOccupation}\n" +
			$"ĳ���� ���� : {data.CharacterLevel}\n" +
			$"ĳ���� ��� : {data.CharacterGrade}\n" +
			$"ĳ���� �ر� : {data.IsUnlock}");
	}

	public void UpdateText()
	{
		var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(character.CharacterID);

		textInfo.SetText(
			$"ĳ���� ���̵� : {info.CharacterID}\n" +
			$"ĳ���� �̸� : {info.CharacterName}\n" +
			$"ĳ���� �Ӽ� : {(Defines.Property)info.CharacterProperty}\n" +
			$"ĳ���� ���� : {(Defines.Occupation)info.CharacterOccupation}\n" +
			$"ĳ���� ���� : {character.CharacterLevel}\n" +
			$"ĳ���� ��� : {character.CharacterGrade}\n" +
			$"ĳ���� �ر� : {character.IsUnlock}");
	}
}
