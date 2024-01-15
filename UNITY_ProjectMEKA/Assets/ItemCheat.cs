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

	// ��ư�� ������ �� ����� �Լ�
	public void RunFunction()
	{
		cardManager.UpdateItemCard();
	}
}