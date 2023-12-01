using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 캐릭터 관리하는 싱글턴 클래스
//Singleton class that manages all characters

public class CharacterManager
{
	private static CharacterManager instance;
	public Dictionary<int, Character> m_CharacterStorage;
	private CharacterManager()
	{
		m_CharacterStorage = new Dictionary<int, Character>();
	}
	public static CharacterManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new CharacterManager();
			}
			return instance;
		}
	}

	public void InitCharacterStorage(CharacterTable charTable, LevelTable levelTable)
	{
		var table = charTable.GetOriginalTable();

		//세이브파일 있는지 확인
		//Check if there is a save file

		foreach(var character in table)
		{
			var chara = new Character();
			chara.CharacterID = character.Value.CharacterID;
			chara.CharacterLevel = 1;
			chara.CurrentExp = 0;
			chara.CharacterGrade = 3;
			chara.IsUnlock = false;

			//테이블

			m_CharacterStorage.Add(chara.CharacterID, chara);
		}
	}
}
