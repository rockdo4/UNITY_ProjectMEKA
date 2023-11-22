using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter
{
	public int ID { get; set; }
	public int Level { get; set; }
	public int Rare { get; set; }
	public string Name { get; set; }
	public int Weight { get; set; }
}

public class TestCharacterInfo : TestCharacter
{
	public int count = 0;
}