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
	[Header("아이템 카드 프리팹")]
    public GameObject itemCardPrefab;

    [Header("아이템 정렬 선택")]
    public Dropdown dropdown;

    [Header("아이템 검색창")]
    public TMP_InputField searchInputField;

	[Header("검색 리셋 버튼")]
	public Button resetButton;

	[Header("아이템 정보창")]
	public TextMeshProUGUI itemInfoText;
	public TextMeshProUGUI itemGetInfoText;
	public Image itemImage;
	public TextMeshProUGUI itemNameText;
	
	private RectTransform itemCardScrollView;

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

		//resetButton.onClick.AddListener(() =>
		//{
		//	dropdown.value = 0;
		//	searchInputField.text = "";

		//	UpdateItemCard();
		//});
	}

	private void Start()
	{
		stringTable = DataTableMgr.GetTable<StringTable>();
		itemInfoTable = DataTableMgr.GetTable<ItemInfoTable>();

		ObjectPoolManager.instance.AddObjectToPool("ItemCard", itemCardPrefab, 30);
		UpdateItemCard();
		dropdown.value = 0;
	}

	//카드 리스트 업데이트
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
			var image = itemCard.GetComponent<Image>();
			var quantity = itemCard.GetComponentInChildren<TextMeshProUGUI>();
			var str = item.Name;

			var info = itemInfoTable.GetItemData(item.ID);

			image.sprite = Resources.Load<Sprite>(info.ImagePath);
			quantity.SetText($"x{item.Count}");

			itemCard.GetComponent<Button>().onClick.AddListener(() => 
			{
				itemInfoText.SetText($"{stringTable.GetString(info.DescriptionStringID)}");
				itemNameText.SetText($"{stringTable.GetString(info.NameStringID)}");
				itemGetInfoText.SetText($"{stringTable.GetString(info.NameStringID)}");
				itemImage.sprite = Resources.Load<Sprite>(info.ImagePath);

			});
			itemCard.name = item.ToString();
			itemCard.transform.SetParent(itemCardScrollView, false);
			itemCard.gameObject.name = count++.ToString();
			itemCard.transform.SetAsLastSibling();
		}
	}

	//카드 정렬
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

	//카드 검색
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
