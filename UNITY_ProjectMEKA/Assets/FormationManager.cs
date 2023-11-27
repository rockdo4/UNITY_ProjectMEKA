using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

	private List<GameObject> activeFalseList;


	private void Awake()
	{
		characterCard = GetComponentsInChildren<Button>();
		formationList = new List<int[]>();
        activeFalseList = new List<GameObject>();
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

				//���� ���� ĳ���ʹ� ����� ����
				CheckDuplicationCharacter();
            }
            );
		}
	}

	public void OnClickCharacterCard(int index)
	{

	}

	//�� ������ �ٲٱ�
	public void ChangeFormationSet(int formationListIndex)
	{
		if (selectedFormationList == formationListIndex) return;

		selectedFormationList = formationListIndex;
		for (int i = 0; i < numberOfCharacters; i++)
		{
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(formationList[selectedFormationList][i]);

			for(int j = 0; j < activeFalseList.Count; j++)
			{
				//activeFalseList[j].SetActive(true);
				//���⼭ ���� ���Ѿߵ�
            }
		}
	}


	//���� ���� ĳ���ʹ� ����� ����
	public void CheckDuplicationCharacter()
	{
		var characterCardList = characterCardScrollView.GetComponentsInChildren<CardInfo>();

		foreach(CardInfo cardInfo in characterCardList)
		{
			var ID = cardInfo.GetCardID();
        }
	}

	//ĳ���� ����Ʈ ����
	public void CloseCharacterList()
	{
		var table = DataTableMgr.GetTable<TestCharacterTable>();
   
		characterPanel.gameObject.SetActive(false);
		ResetSelectCharacterCard();
	}

	//���� ������ ĳ���� �� ī�� id �ٲ�
	public void ChangeCharacterCard()
	{
		//ī������ �ٲ�
		var cardInfo = characterCard[selectedFormationIndex].GetComponent<CardInfo>();
		bool isDuplication = false;

		for (int i = 0; i < numberOfCharacters; i++)
		{
			if (formationList[selectedFormationList][i] == selectedCharacterID)
			{
				isDuplication = true;
			}
		}

		if(!isDuplication)
		{
			//ī�� ���� �ٲ�
			cardInfo.ChangeCardId(selectedCharacterID);

			//selectedFormationList�����¿� selectedFormationIndex�ε����� ���̵� �ٲ�
			formationList[selectedFormationList][selectedFormationIndex] = selectedCharacterID;

            cardInfo.GetComponentInChildren<TextMeshProUGUI>().SetText("-");
            activeFalseList.Add(cardInfo.gameObject);
        }

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

	public void OnClickDeleteCurrentFormation()
	{
		for (int i = 0; i < numberOfCharacters; i++)
		{
			formationList[selectedFormationList][i] = 0;
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(0);
		}
	}
}
