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

    public CharacterPanelManager characterManager;

    [Header("testPicker")]
    private GachaSystem<int> testPicker;
	[Header("characterTable")]
	private CharacterTable characterTable;

    private void Awake()
    {
        testPicker = new GachaSystem<int>();
		characterTable = DataTableMgr.GetTable<CharacterTable>();
	}

    private void Start()
    {
        var items = characterTable.GetOriginalTable();

        foreach(var item in items)
        {
            Debug.Log((item.Key, item.Value.ArrangementCost));
            testPicker.Add(item.Key, item.Value.ArrangementCost);
        }
    }

    public void Gacha1()
    {
        ClearPanel();

		var itemID = testPicker.GetItem();
        CharacterManager.Instance.m_CharacterStorage[itemID].IsUnlock = true;

        var item = characterTable.GetCharacterData(itemID);
        characterManager.PickUpCharacter(itemID);

        var itemImage = ObjectPoolManager.instance.GetGo("GachaCard");
		itemImage.transform.SetParent(resultPanel.transform, false);
        itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.CharacterName);

        CharacterManager.Instance.UpdatePlayData();
    }

    public void Gacha10()
    {
        ClearPanel();

		var itemIDs = testPicker.GetItem(10);

        foreach (var itemID in itemIDs)
        {
			var item = characterTable.GetCharacterData(itemID);
			CharacterManager.Instance.m_CharacterStorage[itemID].IsUnlock = true;

			characterManager.PickUpCharacter(itemID);

			var itemImage = ObjectPoolManager.instance.GetGo("GachaCard");
			itemImage.transform.SetParent(resultPanel.transform, false);
            itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.CharacterName);
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
