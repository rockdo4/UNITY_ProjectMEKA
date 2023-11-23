using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FormationManager : MonoBehaviour
{
	private const int numberOfCharacters = 8;
	private const int numberOfFormations = 4;

	public RectTransform formationPanel;
	public RectTransform characterPanel;
	public Button[] characterCard; 

	private List<int[]> formationList;
	private int selectFormation;

	private void Awake()
	{
		characterCard = GetComponentsInChildren<Button>();
		formationList = new List<int[]>();
		selectFormation = 0;

		for (int i = 0; i < numberOfFormations; i++)
		{
			int[] formation = new int[numberOfCharacters];
			formationList.Add(formation);
		}			
	}

	private void Start()
	{
		for (int i = 0; i < numberOfCharacters; i++)
		{
			characterCard[i].onClick.AddListener(() =>
			{
				
			}
			);
		}
	}

	public void OnClickCharacterCard(int index)
	{

	}

	//편성 프리셋 바꾸기
	public void ChangeFormationSet(int index)
	{
		selectFormation = index;
		for (int i = 0; i < numberOfCharacters; i++)
		{
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(formationList[selectFormation][i]);
		}
	}

	//캐릭터 카드 클릭하면 캐릭터 정보창 띄우기
	public void OpenCharacterList(int index)
	{

	}
}
