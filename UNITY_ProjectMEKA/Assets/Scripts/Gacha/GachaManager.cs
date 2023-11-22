using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
	public

    - Gacha1() : testPicker에서 아이템 1개 받아서 패널에 추가
    - Gacha10() : testPicker에서 아이템 10개 받아서 패널에 추가
*/

public class GachaManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Image imagePrefab;

    public CharacterManager characterManager;

    private GachaSystem<int> testPicker;
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
        var arr = resultPanel.GetComponentsInChildren<Image>();
        foreach(var it in arr)
        {
			if (it.gameObject == resultPanel) continue;
			Destroy(it.gameObject);
        }

		var itemID = testPicker.GetItem();

        var item = characterTable.GetCharacterData(itemID);
        characterManager.PickUpCharacter(itemID);

		var itemImage = Instantiate(imagePrefab);
        itemImage.transform.SetParent(resultPanel.transform);
        itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.Name);
    }

    public void Gacha10()
    {
		var arr = resultPanel.GetComponentsInChildren<Image>();
		foreach (var it in arr)
		{
            if (it.gameObject == resultPanel) continue;
			Destroy(it.gameObject);
		}

		var itemIDs = testPicker.GetItem(10);

        foreach (var itemID in itemIDs)
        {
			var item = characterTable.GetCharacterData(itemID);
			characterManager.PickUpCharacter(itemID);

			var itemImage = Instantiate(imagePrefab);
            itemImage.transform.SetParent(resultPanel.transform);
            itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.Name);
        }
    }
}
