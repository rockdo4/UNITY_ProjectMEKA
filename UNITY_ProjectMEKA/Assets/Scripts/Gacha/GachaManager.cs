using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct GachaTestCharacter
{
    public int id;
    public string name;
    public int weight;

    public GachaTestCharacter(string name, int weight)
    {
        this.name = name;
        this.weight = weight;
        id = 0;
    }
}

public class GachaManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Image imagePrefab;
    public TextMeshProUGUI resultText;
    public int[] count;

    private GachaSystem<GachaTestCharacter> testPicker;

    private void Awake()
    {
		testPicker = new GachaSystem<GachaTestCharacter>();
	}

    private void Start()
    {
        for (int i = 1; i <= 20; i++) 
        {
            var testClass = new GachaTestCharacter(i.ToString(), i * 10);
            testPicker.Add(testClass, testClass.weight);
        }

        count = new int[20];
    }

    private void Update()
    {

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
		Debug.Log($"이름 : {item.name}, 가중치 : {item.weight}");

        var itemImage = Instantiate(imagePrefab);
        itemImage.transform.SetParent(resultPanel.transform);
		itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(item.name);
        itemImage.color = Color.red * (30 - int.Parse(item.name)) / 30;
		count[int.Parse(item.name) - 1]++;
		SetText();
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

		foreach (var it in item)
		{
			Debug.Log($"이름 : {it.name}, 가중치 : {it.weight}");
			var itemImage = Instantiate(imagePrefab);
			itemImage.transform.SetParent(resultPanel.transform);
            itemImage.GetComponentInChildren<TextMeshProUGUI>().SetText(it.name);
			itemImage.color = Color.red * (30 - int.Parse(it.name)) / 30;

            count[int.Parse(it.name) - 1]++;
		}

        SetText();
	}

    public void SetText()
    {
        StringBuilder stringBuilder = new StringBuilder();

        for(int i=0; i<count.Length; i++)
        {
            stringBuilder.Append($"[{i} : {count[i]}] / ");
        }

        resultText.SetText(stringBuilder.ToString());
    }
}
