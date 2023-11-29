using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ��ü
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
}

//������ ����
public class ItemInfo
{
	public int ID { get; set; }
	public string Name { get; set; }
}