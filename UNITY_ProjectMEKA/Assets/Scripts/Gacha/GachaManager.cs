using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
	public

    - Gacha1() : testPicker���� ������ 1�� �޾Ƽ� �гο� �߰�
    - Gacha10() : testPicker���� ������ 10�� �޾Ƽ� �гο� �߰�
*/

public class GachaManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Image imagePrefab;

    public CharacterManager characterManager;

    [Header("testPicker")]
    private GachaSystem<int> testPicker;
	[Header("characterTable")]
	private TestCharacterTable characterTable;

    private void Awake()
    {
        testPicker = new GachaSystem<int>();
		characterTable = DataTableMgr.GetTable<TestCharacterTable>();
	}

    private void Start()
    {
        var items = characterTable.GetOriginalTable();
        
        foreach(var item in items)
        {
            testPicker.Add(item.Key, item.Value.Weight);
        }
    }

    public void Gacha1()
    {
        ClearPanel();

		var itemID = testPicker.GetItem();

        var item = characterTable.GetCharacterData(itemID);
        characterManager.PickUpCharacter(itemID);

        var itemImage = ObjectPoolManager.instance.GetGo("GachaCard");
		itemImage.transform.SetParent(resultPanel.transform);
        itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.Name);
    }

    public void Gacha10()
    {
        ClearPanel();

		var itemIDs = testPicker.GetItem(10);

        foreach (var itemID in itemIDs)
        {
			var item = characterTable.GetCharacterData(itemID);
			characterManager.PickUpCharacter(itemID);

			var itemImage = ObjectPoolManager.instance.GetGo("GachaCard");
			itemImage.transform.SetParent(resultPanel.transform);
            itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.Name);
        }
    }

    public void ClearPanel()
    {
		var arr = resultPanel.GetComponentsInChildren<Image>();
		foreach (var it in arr)
		{
			if (it.gameObject == resultPanel) continue;
			it.gameObject.GetComponent<PoolAble>().ReleaseObject();
		}
	}
}
