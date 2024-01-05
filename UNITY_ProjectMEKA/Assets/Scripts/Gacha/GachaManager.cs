using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public CharacterPanelManager characterManager;
    public ModalWindow modalWindow;

    [Header("Gacha Button")]
    public Button gacha1Button;
    public Button gacha10Button;

    [Header("testPicker")]
    private GachaSystem<int> testPicker;
	[Header("characterTable")]
	private CharacterTable characterTable;

    private Coroutine gachaCoroutine;
    private float timer = 0.5f;
    private float time = 0f;
    private const int diamond = 5920001;

    private void Awake()
    {
        testPicker = new GachaSystem<int>();

        gacha1Button.onClick.AddListener(() => 
        {
            modalWindow.Show("���̾� 200���� ����Ͽ�\n��í�� �����ðڽ��ϱ�?", () =>
            {
                if (CheckDiamond(200))
                {
                    Gacha1();
					int index = ItemInventoryManager.Instance.m_ItemStorage.FindIndex(x => x.ID == diamond);
					ItemInventoryManager.Instance.m_ItemStorage[index].Count -= 200;
                }
                else
                {
                   // modalWindow.Notice("���̾ư� �����մϴ�.", "Ȯ��");
                }
            }, "Ȯ��", "���");
        });

        gacha10Button.onClick.AddListener(() => 
        { 
            modalWindow.Show("���̾� 2000���� ����Ͽ�\n��í�� �����ðڽ��ϱ�?", () =>
            {
                if (CheckDiamond(2000))
                {
                    Gacha10();
					int index = ItemInventoryManager.Instance.m_ItemStorage.FindIndex(x => x.ID == diamond);
					ItemInventoryManager.Instance.m_ItemStorage[index].Count -= 2000;
                }
                else
                {
                    
                }
            }, "Ȯ��", "���");
        });
	}

    private void Start()
    {
		characterTable = DataTableMgr.GetTable<CharacterTable>();
        var items = characterTable.GetOriginalTable();

        foreach(var item in items)
        {
            //Debug.Log((item.Key, item.Value.ArrangementCost));
            if(item.Value.PortraitPath == "None")
            {
                continue;
            }
            else if(item.Value.IsBasic == 1)
            {
                continue;
            }
            else
            {
				testPicker.Add(item.Key, item.Value.ArrangementCost);
			}
        }
    }

    public bool CheckDiamond(int count)
    {
        bool check = false;
        int index = ItemInventoryManager.Instance.m_ItemStorage.FindIndex(x => x.ID == diamond);

		if (ItemInventoryManager.Instance.m_ItemStorage.Find(x => x.ID == diamond) == null)
        {
            check = false;
		}
        else if(index != -1)
        {
			if(ItemInventoryManager.Instance.m_ItemStorage[index].Count >= count)
            {
				check = true;
			}
        }

        if(!check) modalWindow.Notice("���̾ư� �����մϴ�.", "Ȯ��");

        return check;
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
