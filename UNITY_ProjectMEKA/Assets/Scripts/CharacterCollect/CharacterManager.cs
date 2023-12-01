using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� ĳ���� �����ϴ� �̱��� Ŭ����
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

		//���̺����� �ִ��� Ȯ��
		//Check if there is a save file

		foreach(var character in table)
		{
			var chara = new Character();
			chara.CharacterID = character.Value.CharacterID;
			chara.CharacterLevel = 1;
			chara.CurrentExp = 0;
			chara.CharacterGrade = 3;
			chara.IsUnlock = false;

			//���̺�

			m_CharacterStorage.Add(chara.CharacterID, chara);
		}
	}
}
