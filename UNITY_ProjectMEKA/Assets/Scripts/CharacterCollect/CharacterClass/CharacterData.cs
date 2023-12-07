using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//세이브 파일에 저장할 클래스
//Class to save files
public class Character
{
	public int CharacterID;
	public int CharacterLevel;
	public int CurrentExp;
	public int CharacterGrade;
	public bool IsUnlock;

	public float Damage
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<LevelTable>();
			var levelData = levelTable.GetLevelData(CharacterLevel);
			return levelData.CharacterDamage;
		}
	}
	public float Armor
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<LevelTable>();
			var levelData = levelTable.GetLevelData(CharacterLevel);
			return levelData.CharacterArmor;
		}
	}
	public float HP
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<LevelTable>();
			var levelData = levelTable.GetLevelData(CharacterLevel);
			return levelData.CharacterHP;
		}
	}
}

//캐릭터 정보
//Character information
public class CharacterData
{
	public int CharacterID { get; set; }
	public string CharacterName { get; set; }	
	public int CharacterProperty { get; set; }
	public int CharacterOccupation { get; set; }
	public int ArrangementCost { get; set; }
	public int WithdrawCost { get; set; }
	public int ReArrangementCoolDown { get; set; }
	public string ImagePath { get; set; }
}

//캐릭터ID + 레벨을 ID로 사용해서 레벨에 따른 스탯 불러옴
//Use CharacterID + Level as ID to load stats according to level
public class LevelData
{
	public int CharacterLevelID { get; set; }
	public int CharacterGrade { get; set; }
	public int CharacterLevel { get; set; }
	public float CharacterDamage { get; set; }
	public float CharacterArmor { get; set; }
	public float CharacterHP { get; set; }
}

//레벨업하기 위한 경험치 정보
//Experience information for level up
public class ExpData
{
	public int CharacterLevel { get; set; }
	public int CharacterGrade { get; set; }
	public int RequireExp { get; set; }
	public int AccumulateExp { get; set; }
}