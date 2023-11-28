using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory
{
	public static ItemInventory instance;
	private ItemInventory() 
	{ 
		m_ItemStorage = new List<Item>();
	}
	public static ItemInventory Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new ItemInventory();
			}
			return instance;
		}
	}

	public List<Item> m_ItemStorage;
	public void AddItemByInstance(Item item, int count = 1)
	{
		if(item == null)
		{
			throw new System.Exception("Item is null");
		}

		item.Count += count;
		m_ItemStorage.Add(item);
	}

	public void AddItemByID(int ID, int count = 1)
	{
		//데이터 테이블에서 ID 찾아서 받아와서 추가
		//m_ItemStorage.Add(item);
	}

	public void RemoveItemByInstance(Item item, int count = 1)
	{
		if(item == null)
		{
			throw new System.Exception("Item is null");
		}

		if(item.Count >= count)
			item.Count -= count;

		if (item.Count <= 0)
		{
			m_ItemStorage.Remove(item);
		}
	}

	public void RemoveItemByID(int ID, int count = 1)
	{
		//데이터 테이블에서 ID 찾아서 받아와서 삭제
		//m_ItemStorage.Remove(item);
	}

	public int CheckItem(Item item)
	{
		var it = m_ItemStorage.Find(x => x.ID == item.ID);

		if (it != null)
		{
			return it.Count;
		}
		return 0;
	}

	public List<Item> GetItemList()
	{
		return new List<Item>(m_ItemStorage);
	}
}
