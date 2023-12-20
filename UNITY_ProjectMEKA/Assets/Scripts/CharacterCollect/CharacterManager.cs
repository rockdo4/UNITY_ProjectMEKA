using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
	private static CharacterManager instance;
	public static CharacterManager Instance 
	{
		get
		{
			if(instance == null)
			{
				instance = new CharacterManager();
            }
			else
			{

			}
			return instance;
		} 
	}
	public Dictionary<int, Character> m_CharacterStorage = new Dictionary<int, Character>();
	public void InitCharacterStorage(CharacterTable charTable, CharacterLevelTable levelTable)
	{
        var table = charTable.GetOriginalTable();

		foreach(var character in table)
		{
			var chara = new Character();
			chara.CharacterID = character.Value.CharacterID;
			chara.CharacterLevel = 1;
			chara.CurrentExp = 0;
			chara.CharacterGrade = character.Value.InitialGrade;
			chara.SkillLevel = 0;
			chara.IsUnlock = false;

			if(!m_CharacterStorage.ContainsKey(chara.CharacterID))
			{
				m_CharacterStorage.Add(chara.CharacterID, chara);
			}
		}

		CheckPlayData();
    }

	public void UpdatePlayData()
	{
		PlayDataManager.data.characterStorage = m_CharacterStorage;
	}
	public void CheckPlayData()
	{
        if (PlayDataManager.data == null)
		{
			Debug.Log("데이터 없음");
			return;
		}

		if(PlayDataManager.data.characterStorage != null)
		{
			m_CharacterStorage = PlayDataManager.data.characterStorage;
		}
	}
}
