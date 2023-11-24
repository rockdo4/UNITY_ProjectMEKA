using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �� Ŭ������ ���� �����ߴ�. - �ھƹ���
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

		//ĳ���� ����Ʈ ī�� ����
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

		//�� ĳ���� ī�� 8�� ����
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

	//�� ������ �ٲٱ�
	public void ChangeFormationSet(int formationListIndex)
	{
		selectedFormationList = formationListIndex;
		for (int i = 0; i < numberOfCharacters; i++)
		{
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(formationList[selectedFormationList][i]);
		}
	}


	//ĳ���� ī�� Ŭ���ϸ� ĳ���� ����Ʈ ���
	//���� ���� ĳ���ʹ� ����� ����
	public void OpenCharacterList(int index)
	{
		var table = DataTableMgr.GetTable<TestCharacterTable>().GetOriginalTable();

		foreach (var character in table)
		{
			
		}

		//characterCardScrollView.gameObject
	}

	//ĳ���� ����Ʈ ����
	public void CloseCharacterList()
	{
		characterPanel.gameObject.SetActive(false);
		ResetSelectCharacterCard();
	}

	//���� ������ ĳ���� �� ī�� id �ٲ�
	public void ChangeCharacterCard()
	{
		//ī������ �ٲ�
		var cardInfo = characterCard[selectedFormationIndex].GetComponent<CardInfo>();
		cardInfo.ChangeCardId(selectedCharacterID);

		//����Ʈ �ٲ�
		formationList[selectedFormationList][selectedFormationIndex] = selectedCharacterID;

		//����
		CloseCharacterList();
	}

	//���� ������ ī�� ����
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
