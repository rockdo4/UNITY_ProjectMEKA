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

	public TextMeshProUGUI[] textUiArr;
	public Image[] imageUiArr;

	private Button[] characterCard; 

	private List<int[]> formationList;
	private int selectedFormationList;
	private int selectedFormationIndex;
	private int selectedCharacterID;

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

		////편성 캐릭터 카드 델리게이트 추가
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

		//버튼 델리게이트 할당
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

	//편성 프리셋 바꾸기
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
					//중복캐릭터 가리기
					cardList[i].gameObject.SetActive(false);
					activeFalseList.Add(cardList[i].gameObject);
				}
			}
		}
	}

	//삭제 팝업창 열기
	public void OpenDeletePopUp()
	{
		popupPanel.gameObject.SetActive(true);
	}

	//삭제 팝업창 닫기
	public void CloseDeletePopUp()
	{
		popupPanel.gameObject.SetActive(false);
	}

	//다음 프리셋으로 바꾸기
	public void ChangePresetPrevious()
	{
		if (selectedFormationList > 0)
		{
			ChangeFormationSet(selectedFormationList - 1);
		}
	}

	//이전 프리셋으로 바꾸기
	public void ChangePresetNext()
	{
		if(selectedFormationList < numberOfFormations - 1)
		{
			ChangeFormationSet(selectedFormationList + 1);
		}
	}

	//캐릭터 선택하는 창 끄기
	public void CloseCharacterList()
	{
		var table = DataTableMgr.GetTable<TestCharacterTable>();
   
		characterPanel.gameObject.SetActive(false);
		ResetSelectCharacterCard();
	}

	//캐릭터 선택하는 창 열기
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
					//중복캐릭터 가리기
					cardList[i].gameObject.SetActive(false);
					activeFalseList.Add(cardList[i].gameObject);
				}
			}
		}
	}

	//캐릭터 카드 갖고 있는지 확인
	public void CheckCollectCharacter()
	{
		var table = DataTableMgr.GetTable<CharacterTable>();

		for (int i = 0; i < cardList.Length; i++)
		{
			var cardInfo = cardList[i].GetComponent<CardInfo>();
            var cardId = cardInfo.GetCardID();
            var cardData = table.GetCharacterData(cardId);
			
			if(!CharacterManager.Instance.m_CharacterStorage[cardId].IsUnlock)
			{
				cardList[i].gameObject.SetActive(false);
            }
            else
			{
                cardList[i].gameObject.SetActive(true);
            }
		}
	}

	//캐릭터 선택창 카드 활성화 시키기
	public void UpdateActiveCard()
	{
		foreach(var card in activeFalseList)
		{
			card.SetActive(true);
		}
		activeFalseList.Clear();
	}

	//선택 누르면 캐릭터 그 카드 id 바꿈
	public void ChangeCharacterCard()
	{
		//카드정보 바꿈
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
			//카드 글자 바꿈
			cardInfo.ChangeCardId(selectedCharacterID);

			//selectedFormationList프리셋에 selectedFormationIndex인덱스의 아이디 바꿈
			formationList[selectedFormationList][selectedFormationIndex] = selectedCharacterID;
            activeFalseList.Add(cardInfo.gameObject);
        }

		//닫음
		CloseCharacterList();
		UpdatePlayData();
		GameManager.instance.SaveExecution();
	}

	//현재 선택한 카드 변경
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

	//현재 프리셋 지우기
	public void OnClickDeleteCurrentFormation()
	{
		for (int i = 0; i < numberOfCharacters; i++)
		{
			formationList[selectedFormationList][i] = 0;
			characterCard[i].GetComponent<CardInfo>().ChangeCardId(0);
		}

		UpdateActiveCard();
		UpdatePlayData();
		GameManager.instance.SaveExecution();
	}

	//캐릭터 인포 열기
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
		DataHolder.formation = formationList[selectedFormationList];
		DataHolder.formation = DataHolder.formation.Where(x => x != 0).ToArray();
	}
}
