using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSkillCard : MonoBehaviour
{
	public Image itemImage;
	public TextMeshProUGUI countText;
	public TextMeshProUGUI requireText;
	public SkillPanel panel;
	private Button button;

	[HideInInspector]
	public Item item;
	public int selectedQuantity { get; private set; } = 0;
	public int requiredQuantity { get; set; } = 0;

	private void Awake()
	{
		panel = GetComponentInParent<SkillPanel>();
		button = GetComponent<Button>();
	}

	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
		selectedQuantity = 0;
	}

	public void SetItem(int id, int quantity)
	{
		var item = ItemInventoryManager.Instance.GetItemByID(id);

        var itemTable = DataTableMgr.GetTable<ItemInfoTable>();
        var data = itemTable.GetItemData(id);
        itemImage.sprite = Resources.Load<Sprite>(data.ImagePath);

		if (item != null)
		{
			SetItem(item, quantity);
		}
		else
		{
			var emptyItem = new Item();
			emptyItem.ID = data.ID;
			emptyItem.Count = 0;
			emptyItem.InstanceID = id;

			this.item = emptyItem;
			//ItemInventoryManager.Instance.AddItemByInstance(emptyItem, emptyItem.Count);


			SetItem(emptyItem, quantity);
		}
	}

	public void SetItem(Item item, int quantity)
	{
		this.item = item;
		requiredQuantity = quantity;
		selectedQuantity = item.Count;

		if (item == null)
		{
			Debug.Log("아이템 없음");
		}
		else
		{
			Debug.Log(item.Name);
		}
	}

	public void SetText()
	{
		if(item != null)
		{
			if(item.Count >= requiredQuantity)
			{
				countText.SetText($"{item.Count}");
				requireText.SetText($"{requiredQuantity}");
			}
			else
			{
				countText.SetText($"<color=red>{item.Count}</color>");
				requireText.SetText($"{requiredQuantity}");
			}
		}
		else
		{
			countText.SetText($"<color=red>{0}</color>");
			requireText.SetText($"{requiredQuantity}");
		}
	}

	public void SetMaxLevel()
	{
		if (item != null)
		{
			countText.SetText($"{item.Count}");
			requireText.SetText($"--");
		}
		else
		{
			countText.SetText($"{0}");
			requireText.SetText($"--");
		}
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
