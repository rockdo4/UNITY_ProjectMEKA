using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynchroPanel : MonoBehaviour
{
	private SynchroData synchroInfoData;
	
	public TextMeshProUGUI levelText;
	public ItemAutoQuantityCard[] synchroItemCard;

	public Button applyButton;

	public CharacterInfoText UpdateInfoPanel;
	private Character currCharacter;

	private CharacterTable charTable;
	private SynchroTable synchroTable;

	private void Awake()
	{
		charTable = DataTableMgr.GetTable<CharacterTable>();
		synchroTable = DataTableMgr.GetTable<SynchroTable>();

		//applyButton.onClick.AddListener(() =>
		//{
		//	//ApplySynchro();

		//	foreach (var card in synchroItemCard)
		//	{
		//		card.ConsumeItem();
		//		card.SetText();
		//	}

		//	//UpdateTargetLevel();
		//});
	}

	private void OnEnable()
	{
		
	}
	
	public void SetCharacter(Character character)
	{
		currCharacter = character;

		var grade = currCharacter.CharacterGrade;
		var occupation = charTable.GetCharacterData(currCharacter.CharacterID).CharacterOccupation;

		synchroInfoData = synchroTable.GetSynchroData(grade, occupation);

		synchroItemCard[0].SetItem(synchroInfoData.Tier1ID, synchroInfoData.RequireTier1);
		synchroItemCard[1].SetItem(synchroInfoData.Tier2ID, synchroInfoData.RequireTier2);
		synchroItemCard[2].SetItem(synchroInfoData.Tier3ID, synchroInfoData.RequireTier3);

		if (currCharacter.CharacterLevel < synchroInfoData.Grade * 10)
		{
			levelText.SetText($"<color=red>{currCharacter.CharacterLevel}</color>");
		}
		else
		{
			levelText.SetText($"{currCharacter.CharacterLevel}");
		}

		UpdateRequired();
	}

	public void UpdateRequired()
	{
		foreach (var card in synchroItemCard)
		{
			card.SetText();
		}
	}

	public void ApplySynchro()
	{
		bool isRequired = true;

		foreach (var card in synchroItemCard)
		{
			isRequired = card.IsEnough();
		}

		if(isRequired)
		{

		}
		else
		{

		}
	}
}
