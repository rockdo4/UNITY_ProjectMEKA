using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuantityCard : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemCount;
    public TextMeshProUGUI selectItemCount;
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
        if (item == null)
            return;

        if (selectedQuantity + 1 > item.Count)
            return;

        selectedQuantity++;

        if(panel.CheckFull())
        {
            selectedQuantity--;
        }

		SetText();
	}

    public void SetText()
    {
        if(item != null)
        {
            itemCount.SetText($"{selectedQuantity}");
            selectItemCount.SetText($"{item.Count}");
		}
        else
        {
            Debug.Log("아이템이 없습니다.");
            itemCount.SetText($"{selectedQuantity}");
            selectItemCount.SetText($"{0}");
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
