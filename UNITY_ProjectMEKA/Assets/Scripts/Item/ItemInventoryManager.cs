using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryManager
{
	private static ItemInventoryManager instance;
	public List<Item> m_ItemStorage;
	private ItemInventoryManager() 
	{
		m_ItemStorage = new List<Item>();
	}
	public static ItemInventoryManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new ItemInventoryManager();
			}
			return instance;
		}
	}
	public int Count
	{ 
		get
		{
			return m_ItemStorage.Count;
		}
	}

	public void AddItemByInstance(Item item, int count = 1)
	{
		if(item == null)
		{
			throw new System.Exception("Item is null");
		}

		var exist = m_ItemStorage.Find(x => x.InstanceID == item.InstanceID);
		
		if(exist != null)
		{
			exist.Count += count;
			return;
		}

		item.Count = count;
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

		var exist = m_ItemStorage.Find(x => x.InstanceID == item.InstanceID);
		if(exist != null)
		{
			exist.Count -= count;
			if(exist.Count <= 0)
			{
				m_ItemStorage.Remove(exist);
			}
		}
		else
		{
			throw new System.Exception("Item is not exist");
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
