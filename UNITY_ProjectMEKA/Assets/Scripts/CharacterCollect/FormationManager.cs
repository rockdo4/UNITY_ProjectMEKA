using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
	public RectTransform characterInfoPanel;
	private Vector3 characterInfoPos;

	public RectTransform characterCardScrollView;
	public GameObject characterCardPrefab;

	public RectTransform popupPanel;

	public Button yesButton;
	public Button noButton;
	public Button startButton;

	public TextMeshProUGUI[] textUiArr;
	public Image[] imageUiArr;

	private Button[] characterCard; 

	private List<int[]> formationList;
	private int selectedFormationList;
	private int selectedFormationIndex;
	private int selectedCharacterID;
	public TextMeshProUGUI currentPresetText;

	private List<GameObject> activeFalseList;
	private CardInfo[] cardList;


	private void Awake()
	{
		characterCard = GetComponentsInChildren<Button>();
		formationList = PlayDataManager.data.formationList;
        activeFalseList = new List<GameObject>();
        selectedFormationList = 0;
		characterInfoPos = characterInfoPanel.position;

		while(formationList.Count < numberOfFormations)
		{
			formationList.Add(new int[numberOfCharacters]);
		}

		var characterTable = DataTableMgr.GetTable<CharacterTable>();
		var table = characterTable.GetOriginalTable();

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

		////���� ĳ���� ī�� ��������Ʈ �߰�
		for (int i = 0; i < numberOfCharacters; i++)
		{
			int index = i;

			var button = characterCard[i].AddComponent<ButtonHoldListener>();

			button.onClickButton.AddListener(() =>
			{
				OpenCharacterList();
				selectedFormationIndex = index;
				Debug.Log("click");
			});

			button.holdButton.AddListener(() =>
			{
				if (formationList[selectedFormationList][index] != 0)
				{
					//var info = characterTable.GetCharacterData(formationList[selectedFormationList][index]);
					var info = CharacterManager.Instance.m_CharacterStorage[formationList[selectedFormationList][index]];
					OpenCharacterInfo(info);
                }
            });
		}

		//��ư ��������Ʈ �Ҵ�
		yesButton.onClick.AddListener(() =>
		{
			OnClickDeleteCurrentFormation();
			CloseDeletePopUp();
		});
		noButton.onClick.AddListener(() =>
		{
			CloseDeletePopUp();
		});

		cardList = characterCardScrollView.GetComponentsInChildren<CardInfo>();
	}

	private void OnEnable()
	{
		CheckPlayData();
		CheckCollectCharacter();
		ChangeFormationSet(selectedFormationList);
	}

	//���� ������ �ٲٱ�
	public void ChangeFormationSet(int formationListIndex)
	{
		UpdateActiveCard();
		selectedFormationList = formationListIndex;

		for (int i = 0; i < numberOfCharacters; i++)
		{
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(formationList[selectedFormationList][i]);
		}

		for (int i = 0; i < cardList.Length; i++)
		{
			var id = cardList[i].GetCardID();

			for (int j = 0; j < numberOfCharacters; j++)
			{
				if (formationList[selectedFormationList][j] == id)
				{
					//�ߺ�ĳ���� ������
					cardList[i].gameObject.SetActive(false);
					activeFalseList.Add(cardList[i].gameObject);
				}
			}
		}

		currentPresetText.SetText($"현재 프리셋 : {selectedFormationList + 1}");
	}

	//���� �˾�â ����
	public void OpenDeletePopUp()
	{
		popupPanel.gameObject.SetActive(true);
	}

	//���� �˾�â �ݱ�
	public void CloseDeletePopUp()
	{
		popupPanel.gameObject.SetActive(false);
	}

	//���� ���������� �ٲٱ�
	public void ChangePresetPrevious()
	{
		if (selectedFormationList > 0)
		{
			ChangeFormationSet(selectedFormationList - 1);
		}
		else
		{
			ChangeFormationSet(numberOfFormations - 1);
		}
	}

	//���� ���������� �ٲٱ�
	public void ChangePresetNext()
	{
		if(selectedFormationList < numberOfFormations - 1)
		{
			ChangeFormationSet(selectedFormationList + 1);
		}
		else
		{
			ChangeFormationSet(0);
		}
	}

	//ĳ���� �����ϴ� â ����
	public void CloseCharacterList()
	{
		var table = DataTableMgr.GetTable<TestCharacterTable>();
   
		characterPanel.gameObject.SetActive(false);
		ResetSelectCharacterCard();
	}

	//ĳ���� �����ϴ� â ����
	public void OpenCharacterList()
	{
		characterPanel.gameObject.SetActive(true);

		for (int i = 0; i < cardList.Length; i++)
		{
			var id = cardList[i].GetCardID();

			for (int j = 0; j < numberOfCharacters; j++)
			{
				if(formationList[selectedFormationList][j] == id)
				{
					//�ߺ�ĳ���� ������
					cardList[i].gameObject.SetActive(false);
					activeFalseList.Add(cardList[i].gameObject);
				}
			}
		}
	}

	//ĳ���� ī�� ���� �ִ��� Ȯ��
	public void CheckCollectCharacter()
	{
		var table = DataTableMgr.GetTable<CharacterTable>();

		for (int i = 0; i < cardList.Length; i++)
		{
			var cardInfo = cardList[i].GetComponent<CardInfo>();
            var cardId = cardInfo.GetCardID();
            var cardData = table.GetCharacterData(cardId);

			if (!CharacterManager.Instance.m_CharacterStorage[cardId].IsUnlock)
			{
				cardList[i].gameObject.SetActive(false);
            }
            else
			{
                cardList[i].gameObject.SetActive(true);
            }
		}
	}

	//ĳ���� ����â ī�� Ȱ��ȭ ��Ű��
	public void UpdateActiveCard()
	{
		foreach(var card in activeFalseList)
		{
			card.SetActive(true);
		}
		activeFalseList.Clear();
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
            activeFalseList.Add(cardInfo.gameObject);
        }

		//����
		CloseCharacterList();
		UpdatePlayData();
		UpdateActiveCard();
		GameManager.Instance.SaveExecution();
	}

	//���� ������ ī�� ����
	public void ChangeSelectCharacterCard(CardInfo card)
	{
		var ID = card.GetCardID();

		selectedCharacterID = ID;

		var info = DataTableMgr.GetTable<CharacterTable>().GetCharacterData(ID);

		textUiArr[(int)UINumeric.Name].SetText(info.CharacterName);
		textUiArr[(int)UINumeric.Company].SetText(((Defines.Property)info.CharacterProperty).ToString());
		textUiArr[(int)UINumeric.Level].SetText(((Defines.Occupation)info.CharacterOccupation).ToString());

		textUiArr[(int)UINumeric.HP].SetText("Wt:"+info.ArrangementCost);
		textUiArr[(int)UINumeric.Damage].SetText("Dmg:"+info.ReArrangementCoolDown);
	}

	public void ResetSelectCharacterCard()
	{
		selectedCharacterID = 0;
	}

	//���� ������ �����
	public void OnClickDeleteCurrentFormation()
	{
		for (int i = 0; i < numberOfCharacters; i++)
		{
			formationList[selectedFormationList][i] = 0;
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(0);
		}

		UpdateActiveCard();
		UpdatePlayData();
		GameManager.Instance.SaveExecution();
	}

	//ĳ���� ���� ����
	public void OpenCharacterInfo(Character info)
	{
		characterInfoPanel.gameObject.SetActive(true);
		characterInfoPanel.GetComponent<CharacterInfoText>().SetText(info);

		var pos = GetComponentInParent<Canvas>().gameObject.transform.position;

		characterInfoPanel.position = pos;
	}

	public void UpdatePlayData()
	{
		PlayDataManager.data.formationList = formationList;
	}
	public void CheckPlayData()
	{
		if(PlayDataManager.data.formationList != null)
		{
			formationList = PlayDataManager.data.formationList;
		}
	}

	public void SetHolderFormation()
	{
		var list = formationList[selectedFormationList].Where(x => x != 0).ToArray();
		if(list.Length == 0)
		{
			DataHolder.isVaild = false;
		}
		else
		{
			DataHolder.formation = list;
			DataHolder.isVaild = true;
		}

		// origin code

		//DataHolder.formation = formationList[selectedFormationList];
		//DataHolder.formation = DataHolder.formation.Where(x => x != 0).ToArray();

		//if(DataHolder.formation.Length == 0)
		//	DataHolder.isVaild = false;
		//else
		//	DataHolder.isVaild = true;
	}

	public void OnClickBattleButton()
	{
		startButton.gameObject.SetActive(true);
	}

	public void OnClickFormationButton()
	{
		startButton.gameObject.SetActive(false);
	}
}
