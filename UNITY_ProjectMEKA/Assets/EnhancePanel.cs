using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class EnhancePanel : MonoBehaviour
{
	public TextMeshProUGUI[] currInfo;
	public TextMeshProUGUI[] targetInfo;

	public TMP_InputField inputExp;
	public CharacterInfoText UpdateInfoPanel;

	private Character currCharacter;

	public void Awake()
	{
		inputExp.onEndEdit.AddListener((x) => 
		{
			if(int.TryParse(x, out int result))
			{
				ApplyUpgradeLevel(result);
				
			}
			inputExp.text = "";
		});

		inputExp.onValueChanged.AddListener((x) =>
		{
			if (int.TryParse(x, out int result))
			{
				UpdateTargetLevel(result);
			}
		});
	}

	public void SetCharacter(Character character)
	{
		currCharacter = character;

		int characterID = currCharacter.CharacterID;
		//임시로 고정 추후 바꿔야함
		characterID = 13401;

		int level = currCharacter.CharacterLevel;

		int result = CombineID(characterID, level);

		Debug.Log(("ID",result));
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		currInfo[0].SetText("현재 레벨 : " + levelData.CharacterLevel.ToString());
		currInfo[1].SetText("현재 공격력 : " + levelData.CharacterDamage.ToString());
		currInfo[2].SetText("현재 방어력 : " + levelData.CharacterArmor.ToString());
		currInfo[3].SetText("현재 체력 : " + levelData.CharacterHP.ToString());
		currInfo[4].SetText("현재 경험치 : " + currCharacter.CurrentExp.ToString());
	}

	public void UpdateTargetLevel(int exp)
	{
		var table = DataTableMgr.GetTable<ExpTable>().GetOriginalTable();

		int currentLevel = currCharacter.CharacterLevel;
		int targetLevel = currentLevel;

		while(exp > 0)
		{
			if(exp >= table[targetLevel - 1].RequireExp)
			{
				exp -= table[targetLevel - 1].RequireExp;
				targetLevel++;
			}
			else
			{
				break;
			}

			if(targetLevel > table.Count)
			{
				targetLevel--;
				break;
			}
		}
		int characterID = currCharacter.CharacterID;
		//임시로 고정 추후 바꿔야함
		characterID = 13401;

		int result = CombineID(characterID, targetLevel);
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		targetInfo[0].SetText("예상 레벨 : " + targetLevel);
		targetInfo[1].SetText("예상 공격력 : " + levelData.CharacterDamage.ToString());
		targetInfo[2].SetText("예상 방어력 : " + levelData.CharacterArmor.ToString());
		targetInfo[3].SetText("예상 체력 : " + levelData.CharacterHP.ToString());
		targetInfo[4].SetText("예상 경험치 : " + exp.ToString()); ;
	}

	public void ApplyUpgradeLevel(int exp)
	{
		var table = DataTableMgr.GetTable<ExpTable>().GetOriginalTable();

		int currentLevel = currCharacter.CharacterLevel;
		int targetLevel = currentLevel;

		while (exp > 0)
		{
			if (exp >= table[targetLevel - 1].RequireExp)
			{
				exp -= table[targetLevel - 1].RequireExp;
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
		//임시로 고정 추후 바꿔야함
		characterID = 13401;

		int result = CombineID(characterID, targetLevel);
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		currCharacter.CharacterLevel = levelData.CharacterLevel;
		currCharacter.CurrentExp = exp;

		currInfo[0].SetText("현재 레벨 : " + targetLevel);
		currInfo[1].SetText("현재 공격력 : " + levelData.CharacterDamage.ToString());
		currInfo[2].SetText("현재 방어력 : " + levelData.CharacterArmor.ToString());
		currInfo[3].SetText("현재 체력 : " + levelData.CharacterHP.ToString());
		currInfo[4].SetText("현재 경험치 : " + exp.ToString()); ;
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
