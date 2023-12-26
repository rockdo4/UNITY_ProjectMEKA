using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 객체
public class Item
{
	public int ID { get; set; }
	public int InstanceID { get; set; }
	public int Count { get; set; }
	public string Name 
	{ 
		get
		{
			return DataTableMgr.GetTable<ItemInfoTable>().GetItemData(ID).Name;
		}
	}

	public int Value
	{
		get
		{
			return DataTableMgr.GetTable<ItemInfoTable>().GetItemData(ID).Value;
		}
	}
}

//아이템 정보
public class ItemInfo
{
	public int ID { get; set; }
	public string NameStringID { get; set; }
	public int Type { get; set; }
	public int Rare { get; set; }
	public int Value { get; set; }
	public string ImagePath { get; set; }
}