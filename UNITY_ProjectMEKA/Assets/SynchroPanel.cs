using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynchroPanel : MonoBehaviour
{
	[Header("�Ҹ� ������ ID")]
	public int[] itemID;
	
	public ItemQuantityCard[] synchroItemCard;
	public Button applyButton;

	public CharacterInfoText UpdateInfoPanel;
	private Character currCharacter;

	private void Awake()
	{
		
	}

	private void OnEnable()
	{
		
	}
	
	public void SetCharacter(Character character)
	{
		currCharacter = character;
	}
}
