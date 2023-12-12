using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//모든 캐릭터 관리하는 싱글턴 클래스
//Singleton class that manages all characters

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
			chara.CharacterGrade = 3;
			chara.IsUnlock = false;

			if(!m_CharacterStorage.ContainsKey(chara.CharacterID))
			{
				m_CharacterStorage.Add(chara.CharacterID, chara);
			}
		}

		//세이브파일 있는지 확인
		CheckPlayData();
	}

	public void UpdatePlayData()
	{
		PlayDataManager.data.characterStorage = m_CharacterStorage;
	}
	public void CheckPlayData()
	{
		m_CharacterStorage = PlayDataManager.data.characterStorage;
	}
}
