using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDict
{
	public Dictionary<int, TestCharacter> charDict;

	public CharacterDict(TestCharacterTable dict)
	{
		charDict = new Dictionary<int, TestCharacter>();

		var items = dict.GetOriginalTable();

		foreach(var item in items)
		{
			charDict.Add(item.Key, item.Value);
		}
	}
}
