using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    public TMP_Dropdown dropdown;

    [Header("아이템 검색창")]
    public TMP_InputField searchInputField;

	[Header("검색 리셋 버튼")]
	public Button resetButton;
	
	private RectTransform itemCardScrollView;

	private bool Once = true;

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
			var text = itemCard.GetComponentInChildren<TextMeshProUGUI>();
			var str = item.Name;

			text.SetText(item.Name);
			itemCard.GetComponent<Button>().onClick.AddListener(() => { Debug.Log(str); });
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
