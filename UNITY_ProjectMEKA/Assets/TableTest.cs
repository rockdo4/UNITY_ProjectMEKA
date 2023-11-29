using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableTest : MonoBehaviour
{
	public ItemCardManager itemCardManager;

	private void Awake()
	{
		Item[] items = new Item[10];

		for (int i = 0; i < 10; i++)
		{
			items[i] = new Item();
			items[i].ID = i + 1;
			items[i].InstanceID = i;
			items[i].Count = 1;
		}

		//인벤토리에 템 넣음
		for (int i = 0; i < 10; i++)
		{
			ItemInventory.Instance.AddItemByInstance(items[i]);
		}
	}
	public void OnClickAddItem()
	{
		var range = DataTableMgr.GetTable<ItemInfoTable>().Count;

		var item = new Item();

		item.ID = Random.Range(1, range);
		item.InstanceID = ItemInventory.Instance.Count;

		ItemInventory.Instance.AddItemByInstance(item);
		itemCardManager.SortCard();
	}
}
