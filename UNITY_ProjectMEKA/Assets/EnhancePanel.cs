using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhancePanel : MonoBehaviour
{
	public int[] itemID;

	public TextMeshProUGUI levelText;
	public TextMeshProUGUI damageText;
	public TextMeshProUGUI armorText;
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI expText;

	public ItemQuantityCard[] reportItemCard;
	public Button applyButton;

	public CharacterInfoText UpdateInfoPanel;

	private Character currCharacter;

	private void Awake()
	{
		applyButton.onClick.AddListener(() => 
		{
			ApplyUpgradeLevel();

			foreach(var card in reportItemCard)
			{
				card.ConsumeItem();
			}

			UpdateTargetLevel();
		});

	}

	public void OnEnable()
	{

		for (int i = 0; i < reportItemCard.Length; i++)
		{
			reportItemCard[i].SetItem(itemID[i]);
		}

		currCharacter = UpdateInfoPanel.character;
	}

	public void SetCharacter(Character character)
	{
		currCharacter = character;

		int characterID = currCharacter.CharacterID;
		int level = currCharacter.CharacterLevel;

		int result = CombineID(characterID, level);

		Debug.Log(("ID",result));
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		UpdateTargetLevel();
	}

	public LevelData CalculateData(int totalExp, out int remain)
	{
		var table = DataTableMgr.GetTable<ExpTable>().GetOriginalTable();

		int currentLevel = currCharacter.CharacterLevel;
		int targetLevel = currentLevel;

		while (totalExp > 0)
		{
			if (totalExp >= table[targetLevel - 1].RequireExp)
			{
				totalExp -= table[targetLevel - 1].RequireExp;
				targetLevel++;
			}
			else
			{
				break;
			}

			if (targetLevel > table.Count)
			{
				targetLevel--;
				break;
			}
		}
		int characterID = currCharacter.CharacterID;
		int result = CombineID(characterID, targetLevel);

		remain = totalExp;
		return DataTableMgr.GetTable<LevelTable>().GetLevelData(result);
	}

	public void UpdateTargetLevel()
	{
		int totalExp = 0;
		int remainExp = 0;

		foreach(var card in reportItemCard)
		{
			var value = card.item.Value;
			totalExp += (card.selectedQuantity * value);
		}

		var data = CalculateData(totalExp, out remainExp);

		int result = CombineID(currCharacter.CharacterID, currCharacter.CharacterLevel);
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		levelText.SetText($"레벨 : {levelData.CharacterLevel}	>>	{data.CharacterLevel}");
		damageText.SetText($"공격력 : {levelData.CharacterDamage}	>>	{data.CharacterDamage}");
		armorText.SetText($"방어력 : {levelData.CharacterArmor}	>>	{data.CharacterArmor}");
		hpText.SetText($"체력 : {levelData.CharacterHP}	>>	{data.CharacterHP}");
		expText.SetText($"경험치 : {currCharacter.CurrentExp} >> {remainExp}");
	}

	public void ApplyUpgradeLevel()
	{
		int totalExp = 0;
		int remainExp = 0;

		foreach (var card in reportItemCard)
		{
			var value = card.item.Value;
			totalExp += (card.selectedQuantity * value);
		}

		var data = CalculateData(totalExp, out remainExp);

		currCharacter.CharacterLevel = data.CharacterLevel;
		currCharacter.CurrentExp = remainExp;

		int result = CombineID(currCharacter.CharacterID, currCharacter.CharacterLevel);
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		levelText.SetText($"레벨 : {levelData.CharacterLevel}	>>	{data.CharacterLevel}");
		damageText.SetText($"공격력 : {levelData.CharacterDamage}	>>	{data.CharacterDamage}");
		armorText.SetText($"방어력 : {levelData.CharacterArmor}	>>	{data.CharacterArmor}");
		hpText.SetText($"체력 : {levelData.CharacterHP}	>>	{data.CharacterHP}");
		expText.SetText($"경험치 : {currCharacter.CurrentExp} >> {remainExp}");
	}

	public int CombineID(int characterID, int level)
	{
		StringBuilder stringBuilder = new StringBuilder();
		//stringBuilder.Append(character.CharacterID);
		//임시로 고정 숫자 넣음 고쳐야 됨
		stringBuilder.Append(characterID);

		if (level < 10)
		{
			stringBuilder.Append(0);
			stringBuilder.Append(level);
		}
		else
			stringBuilder.Append(level);


		int.TryParse(stringBuilder.ToString(), out int result);

		return result;
	}
}
