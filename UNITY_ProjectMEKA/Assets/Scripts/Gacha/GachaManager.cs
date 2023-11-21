using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Image imagePrefab;
    public TextMeshProUGUI resultText;
    public int[] count;

    private GachaSystem<int> testPicker;

    private void Awake()
    {
		testPicker = new GachaSystem<int>();
	}

    private void Start()
    {
        var items = DataTableMgr.GetTable<TestCharacterTable>();
		for (int i = 1; i <= 20; i++) 
        {
            //testPicker.Add(items[i].id, items[i].weight);
        }

        count = new int[20];
    }

    public void Gacha1()
    {
        var arr = resultPanel.GetComponentsInChildren<Image>();
        foreach(var it in arr)
        {
			if (it.gameObject == resultPanel) continue;
			Destroy(it.gameObject);
        }

		var item = testPicker.GetItem();




		//Debug.Log($"이름 : {item.name}, 가중치 : {item.weight}");

  //      var itemImage = Instantiate(imagePrefab);
  //      itemImage.transform.SetParent(resultPanel.transform);
		//itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.name);
  //      itemImage.color = Color.red * (30 - int.Parse(item.name)) / 30;
		//count[int.Parse(item.name) - 1]++;
		//SetText();
	}

    public void Gacha10()
    {
		var arr = resultPanel.GetComponentsInChildren<Image>();
		foreach (var it in arr)
		{
            if (it.gameObject == resultPanel) continue;
			Destroy(it.gameObject);
		}

		var item = testPicker.GetItem(10);

		//foreach (var it in item)
		//{
		//	Debug.Log($"이름 : {it.name}, 가중치 : {it.weight}");
		//	var itemImage = Instantiate(imagePrefab);
		//	itemImage.transform.SetParent(resultPanel.transform);
  //          itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(it.name);
		//	itemImage.color = Color.red * (30 - int.Parse(it.name)) / 30;

  //          count[int.Parse(it.name) - 1]++;
		//}

        SetText();
	}

    public void SetText()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for(int i=0; i<count.Length; i++)
        {
            stringBuilder.Append($"[{i + 1} : {count[i]}] / ");
        }

        resultText.SetText(stringBuilder.ToString());
    }
}
