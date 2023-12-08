using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableTest : MonoBehaviour
{
	public ItemCardManager itemCardManager;

	private void Start()
	{
	}

	public void OnClickAddItem()
	{
		var range = DataTableMgr.GetTable<ItemInfoTable>().Count;

		var item = new Item();

		item.ID = Random.Range(1, range);
		item.InstanceID = item.ID;

		ItemInventoryManager.Instance.AddItemByInstance(item);
		itemCardManager.UpdateItemCard();

		Debug.Log(item.Name + "추가");

		//세이브
		GameManager.Instance.SaveExecution();
	}

	public void OnClickUpdateItemCard()
	{
		itemCardManager.UpdateItemCard();
	}
}
