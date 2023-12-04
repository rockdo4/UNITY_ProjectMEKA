using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 캐릭터 관리하는 싱글턴 클래스
//Singleton class that manages all characters

public class CharacterManager : MonoBehaviour
{
	public static CharacterManager Instance { get; private set; }
	public Dictionary<int, Character> m_CharacterStorage;
	private CharacterManager()
	{
		m_CharacterStorage = new Dictionary<int, Character>();
	}
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
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
		UpdatePlayData();
	}

	public void UpdatePlayData()
	{
		PlayDataManager.data.characterStorage = m_CharacterStorage;
		foreach(var character in m_CharacterStorage)
		{
			//Debug.Log((character.Key, character.Value.IsUnlock));
		}

	}
	public void CheckPlayData()
	{
		if (PlayDataManager.data != null)
		{
			m_CharacterStorage = PlayDataManager.data.characterStorage;
		}
	}
}
