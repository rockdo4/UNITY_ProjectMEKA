using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemAutoQuantityCard : MonoBehaviour
{
	public Sprite defaultSprite;
	public Image itemImage;
	public TextMeshProUGUI mainText;
	public TextMeshProUGUI haveQuantityText;
	public TextMeshProUGUI requireQuantityText;
	public SynchroPanel panel;
	private Button button;

	[HideInInspector]
	public Item item;
	public int selectedQuantity { get; private set; } = 0;
	public int requiredQuantity { get; set; } = 0;

	private void Awake()
	{
		panel = GetComponentInParent<SynchroPanel>();
		button = GetComponent<Button>();
	}

	private void OnDisable()
	{
		selectedQuantity = 0; 
	}

	public void SetItem(int id, int quantity)
	{
		var item = ItemInventoryManager.Instance.GetItemByID(id);

		if (item != null)
		{
			SetItem(item, quantity);
			var itemTable = DataTableMgr.GetTable<ItemInfoTable>();
			var data = itemTable.GetItemData(id);
			itemImage.sprite = Resources.Load<Sprite>(data.ImagePath);
		}
		else
		{
			var itemTable = DataTableMgr.GetTable<ItemInfoTable>();
			var data = itemTable.GetItemData(id);

			var emptyItem = new Item();
			emptyItem.ID = data.ID;
			emptyItem.Count = 0;
			emptyItem.InstanceID = id;

			this.item = emptyItem;
			//ItemInventoryManager.Instance.AddItemByInstance(emptyItem, emptyItem.Count);

			itemImage.sprite = Resources.Load<Sprite>(data.ImagePath);

			SetItem(emptyItem, quantity);
		}
	}

	public void SetItem(Item item, int quantity)
	{
		this.item = item;
		requiredQuantity = quantity;
		selectedQuantity = item.Count;

		if (mainText!= null)
		{
			mainText.SetText(item.Name);
		}

		if (item == null)
		{
			Debug.Log("아이템 없음");
		}
		else
		{
			//Debug.Log(item.Name);
		}
	}

	public void SetText()
	{
		if(item != null)
		{
			if (item.Count >= requiredQuantity)
			{
				haveQuantityText.SetText($"{item.Count}");
				requireQuantityText.SetText($"{requiredQuantity}");
			}
			else
			{
				haveQuantityText.SetText($"<color=red>{item.Count}</color>");
				requireQuantityText.SetText($"{requiredQuantity}");
			}
		}
		else
		{
			haveQuantityText.SetText($"<color=red>0</color>");
			requireQuantityText.SetText($"{requiredQuantity}");
		}

		UpdateLayout();
	}

	public void SetMaxLevel()
	{
		if(item != null)
		{
			haveQuantityText.SetText($"{item.Count}");
			requireQuantityText.SetText($"--");
		}
		else
		{
			haveQuantityText.SetText($"{0}");
			requireQuantityText.SetText($"--");
		}

		UpdateLayout();
	}

	public void UpdateLayout()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(haveQuantityText.rectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(requireQuantityText.rectTransform);
	}

	public bool IsEnoughRequire()
	{
		if(item != null)
		{
			if(item.Count >= requiredQuantity)
			{
				return true;
			}
		}
		return false;
	}

	public void ConsumeItem()
	{
		if(!IsEnoughRequire())
			return;

		if(item != null)
		{
			item.Count -= requiredQuantity;
			selectedQuantity = 0;
		}
	}
}
