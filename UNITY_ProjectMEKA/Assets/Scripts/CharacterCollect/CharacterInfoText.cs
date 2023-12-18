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
			$"ĳ���� ���̵� : {info.CharacterID}\n" +
			$"ĳ���� �̸� : {info.CharacterName}\n" +
			$"ĳ���� �Ӽ� : {(Defines.Property)info.CharacterProperty}\n" +
			$"ĳ���� ���� : {(Defines.Occupation)info.CharacterOccupation}\n" +
			$"ĳ���� ���� : {character.CharacterLevel}\n" +
			$"ĳ���� ��� : {character.CharacterGrade}\n" +
			$"ĳ���� �ر� : {character.IsUnlock}");
	}

	public void SetText(Character data)
	{
		if (textInfo == null)
			return;

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
}
