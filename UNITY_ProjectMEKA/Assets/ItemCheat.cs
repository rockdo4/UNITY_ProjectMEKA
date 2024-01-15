using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UI;

[CustomEditor(typeof(ItemCheat))] 
public class ItemCheatEditor : Editor 
{ 
	public override void OnInspectorGUI() 
	{
		base.OnInspectorGUI(); 
		if (GUILayout.Button("UpdateItem")) 
		{
			ItemCheat itemCheat = (ItemCheat)target;
			if (itemCheat != null)
			{
				itemCheat.RunFunction();
			}
		}

		if (GUILayout.Button("AddAllItem"))
		{
			ItemCheat itemCheat = (ItemCheat)target;
			if (itemCheat != null)
			{
				itemCheat.RunFunction2();
			}
		}
	} 
}

public class ItemCheat : MonoBehaviour
{
	public ItemCardManager cardManager;

	[SerializeField]
    public List<Item> inven;

	private void Start()
	{
		inven = ItemInventoryManager.Instance.m_ItemStorage;
	}	

	// 버튼이 눌렸을 때 실행될 함수
	public void RunFunction()
	{
		cardManager.UpdateItemCard();
	}

	public void RunFunction2()
	{
		cardManager.GetComponentInParent<TableTest>().OnClickAddItem();
	}
}
