using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoText : MonoBehaviour
{
	public Image characterImage;

	[Header("Button")]
	public Button enhanceButton;
	public Button synchroButton;
	public Button deviceButton;
	public Button skillButton;
	public Button classButton;
	public Button companyButton;

	[Header("Panel")]
	public EnhancePanel enhancePanel;
	public SynchroPanel synchroPanel;
	public DevicePanel devicePanel;
	public SkillPanel skillPanel;

	[Header("Text")]
	public TextMeshProUGUI textInfo;

	[HideInInspector]
	public Character character;

	private CharacterTable characterTable;

	private void Start()
	{
		characterTable = DataTableMgr.GetTable<CharacterTable>();
	}

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

		skillButton.onClick.RemoveAllListeners();
		skillButton.onClick.AddListener(() =>
		{
			skillPanel.gameObject.SetActive(true);
			skillPanel.SetCharacter(character);
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

		UpdateCharacter();
	}

	public void UpdateCharacter()
	{
		SetSkillInfo();
		SetLevelInfo();
		SetSynchroInfo();
		SetClass_Range_Keyword();
		SetCompnayInfo();
		SetDeviceInfo();
	}

	public void SetSkillInfo()
	{
		//var table = DataTableMgr.GetTable<>();
		var skillText = skillButton.GetComponentInChildren<TextMeshProUGUI>();
		var skillTable = DataTableMgr.GetTable<SkillTable>();
		var datas = skillTable.GetSkillDatas(character.SkillID);

		int skillID;
		if (datas.Length == character.SkillLevel - 1)
		{
			skillID = -1;
		}
		else
		{
			skillID = datas[character.SkillLevel - 1].SkillLevelID;
		}

		skillText.SetText(
			$"Skill ID: {character.SkillID}\n" +
			$"SkillLevel ID: {skillID}\n" +
			$"SkillLevel: {character.SkillLevel}\n" +
			$"(스킬업버튼)");
	}

	public void SetLevelInfo()
	{
		var levelText = enhanceButton.GetComponentInChildren<TextMeshProUGUI>();

		levelText.SetText(
			$"LV: {character.CharacterLevel}\n" +
			$"(레벨업버튼)");
	}

	public void SetSynchroInfo()
	{
		var synchroText = synchroButton.GetComponentInChildren<TextMeshProUGUI>();

		synchroText.SetText(
			$"현재등급: {character.CharacterGrade}\n" +
			$"이름: {character.Name}\n" +
			$"(싱크로버튼)");
	}

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

	public void SetDeviceInfo()
	{
		var deviceText = deviceButton.GetComponentInChildren<TextMeshProUGUI>();

		deviceText.SetText(
			$"코어: {character.DeviceCoreID}\n" +
			$"엔진: {character.DeviceEngineID}");
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
