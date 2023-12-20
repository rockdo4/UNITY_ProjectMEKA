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
			$"(��ų����ư)");
	}

	public void SetLevelInfo()
	{
		var levelText = enhanceButton.GetComponentInChildren<TextMeshProUGUI>();

		levelText.SetText(
			$"LV: {character.CharacterLevel}\n" +
			$"(��������ư)");
	}

	public void SetSynchroInfo()
	{
		var synchroText = synchroButton.GetComponentInChildren<TextMeshProUGUI>();

		synchroText.SetText(
			$"������: {character.CharacterGrade}\n" +
			$"�̸�: {character.Name}\n" +
			$"(��ũ�ι�ư)");
	}

	public void SetClass_Range_Keyword()
	{
		var classText = classButton.GetComponentInChildren<TextMeshProUGUI>();
		var classInfo = characterTable.GetCharacterData(character.CharacterID).CharacterOccupation;

		classText.SetText(
			$"Ŭ����: {(Defines.Occupation)classInfo}\n" +
			$"(���ݹ���, Ű����?)");
	}

	public void SetCompnayInfo()
	{
		var companyText = companyButton.GetComponentInChildren<TextMeshProUGUI>();
		var companyInfo = characterTable.GetCharacterData(character.CharacterID).CharacterProperty;

		companyText.SetText(
			$"ȸ��: {(Defines.Property)companyInfo}\n" +
			$"(���⿡ ��ũ)");
	}

	public void SetDeviceInfo()
	{
		var deviceText = deviceButton.GetComponentInChildren<TextMeshProUGUI>();

		deviceText.SetText(
			$"�ھ�: {character.DeviceCoreID}\n" +
			$"����: {character.DeviceEngineID}");
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
