using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoText : MonoBehaviour
{
	public Image characterImage;
	public Button enhanceButton;
	public Button synchroButton;
	public Button deviceButton;
	public Button skillButton;
	public EnhancePanel enhancePanel;
	public SynchroPanel synchroPanel;
	public DevicePanel devicePanel;

	public TextMeshProUGUI textInfo;

	[HideInInspector]
	public Character character;

	private void SetListener()
	{
		enhanceButton.onClick.RemoveAllListeners();
		enhanceButton.onClick.AddListener(() =>
		{
			enhancePanel.gameObject.SetActive(true);
			enhancePanel.SetCharacter(character);
		});

		synchroButton.onClick.RemoveAllListeners();
		synchroButton.onClick.AddListener(() =>
		{
			synchroPanel.gameObject.SetActive(true);
			synchroPanel.SetCharacter(character);
			//synchroPanel.CheckGrade();
		});

		deviceButton.onClick.RemoveAllListeners();
		deviceButton.onClick.AddListener(() =>
		{
			devicePanel.gameObject.SetActive(true);
			devicePanel.SetCharacter(character);
		});
	}

	public void SetCharacter (Character data)
	{
		character = data;
		if (data == null) return;

		SetListener();
		Debug.Log("CharacterIcon/" + character.ImagePath);
		characterImage.sprite = Resources.Load<Sprite>("CharacterIcon/" + character.ImagePath);
		characterImage.preserveAspect = true;
	}

	public void SetSkillInfo()
	{
		var info = skillButton.GetComponentInChildren<TextMeshProUGUI>();

	}

	public void UpdateText()
	{
		if (textInfo == null)
			return;

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

	public void SetText(Character data)
	{
		if (textInfo == null)
			return;

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
}
