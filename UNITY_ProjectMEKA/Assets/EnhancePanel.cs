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
		//�ӽ÷� ���� ���� �ٲ����
		characterID = 13401;

		int level = currCharacter.CharacterLevel;

		int result = CombineID(characterID, level);

		Debug.Log(("ID",result));
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		currInfo[0].SetText("���� ���� : " + levelData.CharacterLevel.ToString());
		currInfo[1].SetText("���� ���ݷ� : " + levelData.CharacterDamage.ToString());
		currInfo[2].SetText("���� ���� : " + levelData.CharacterArmor.ToString());
		currInfo[3].SetText("���� ü�� : " + levelData.CharacterHP.ToString());
		currInfo[4].SetText("���� ����ġ : " + currCharacter.CurrentExp.ToString());
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
		//�ӽ÷� ���� ���� �ٲ����
		characterID = 13401;

		int result = CombineID(characterID, targetLevel);
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		targetInfo[0].SetText("���� ���� : " + targetLevel);
		targetInfo[1].SetText("���� ���ݷ� : " + levelData.CharacterDamage.ToString());
		targetInfo[2].SetText("���� ���� : " + levelData.CharacterArmor.ToString());
		targetInfo[3].SetText("���� ü�� : " + levelData.CharacterHP.ToString());
		targetInfo[4].SetText("���� ����ġ : " + exp.ToString()); ;
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
		//�ӽ÷� ���� ���� �ٲ����
		characterID = 13401;

		int result = CombineID(characterID, targetLevel);
		var levelData = DataTableMgr.GetTable<LevelTable>().GetLevelData(result);

		currCharacter.CharacterLevel = levelData.CharacterLevel;
		currCharacter.CurrentExp = exp;

		currInfo[0].SetText("���� ���� : " + targetLevel);
		currInfo[1].SetText("���� ���ݷ� : " + levelData.CharacterDamage.ToString());
		currInfo[2].SetText("���� ���� : " + levelData.CharacterArmor.ToString());
		currInfo[3].SetText("���� ü�� : " + levelData.CharacterHP.ToString());
		currInfo[4].SetText("���� ����ġ : " + exp.ToString()); ;
	}

	public int CombineID(int characterID, int level)
	{
		StringBuilder stringBuilder = new StringBuilder();
		//stringBuilder.Append(character.CharacterID);
		//�ӽ÷� ���� ���� ���� ���ľ� ��
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
