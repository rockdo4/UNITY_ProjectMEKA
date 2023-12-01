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

		//�κ��丮�� �� ����
		for (int i = 0; i < 10; i++)
		{
			ItemInventoryManager.Instance.AddItemByInstance(items[i]);
		}
	}
	public void OnClickAddItem()
	{
		var range = DataTableMgr.GetTable<ItemInfoTable>().Count;

		var item = new Item();

		item.ID = Random.Range(1, range);
		item.InstanceID = ItemInventoryManager.Instance.Count;

		ItemInventoryManager.Instance.AddItemByInstance(item);
		itemCardManager.UpdateItemCard();
	}
}
