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
    public GameObject itemCardPrefab;
	public TMP_Dropdown dropdown;
	public TMP_InputField searchInputField;
	
	private RectTransform itemCardScrollView;

	private bool Once = true;

	private void Awake()
	{
		itemCardScrollView = GetComponent<RectTransform>();
		dropdown.onValueChanged.AddListener((x) => 
		{
			SortCard(x);
		});
		searchInputField.onEndEdit.AddListener((x) =>
		{
			SearchCard(x);
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
			itemList = ItemInventory.Instance.m_ItemStorage;
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

			text.SetText(item.Name);
			var str = item.Name;
			itemCard.GetComponent<Button>().onClick.AddListener(() => { Debug.Log(str); });
			itemCard.name = count++.ToString();
			itemCard.transform.SetParent(itemCardScrollView);
		}
	}

	//카드 정렬
	public void SortCard(int value = -1)
	{
		if (value == -1)
			value = dropdown.value;

		var itemList = ItemInventory.Instance.m_ItemStorage;

		switch (value)
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

		UpdateItemCard(itemList);
	}

	public void SearchCard(string str)
	{
		var itemList = ItemInventory.Instance.m_ItemStorage;
		itemList = itemList.Where(x => x.Name.Contains(str)).ToList();

		UpdateItemCard(itemList);
	}
}
