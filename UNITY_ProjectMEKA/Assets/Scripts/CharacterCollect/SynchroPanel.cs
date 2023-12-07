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
	//나중에 테이블로 갖고 와야될듯?
	[Header("소모 아이템 ID, 수량 // 테이블로 수정해야 할듯")]
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
