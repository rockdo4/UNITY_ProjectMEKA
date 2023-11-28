using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public int ID { get; set; }
	public string Name { get; set; }
	public int Count { get; set; }
	public string Description { get; set; }
	public string ImagePath { get; set; }
	public int Rare { get; set; }
}

public class ExchangeItem : Item
{
	
}

public class GrowthItem : Item
{

}

public class MissionItem : Item
{

}

public class RapportItem : Item
{

}

public class CollectibleItem : Item
{

}