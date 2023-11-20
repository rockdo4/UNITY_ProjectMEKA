using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDict : MonoBehaviour
{
	public Dictionary<GachaTestCharacter, bool> charDict;

	private void Awake()
	{
		charDict = new Dictionary<GachaTestCharacter, bool>();
	}

	private void Start()
	{
		for (int i = 0; i < 20; i++)
		{
			var item = new GachaTestCharacter(i.ToString(), i * 10);
			charDict.Add(item, false);
		}
	}
}
