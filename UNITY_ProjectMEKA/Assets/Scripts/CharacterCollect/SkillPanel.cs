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
    public ItemAutoQuantitySkillCard[] requireItems;

	[Header("InfoPanel")]
	public CharacterInfoText infoPanel;

    private Character currCharacter;

    private SkillUpgradeTable skillUpgradeTable;
	private SkillTable skillTable;

	private void Awake()
	{
		applyButton.onClick.AddListener(() =>
		{
			infoPanel.SetPopUpPanel("강화 할건가요?", () => { ApplySkillUpgrade(); }, "예", "아니오");
		});
	}

	private void Start()
	{
		
	}

	public void UpdateSkillInfo()
	{
		var datas = skillTable.GetSkillDatas(currCharacter.SkillID);

		Debug.Log(datas.Length);
		Debug.Log(currCharacter.SkillLevel);

		int skillID;
		if (datas.Length == currCharacter.SkillLevel - 1)
		{
			skillID = -1;
		}
		else
		{
			skillID = datas[currCharacter.SkillLevel - 1].SkillID;
		}

		//skillIconImage.sprite = 
		skillNameText.SetText(currCharacter.SkillID.ToString());
		skillDescriptionText.SetText(
			$"{currCharacter.Name}의 스킬 레벨: {currCharacter.SkillLevel}\n" +
			$"스킬 ID: {skillID}");


		for (int i = 0; i < datas.Length; i++)
		{
			//var temp = Instantiate(대충 레벨 별 스킬 설명, skillLevelInfoScroll);
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
			Debug.Log("캐릭터가 없습니다");
			return;
		}

		currCharacter = character;

		UpdateSkillInfo();

		var skillLevel = currCharacter.SkillLevel;
		var info = skillUpgradeTable.GetUpgradeData(skillLevel);

		if(info == null)
		{
			Debug.Log("스킬 업그레이드 정보가 없습니다");

			foreach (var card in requireItems)
			{
				card.SetMaxLevel();
				applyButton.interactable = false;
			}
			return;
		}
		else
		{
			applyButton.interactable = true;
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
			return;
		}

		currCharacter.SkillLevel++;

		CheckMaxLevel();

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

	public void CheckMaxLevel()
	{
		var datas = skillTable.GetSkillDatas(currCharacter.SkillID);

		if (currCharacter.SkillLevel == datas.Length)
		{
			Debug.Log("스킬이 최대레벨 입니다");

			foreach (var card in requireItems)
			{
				card.SetMaxLevel();
			}
		}
	}

	public bool CheckUpgrade()
	{
		bool check = true;

		var skillLevel = currCharacter.SkillLevel;
		var info = skillUpgradeTable.GetUpgradeData(skillLevel);

		if (info == null)
		{
			Debug.Log("스킬 업그레이드 정보가 없습니다");
			return false;
		}

		if (info.RequireTier1 > requireItems[0].selectedQuantity)
		{

			Debug.Log(("티어1 아이템이 부족합니다", info.RequireTier1, requireItems[0].selectedQuantity));
			check = false;
		}

		if (info.RequireTier2 > requireItems[1].selectedQuantity)
		{
			Debug.Log("티어2 아이템이 부족합니다");
			check = false;
		}

		if (info.RequireTier3 > requireItems[2].selectedQuantity)
		{
			Debug.Log("티어3 아이템이 부족합니다");
			check = false;
		}

		return check;
	}
}
