using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SynchroInfo
{
	public int itemID;
	public int quantity;
}

public class SynchroPanel : MonoBehaviour
{
	//���߿� ���̺�� ���� �;ߵɵ�?
	[Header("�Ҹ� ������ ID, ���� // ���̺�� �����ؾ� �ҵ�")]
	public SynchroInfo[] itemInfo;
	
	public ItemAutoQuantityCard[] synchroItemCard;
	public Button applyButton;

	public CharacterInfoText UpdateInfoPanel;
	private Character currCharacter;

	private void Awake()
	{
		applyButton.onClick.AddListener(() =>
		{

		});
	}

	private void OnEnable()
	{
		
	}
	
	public void SetCharacter(Character character)
	{
		currCharacter = character;
	}
}
