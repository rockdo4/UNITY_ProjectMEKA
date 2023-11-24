using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 이 클래스는 내가 점령했다. - 박아무개
public enum UINumeric
{
	Name = 0,
	Company,
	Level,
	HP,
	Damage,
	Armor,
	AttackSpeed,
	Cost,
	CoolDown,
	Block,

	
	Class = 0,
	AttackRange,
}

public class FormationManager : MonoBehaviour
{
	private const int numberOfCharacters = 8;
	private const int numberOfFormations = 4;

	public RectTransform formationPanel;
	public RectTransform characterPanel;
	public RectTransform characterCardScrollView;
	public GameObject characterCardPrefab;

	public TextMeshProUGUI[] textUiArr;
	public Image[] imageUiArr;

	private Button[] characterCard; 

	private List<int[]> formationList;
	private int selectedFormationList;
	private int selectedFormationIndex;
	private int selectedCharacterID;


	private void Awake()
	{
		characterCard = GetComponentsInChildren<Button>();
		formationList = new List<int[]>();
		selectedFormationList = 0;

		for (int i = 0; i < numberOfFormations; i++)
		{
			int[] formation = new int[numberOfCharacters];
			formationList.Add(formation);
		}

		var table = DataTableMgr.GetTable<TestCharacterTable>().GetOriginalTable();

		//캐릭터 리스트 카드 생성
		foreach (var character in table)
		{
			var card = Instantiate(characterCardPrefab);
			card.transform.SetParent(characterCardScrollView);

			var info = card.GetComponent<CardInfo>();
			info.ChangeCardId(character.Key);

			card.GetComponent<Button>().onClick.AddListener(() => 
			{
				ChangeSelectCharacterCard(info);
			});
		}

		//편성 캐릭터 카드 8개 생성
		for (int i = 0; i < numberOfCharacters; i++)
		{
			int index = i;
			characterCard[i].onClick.AddListener(() =>
			{
				characterPanel.gameObject.SetActive(true);
				
				selectedFormationIndex = index;
				Debug.Log(selectedFormationIndex);
			}
			);
		}
	}

	private void Start()
	{
		
	}

	public void OnClickCharacterCard(int index)
	{

	}

	//편성 프리셋 바꾸기
	public void ChangeFormationSet(int formationListIndex)
	{
		selectedFormationList = formationListIndex;
		for (int i = 0; i < numberOfCharacters; i++)
		{
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(formationList[selectedFormationList][i]);
		}
	}


	//캐릭터 카드 클릭하면 캐릭터 리스트 띄움
	//현재 편성된 캐릭터는 띄우지 않음
	public void OpenCharacterList(int index)
	{
		var table = DataTableMgr.GetTable<TestCharacterTable>().GetOriginalTable();

		foreach (var character in table)
		{
			
		}

		//characterCardScrollView.gameObject
	}

	//캐릭터 리스트 끄기
	public void CloseCharacterList()
	{
		characterPanel.gameObject.SetActive(false);
		ResetSelectCharacterCard();
	}

	//선택 누르면 캐릭터 그 카드 id 바꿈
	public void ChangeCharacterCard()
	{
		//카드정보 바꿈
		var cardInfo = characterCard[selectedFormationIndex].GetComponent<CardInfo>();
		cardInfo.ChangeCardId(selectedCharacterID);

		//리스트 바꿈
		formationList[selectedFormationList][selectedFormationIndex] = selectedCharacterID;

		//닫음
		CloseCharacterList();
	}

	//현재 선택한 카드 변경
	public void ChangeSelectCharacterCard(CardInfo card)
	{
		var ID = card.GetCardID();

		selectedCharacterID = ID;

		var info = DataTableMgr.GetTable<TestCharacterTable>().GetCharacterData(ID);

		textUiArr[(int)UINumeric.Name].SetText("Name:"+info.Name);
		textUiArr[(int)UINumeric.Company].SetText("Rare:"+info.Rare.ToString());
		textUiArr[(int)UINumeric.Level].SetText("Lv:"+info.Level.ToString());

		textUiArr[(int)UINumeric.HP].SetText("Wt:"+info.Weight.ToString());
		textUiArr[(int)UINumeric.Damage].SetText("Dmg:"+info.Weight.ToString());
	}

	public void ResetSelectCharacterCard()
	{
		selectedCharacterID = 0;
	}
}
