using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    [Header("Skill Info Setting")]
    public Image skillIconImage;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;
    public Transform skillLevelInfoScroll;
	public Button applyButton;
    public ItemAutoQuantityCard[] requireItems;

	[Header("InfoPanel")]
	public CharacterInfoText infoPanel;

    private Character currCharacter;

    private SkillUpgradeTable skillUpgradeTable;
	private SkillTable skillTable;

	private void Awake()
	{
		applyButton.onClick.AddListener(() =>
		{
			ApplySkillUpgrade();
		});
	}

	private void Start()
	{
		
	}

	public void UpdateSkillInfo()
	{
		var datas = skillTable.GetSkillDatas(currCharacter.SkillID);


		//skillIconImage.sprite = 
		skillNameText.SetText(currCharacter.SkillID.ToString());
		skillDescriptionText.SetText(
			$"{currCharacter.Name}�� ��ų ����: {currCharacter.SkillLevel}\n" +
			$"��ų ID: {datas[currCharacter.SkillLevel - 1].SkillLevelID}");


		for (int i = 0; i < datas.Length; i++)
		{
			//var temp = Instantiate(���� ���� �� ��ų ����, skillLevelInfoScroll);
		}

	}

	public void SetCharacter(Character character)
	{
		if(skillUpgradeTable == null)
			skillUpgradeTable = DataTableMgr.GetTable<SkillUpgradeTable>();
		if(skillTable == null)
			skillTable = DataTableMgr.GetTable<SkillTable>();

		if (character == null)
		{
			Debug.Log("ĳ���Ͱ� �����ϴ�");
			return;
		}

		currCharacter = character;

		UpdateSkillInfo();

		var skillLevel = currCharacter.SkillLevel;
		var info = skillUpgradeTable.GetUpgradeData(skillLevel);

		if(info == null)
		{
			Debug.Log("��ų ���׷��̵� ������ �����ϴ�");
			return;
		}

		requireItems[0].SetItem(info.Tier1ID, info.RequireTier1);
		requireItems[1].SetItem(info.Tier2ID, info.RequireTier2);
		requireItems[2].SetItem(info.Tier3ID, info.RequireTier3);


		UpdateAfterSetCharacter();
	}

	private void UpdateAfterSetCharacter()
	{
		foreach (var card in requireItems)
		{
			card.SetText();
		}
	}

	public void ApplySkillUpgrade()
	{
		if(!CheckUpgrade())
		{
			Debug.Log("��ᰡ ���ڶ��ϴ�");
			return;
		}

		currCharacter.SkillLevel++;

		foreach (var card in requireItems)
		{
			card.ConsumeItem();
			card.SetText();
		}

		UpdateRequired();
		infoPanel.UpdateCharacter();
	}

	public void UpdateRequired()
	{
		foreach (var card in requireItems)
		{
			card.SetText();
		}

		SetCharacter(currCharacter);
	}

	public bool CheckUpgrade()
	{
		bool check = true;

		var datas = skillTable.GetSkillDatas(currCharacter.SkillID);

		if (currCharacter.SkillLevel == datas.Length)
		{
			Debug.Log("��ų�� �ִ뷹�� �Դϴ�");
			return false;
		}

		var skillLevel = currCharacter.SkillLevel;
		var info = skillUpgradeTable.GetUpgradeData(skillLevel);

		if (info == null)
		{
			Debug.Log("��ų ���׷��̵� ������ �����ϴ�");
			return false;
		}

		if (info.RequireTier1 > requireItems[0].selectedQuantity)
		{

			Debug.Log(("Ƽ��1 �������� �����մϴ�", info.RequireTier1, requireItems[0].selectedQuantity));
			check = false;
		}

		if (info.RequireTier2 > requireItems[1].selectedQuantity)
		{
			Debug.Log("Ƽ��2 �������� �����մϴ�");
			check = false;
		}

		if (info.RequireTier3 > requireItems[2].selectedQuantity)
		{
			Debug.Log("Ƽ��3 �������� �����մϴ�");
			check = false;
		}

		return check;
	}
}
