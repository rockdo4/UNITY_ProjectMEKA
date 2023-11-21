using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter2
{
	public int id;
	public string name;
	public int weight;
	public int level;
	public int rare;
	
	public TestCharacter2(string name, int weight)
	{
		this.name = name;
		this.weight = weight;
		id = 0;
	}

	public override int GetHashCode()
	{
		return name.GetHashCode() + name.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		TestCharacter2 o = obj as TestCharacter2;
		return o != null && (o.name == this.name);
	}
}