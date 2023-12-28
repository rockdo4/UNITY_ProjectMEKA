using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.ConstrainedExecution;
using UnityEditor;

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
	public Button deviceEngine;
	public Button deviceCore;

	[Header("Text")]
	public TextMeshProUGUI characterName;
	public TextMeshProUGUI Damage;
	public TextMeshProUGUI HP;
	public TextMeshProUGUI Speed;
	public TextMeshProUGUI Armor;
	public TextMeshProUGUI CriticalHit;
	public TextMeshProUGUI CriticalDamage;
	public TextMeshProUGUI level;

	[Header("Etc")]
	public Sprite defaultSprite;

	[HideInInspector]
	public Character character;

	private CharacterTable characterTable;
	private StringTable stringTable;
	private ItemInfoTable itemInfoTable;

	private void Start()
	{
        if (characterTable == null)
            characterTable = DataTableMgr.GetTable<CharacterTable>();
        if (stringTable == null)
            stringTable = StageDataManager.Instance.stringTable;
		if(itemInfoTable == null)
			itemInfoTable = DataTableMgr.GetTable<ItemInfoTable>();
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

		UpdateCharacter();
	}

	public void UpdateCharacter()
	{
		UpdateStatus();
		UpdateDevice();
    }

	public void UpdateDevice()
	{
        if (itemInfoTable == null)
            itemInfoTable = DataTableMgr.GetTable<ItemInfoTable>();

        if (character.DeviceEngineID != 0)
		{
            deviceEngine.GetComponent<Image>().sprite = Resources.Load<Sprite>(itemInfoTable.GetItemData(88).ImagePath);
        }
		else
		{
            deviceEngine.GetComponent<Image>().sprite = defaultSprite;
        }

		if(character.DeviceCoreID != 0)
		{
			deviceCore.GetComponent<Image>().sprite = Resources.Load<Sprite>(itemInfoTable.GetItemData(99).ImagePath);
        }
		else
		{
            deviceCore.GetComponent<Image>().sprite = defaultSprite;
        }
	}

	public void UpdateStatus()
	{
		if(characterTable == null)
			characterTable = DataTableMgr.GetTable<CharacterTable>();
		if(stringTable == null)
			stringTable = StageDataManager.Instance.stringTable;

        var info = CharacterManager.Instance.m_CharacterStorage[character.CharacterID];
		var data = characterTable.GetCharacterData(character.CharacterID);
		if(data == null) return;
		var nameString = data.CharacterNameStringID;

        characterName.SetText(stringTable.GetString(nameString));
		Damage.SetText(info.Damage.ToString());
		HP.SetText(info.HP.ToString());
		//Speed.SetText(info.speed.ToString());
		Armor.SetText(info.Armor.ToString());
		CriticalHit.SetText("--");
		CriticalDamage.SetText("--");
		level.SetText($"{info.CharacterLevel}");
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
