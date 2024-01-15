using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 객체
[System.Serializable]
public class Item
{
	[HideInInspector]
	public int id;
	[HideInInspector]
	public int instanceID;
	public int count;

	public int ID { get { return id; } set { id = value; } }
	public int InstanceID { get { return instanceID; } set { instanceID = value; } }
	public int Count { get { return count; } set { count = value; } }

	[SerializeField]
	private string itemName; // 새로운 private 필드 추가
	public string Name 
	{ 
		get
		{
			var stringTable = DataTableMgr.GetTable<StringTable>();
			var nameID = DataTableMgr.GetTable<ItemInfoTable>().GetItemData(ID).NameStringID;
            var name = stringTable.GetString(nameID);
			return name;
		}
		set
		{
			itemName = value;
			var stringTable = DataTableMgr.GetTable<StringTable>();
			var nameID = DataTableMgr.GetTable<ItemInfoTable>().GetItemData(ID).NameStringID;
			itemName = stringTable.GetString(nameID);
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
	public string DescriptionStringID { get; set; }
    public int Type { get; set; }
	public int Rare { get; set; }
	public int Value { get; set; }
	public string ImagePath { get; set; }

	public string AcquisitionPathStringID { get; set; }
}