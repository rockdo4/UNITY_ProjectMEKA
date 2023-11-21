using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDict<T>
{
	public Dictionary<int, T> charDict;

	public CharacterDict(int[] idArr, T[] arr)
	{
		charDict = new Dictionary<int, T>();

		for (int i = 0; i < arr.Length; i++)
		{
			charDict.Add(idArr[i], arr[i]);
		}
	}
}
