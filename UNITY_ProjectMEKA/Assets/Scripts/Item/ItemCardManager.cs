using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SortType
{
	None = 0,
	Ascending,
	Descending,
}
 
public class ItemCardManager : MonoBehaviour
{
	[Header("������ ī�� ������")]
    public GameObject itemCardPrefab;

    [Header("������ ���� ����")]
    public TMP_Dropdown dropdown;

    [Header("������ �˻�â")]
    public TMP_InputField searchInputField;

	[Header("�˻� ���� ��ư")]
	public Button resetButton;
	
	private RectTransform itemCardScrollView;

	private bool Once = true;

	private StringTable stringTable;
	private ItemInfoTable itemInfoTable;

	private void Awake()
	{
		itemCardScrollView = GetComponent<RectTransform>();
		dropdown.onValueChanged.AddListener((x) => 
		{
			var list = SortCard(x);
			if(searchInputField.text != "")
				list = SearchCard(searchInputField.text, list);

            UpdateItemCard(list);
        });

		searchInputField.onEndEdit.AddListener((x) =>
		{
			var list = SearchCard(x);
			if(dropdown.value != 0)
				list = SortCard(dropdown.value, list);

            UpdateItemCard(list);
        });

		resetButton.onClick.AddListener(() =>
		{
			dropdown.value = 0;
			searchInputField.text = "";

			UpdateItemCard();
		});

		stringTable = StageDataManager.Instance.stringTable;
        itemInfoTable = StageDataManager.Instance.itemInfoTable;

    }
	private void OnEnable()
	{
		if(Once)
		{
			return;
		}
		UpdateItemCard();
		dropdown.value = 0;
		Once = false;
    }

	//ī�� ����Ʈ ������Ʈ
	public void UpdateItemCard(List<Item> itemList = null)
	{
		var items = itemCardScrollView.GetComponentsInChildren<Button>();

		if(itemList == null)
		{
			itemList = ItemInventoryManager.Instance.m_ItemStorage;
		}

		foreach (var item in items)
		{
			item.GetComponent<Button>().onClick.RemoveAllListeners();
			item.GetComponent<PoolAble>().ReleaseObject();
		}

		int count = 0;
		foreach (var item in itemList)
		{
			var itemCard = ObjectPoolManager.instance.GetGo("ItemCard");
			var itemNameText = itemCard.GetComponentInChildren<TextMeshProUGUI>();
			var itemImage = itemCard.GetComponent<Image>();

			// 12.28, MJ, Apply string table, itemInfoTable
			var itemTableData = itemInfoTable.GetItemData(item.ID);
            var itemImagePath = itemTableData.ImagePath;
			var itemName = stringTable.GetString(itemTableData.NameStringID);
			var itemInfo = stringTable.GetString(itemTableData.DescriptionStringID);

			itemImage.sprite = Resources.Load<Sprite>(itemImagePath);
			itemNameText.SetText(itemName + " : " + item.Count);
			itemCard.GetComponent<Button>().onClick.AddListener(() => 
			{
				Debug.Log(("�̸�: " + itemName, "���̵�: " + item.ID, "�ν��Ͻ����̵�: " + item.InstanceID, "���: " + item.Value, "����: " +item.Count));
			});
			itemCard.name = itemName;
			itemCard.transform.SetParent(itemCardScrollView, false);
			itemCard.gameObject.name = count++.ToString();
			itemCard.transform.SetAsLastSibling();
		}
	}

	//ī�� ����
	public List<Item> SortCard(int sortValue = -1, List<Item> itemList = null)
	{
		if (sortValue == -1)
			sortValue = dropdown.value;

		if(itemList == null)
            itemList = ItemInventoryManager.Instance.m_ItemStorage;

		switch (sortValue)
		{
			case (int)SortType.None:
				break;

			case (int)SortType.Ascending:
				itemList = itemList.OrderBy(x => x.Name).ToList();
				break;

			case (int)SortType.Descending:
				itemList = itemList.OrderByDescending(x => x.Name).ToList();
				break;

			default:
				break;
		}
		return itemList;
	}

	//ī�� �˻�
	public List<Item> SearchCard(string str = "", List<Item> itemList = null)
	{
		if(itemList == null)
		{
            itemList = ItemInventoryManager.Instance.m_ItemStorage;
        }
		itemList = itemList.Where(x => x.Name.Contains(str)).ToList();
		return itemList;
	}
}
