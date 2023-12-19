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
		int[] arr = { 5110001, 5120001, 5130001, 5610001, 5620001, 5630001, 5710001, 5720001, 5730001, 5810001, 5810002 , 5810003 };

		var table = DataTableMgr.GetTable<ItemInfoTable>();

		for(int i=0; i<arr.Length; i++)
		{
			var item = new Item();
			var info = table.GetItemData(arr[i]);

			item.ID = info.ID;
			item.InstanceID = info.ID;
			item.Count = 1;

			ItemInventoryManager.Instance.AddItemByInstance(item);
		}
		itemCardManager.UpdateItemCard();

		//¼¼ÀÌºê
		GameManager.Instance.SaveExecution();
	}

	public void OnClickUpdateItemCard()
	{
		itemCardManager.UpdateItemCard();
	}
}
