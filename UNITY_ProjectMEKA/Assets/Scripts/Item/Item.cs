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
			return DataTableMgr.GetTable<ItemTable>().GetItemData(ID).Name;
		}
	}
}

//아이템 정보
public class ItemInfo
{
	public int ID { get; set; }
	public string Name { get; set; }
}

//public class ExchangeItem : Item
//{
	
//}

//public class GrowthItem : Item
//{

//}

//public class MissionItem : Item
//{

//}

//public class RapportItem : Item
//{

//}

//public class CollectibleItem : Item
//{

//}