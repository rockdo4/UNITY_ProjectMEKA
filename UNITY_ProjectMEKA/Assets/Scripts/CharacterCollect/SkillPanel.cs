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

	private void Awake()
	{
		applyButton.onClick.AddListener(() =>
		{
			ApplySkillUpgrade();
		});

		skillUpgradeTable = DataTableMgr.GetTable<SkillUpgradeTable>();
	}

	private void Start()
	{
		
	}

	public void SetCharacter(Character character)
	{
		if(character == null)
		{
			Debug.Log("캐릭터가 없습니다");
			return;
		}

		currCharacter = character;

		var skillLevel = currCharacter.SkillLevel;
		var info = skillUpgradeTable.GetUpgradeData(skillLevel);

		if(info == null)
		{
			Debug.Log("스킬 업그레이드 정보가 없습니다");
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
			Debug.Log("재료가 모자랍니다");
			return;
		}

		currCharacter.SkillLevel++;

		foreach (var card in requireItems)
		{
			card.ConsumeItem();
			card.SetText();
		}
	}

	public bool CheckUpgrade()
	{
		var skillLevel = currCharacter.SkillLevel;
		var info = skillUpgradeTable.GetUpgradeData(skillLevel);

		if (info == null)
		{
			Debug.Log("스킬 업그레이드 정보가 없습니다");
			return false;
		}

		if (info.RequireTier1 > requireItems[0].selectedQuantity)
		{
			Debug.Log("티어1 아이템이 부족합니다");
			return false;
		}

		if (info.RequireTier2 > requireItems[1].selectedQuantity)
		{
			Debug.Log("티어2 아이템이 부족합니다");
			return false;
		}

		if (info.RequireTier3 > requireItems[2].selectedQuantity)
		{
			Debug.Log("티어3 아이템이 부족합니다");
			return false;
		}

		return true;
	}
}
