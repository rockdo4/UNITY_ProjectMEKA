using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.ConstrainedExecution;

public class CharacterInfoText : MonoBehaviour
{
	public Image characterImage;

	[Header("Button")]
	public Button enhanceButton;
	//public Button synchroButton;
	//public Button deviceButton;
	//public Button skillButton;
	public Button classButton;
	public Button companyButton;

	[Header("Panel")]
	public EnhancePanel enhancePanel;
	//public SynchroPanel synchroPanel;
	//public DevicePanel devicePanel;
	//public SkillPanel skillPanel;
	public PanelManager panelManager;

	[Header("Text")]
	public TextMeshProUGUI characterName;
	public TextMeshProUGUI Damage;
	public TextMeshProUGUI HP;
	public TextMeshProUGUI Speed;
	public TextMeshProUGUI Armor;
	public TextMeshProUGUI CriticalHit;
	public TextMeshProUGUI CriticalDamage;


	[HideInInspector]
	public Character character;

	private CharacterTable characterTable;
	private StringTable stringTable;

	private void Start()
	{
		characterTable = DataTableMgr.GetTable<CharacterTable>();
		stringTable = StageDataManager.Instance.stringTable;
	}

	private void SetListener()
	{
		enhanceButton.onClick.RemoveAllListeners();
		enhanceButton.onClick.AddListener(() =>
		{
			enhancePanel.gameObject.SetActive(true);
			enhancePanel.SetCharacter(character);
		});

		//synchroButton.onClick.RemoveAllListeners();
		//synchroButton.onClick.AddListener(() =>
		//{
		//	synchroPanel.gameObject.SetActive(true);
		//	synchroPanel.SetCharacter(character);
		//	//synchroPanel.CheckGrade();
		//});

		//deviceButton.onClick.RemoveAllListeners();
		//deviceButton.onClick.AddListener(() =>
		//{
		//	devicePanel.gameObject.SetActive(true);
		//	devicePanel.SetCharacter(character);
		//});

		//skillButton.onClick.RemoveAllListeners();
		//skillButton.onClick.AddListener(() =>
		//{
		//	skillPanel.gameObject.SetActive(true);
		//	skillPanel.SetCharacter(character);
		//});
	}

	public void SetCharacter (Character data)
	{
		character = data;
		if (data == null) return;

		characterName.SetText(data.Name);

		SetListener();
		characterImage.sprite = Resources.Load<Sprite>(character.CharacterStanding);
		characterImage.preserveAspect = true;

		UpdateCharacter();
	}

	public void UpdateCharacter()
	{
		//SetSkillInfo();
		//SetLevelInfo();
		//SetSynchroInfo();
		//SetClass_Range_Keyword();
		//SetCompnayInfo();
		//SetDeviceInfo();
	}

	public void UpdateStatus()
	{
		var info = CharacterManager.Instance.m_CharacterStorage[character.CharacterID];

		var nameString = characterTable.GetCharacterData(character.CharacterID).CharacterNameStringID;

		characterName.SetText(stringTable.GetString(nameString));
		Damage.SetText(info.Damage.ToString());
		HP.SetText(info.HP.ToString());
		//Speed.SetText(info.speed.ToString());
		Armor.SetText(info.Armor.ToString());
		CriticalHit.SetText("--");
		CriticalDamage.SetText("--");
	}

	public void SetLevelInfo()
	{
		var levelText = enhanceButton.GetComponentInChildren<TextMeshProUGUI>();

		levelText.SetText(
			$"LV: {character.CharacterLevel}\n" +
			$"(레벨업버튼)");
	}

	//public void SetSynchroInfo()
	//{
	//	var synchroText = synchroButton.GetComponentInChildren<TextMeshProUGUI>();

	//	synchroText.SetText(
	//		$"현재등급: {character.CharacterGrade}\n" +
	//		$"이름: {character.Name}\n" +
	//		$"(싱크로버튼)");
	//}

	public void SetClass_Range_Keyword()
	{
		var classText = classButton.GetComponentInChildren<TextMeshProUGUI>();
		var classInfo = characterTable.GetCharacterData(character.CharacterID).CharacterOccupation;

		classText.SetText(
			$"클래스: {(Defines.Occupation)classInfo}\n" +
			$"(공격범위, 키워드?)");
	}

	public void SetCompnayInfo()
	{
		var companyText = companyButton.GetComponentInChildren<TextMeshProUGUI>();
		var companyInfo = characterTable.GetCharacterData(character.CharacterID).CharacterProperty;

		companyText.SetText(
			$"회사: {(Defines.Property)companyInfo}\n" +
			$"(여기에 마크)");
	}

	//public void SetDeviceInfo()
	//{
	//	var deviceText = deviceButton.GetComponentInChildren<TextMeshProUGUI>();

	//	deviceText.SetText(
	//		$"코어: {character.DeviceCoreID}\n" +
	//		$"엔진: {character.DeviceEngineID}");
	//}


	public void SetPopUpPanel(string text, Action yesAction, string yesText = null, string noText = null)
	{
		panelManager.SetPopUpPanel(text, yesAction, yesText, noText);
	}

	public void SetNoticePanel(string text, string yesText = null)
	{
		panelManager.SetNoticePanel(text, yesText);
	}
}
