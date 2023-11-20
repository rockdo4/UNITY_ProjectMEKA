using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GachaTestClass
{
    public int id;
    public string name;
    public int weight;

    public GachaTestClass(string name, int weight)
    {
        this.name = name;
        this.weight = weight;
        id = 0;
    }
}

public class GachaManager : MonoBehaviour
{
    private GachaSystem<GachaTestClass> testPicker;

    private void Awake()
    {
		testPicker = new GachaSystem<GachaTestClass>();
	}

    private void Start()
    {
        for (int i = 1; i < 11; i++) 
        {
            var testClass = new GachaTestClass(i.ToString(), i * 10);
            testPicker.Add(testClass, testClass.weight);
        }
    }

    private void Update()
    {

    }

    public void Gacha1()
    {
		var item = testPicker.GetItem();
		Debug.Log($"이름 : {item.name}, 가중치 : {item.weight}");
	}

    public void Gacha10()
    {
		var item = testPicker.GetItem(10);
		foreach (var it in item)
		{
			Debug.Log($"이름 : {it.name}, 가중치 : {it.weight}");
		}
	}
}
