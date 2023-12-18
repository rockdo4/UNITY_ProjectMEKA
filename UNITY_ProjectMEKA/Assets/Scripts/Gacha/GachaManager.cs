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

    private Coroutine gachaCoroutine;
    private float timer = 0.5f;
    private float time = 0f;

    private void Awake()
    {
        testPicker = new GachaSystem<int>();
	}

    private void Start()
    {
		characterTable = DataTableMgr.GetTable<CharacterTable>();
        var items = characterTable.GetOriginalTable();

        foreach(var item in items)
        {
            //Debug.Log((item.Key, item.Value.ArrangementCost));
            if(item.Value.PortraitPath != "None")
            {
                testPicker.Add(item.Key, item.Value.ArrangementCost);
            }
            else
            {
                //CharacterManager.Instance.m_CharacterStorage[item.Key].IsUnlock = true;
                //Debug.Log($"{item.Value.CharacterName} 제외");
            }
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
        itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.PortraitPath);

        CharacterManager.Instance.UpdatePlayData();
        GameManager.Instance.SaveExecution();
    }

    IEnumerator WaitGacha10()
    {
		ClearPanel();

		for (int i = 0; i < 10; i++)
		{
			var itemID = testPicker.GetItem();

			CharacterManager.Instance.m_CharacterStorage[itemID].IsUnlock = true;

			var item = characterTable.GetCharacterData(itemID);
			characterManager.PickUpCharacter(itemID);

			var itemImage = ObjectPoolManager.instance.GetGo("GachaCard");
			itemImage.transform.SetParent(resultPanel.transform, false);
			itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.PortraitPath);
            itemImage.transform.SetAsLastSibling();

			CharacterManager.Instance.UpdatePlayData();

			yield return new WaitForSeconds(0.05f);
		}
		GameManager.Instance.SaveExecution();

		yield return new WaitForSeconds(0.1f);
		gachaCoroutine = null;
    }
    public void Gacha10()
    {
        if (gachaCoroutine == null)
        {
		    gachaCoroutine = StartCoroutine(WaitGacha10());
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
