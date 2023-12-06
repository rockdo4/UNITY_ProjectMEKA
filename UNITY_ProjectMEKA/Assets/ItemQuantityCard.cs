using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuantityCard : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI quantityText;
    public EnhancePanel panel;
    private Button button;

    [HideInInspector]
    public Item item;
    public int selectedQuantity { get; private set; } = 0;

	private void Awake()
	{
        panel = GetComponentInParent<EnhancePanel>();
		button = GetComponent<Button>();
	}

	private void OnEnable()
	{
        button.onClick.AddListener(OnClickAddItemButton);
        button.onClick.AddListener(() => panel.UpdateTargetLevel());

        SetText();
	}

	private void OnDisable()
	{
		button.onClick.RemoveAllListeners();
        selectedQuantity = 0;
        SetText();
	}

	public void SetItem(int id)
    {
        var item = ItemInventoryManager.Instance.GetItemByID(id);
        SetItem(item);
    }

    public void SetItem(Item item)
    {
		this.item = item;
	}

    public void OnClickAddItemButton()
    {
        if (selectedQuantity + 1 > item.Count)
            return;

        selectedQuantity++;
		SetText();
	}

    public void SetText()
    {
        if(item != null)
        {
			quantityText.SetText($"{selectedQuantity} / {item.Count}");
		}
        else
        {
			
		}
    }

    public void ConsumeItem( )
    {
        if(item != null)
        {
            item.Count -= selectedQuantity;
            selectedQuantity = 0;
        }
    }
}
