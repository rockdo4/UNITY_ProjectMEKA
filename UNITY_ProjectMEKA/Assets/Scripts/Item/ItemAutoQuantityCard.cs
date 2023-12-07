using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemAutoQuantityCard : MonoBehaviour
{
	public Image itemImage;
	public TextMeshProUGUI quantityText;
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

	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
		//button.onClick.RemoveAllListeners();
		selectedQuantity = 0;
		SetText();
	}

	public void SetItem(int id, int quantity)
	{
		var item = ItemInventoryManager.Instance.GetItemByID(id);
		SetItem(item, quantity);
	}

	public void SetItem(Item item, int quantity)
	{
		this.item = item;
		requiredQuantity = quantity;

		Debug.Log(item.Name);
	}

	public void SetText()
	{
		if(item != null)
		{
			if(item.Count >= requiredQuantity)
			{
				quantityText.SetText($"{item.Count} / {requiredQuantity}");
			}
			else
			{
				quantityText.SetText($"<color=red>{item.Count}</color> / {requiredQuantity}");
			}
		}
		else
		{
			//
		}
	}

	public bool IsEnough()
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
		if(!IsEnough())
			return;

		if(item != null)
		{
			item.Count -= requiredQuantity;
			selectedQuantity = 0;
		}
	}
}
