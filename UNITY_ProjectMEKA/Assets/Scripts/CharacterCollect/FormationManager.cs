using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum UINumeric
{
	ATK = 0,
	Armor,
	HP,
	Speed,
	CriHit,
	CriDamage,
	Name,
	Lv,

	
	CharImage = 0,
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

	public ModalWindow modalWindow;

	[Header ("Button")]
	public Button deleteButton;
	public Button startButton;
	public Button selectButton;

	public TextMeshProUGUI[] textUiArr;
	public Image[] imageUiArr;

	private Button[] characterCard; 

	private List<int[]> formationList;
	private int selectedFormationList;
	private int selectedFormationIndex;
	private int selectedCharacterID;
	public TextMeshProUGUI currentPresetText;

	private List<GameObject> activeFalseList;
	private SelectCardInfo[] cardList;

	private CharacterTable characterTable;


	private void Awake()
	{
		characterCard = formationPanel.GetComponentsInChildren<Button>();

		if(PlayDataManager.data == null)
		{
			PlayDataManager.Init();
		}

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
			if (character.Value.PortraitPath == "None")
			{
				continue;
			}

			var card = Instantiate(characterCardPrefab);
			card.transform.SetParent(characterCardScrollView);

			var info = card.GetComponent<SelectCardInfo>();
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

			button.onClickButton = new UnityEngine.Events.UnityEvent();
			button.onClickButton.AddListener(() =>
			{
				OpenCharacterList();
				selectedFormationIndex = index;
			});

			button.holdButton = new UnityEngine.Events.UnityEvent();
			button.holdButton.AddListener(() =>
			{
				if (formationList[selectedFormationList][index] != 0)
				{
					var info = CharacterManager.Instance.m_CharacterStorage[formationList[selectedFormationList][index]];
					OpenCharacterInfo(info);
                }
            });
		}

		deleteButton.onClick.RemoveAllListeners();
		deleteButton.onClick.AddListener(() => 
		{
			var panel = characterInfoPanel.GetComponent<CharacterInfoText>();
			panel.SetPopUpPanel(
				"프리셋을 지우시겠습니까?",
				() => DeleteCurrentFormation(),
				"예", "아니오"
				);
		});

		selectButton.onClick.RemoveAllListeners();
		selectButton.onClick.AddListener(ChangeCharacterCard);

		cardList = characterCardScrollView.GetComponentsInChildren<SelectCardInfo>();
	}

	private void Start()
	{
		characterTable = DataTableMgr.GetTable<CharacterTable>();
	}

	private void OnEnable()
	{
		CheckPlayData();
		CheckCollectCharacter();
		OnClickFormationButton();
		ChangeFormationSet(selectedFormationList);
	}

	//Change Formation Preset
	public void ChangeFormationSet(int formationListIndex)
	{
		UpdateActiveCard();
		selectedFormationList = formationListIndex;

		for (int i = 0; i < numberOfCharacters; i++)
		{
			characterCard[i].GetComponent<SelectCardInfo>().ChangeFormationId(formationList[selectedFormationList][i]);
		}

		for (int i = 0; i < cardList.Length; i++)
		{
			var id = cardList[i].GetCardID();

			for (int j = 0; j < numberOfCharacters; j++)
			{
				if (formationList[selectedFormationList][j] == id)
				{
					cardList[i].gameObject.SetActive(false);
					activeFalseList.Add(cardList[i].gameObject);
				}
			}
		}

		currentPresetText.SetText($"현재 프리셋 : {selectedFormationList + 1}");
	}

	//Change Preset Previous
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

	//Change Preset Next
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

	//CloseCharacterList
	public void CloseCharacterList()
	{
		characterPanel.gameObject.SetActive(false);
		ResetSelectCharacterCard();
	}

	//OpenCharacterList
	public void OpenCharacterList()
	{
		characterPanel.gameObject.SetActive(true);

		CheckCollectCharacter();

		for (int i = 0; i < cardList.Length; i++)
		{
			var id = cardList[i].GetCardID();

			for (int j = 0; j < numberOfCharacters; j++)
			{
				if(formationList[selectedFormationList][j] == id)
				{
					cardList[i].gameObject.SetActive(false);
					activeFalseList.Add(cardList[i].gameObject);
				}
			}
		}
	}

	public void CheckCollectCharacter()
	{
		var table = DataTableMgr.GetTable<CharacterTable>();

		for (int i = 0; i < cardList.Length; i++)
		{
			var cardInfo = cardList[i].GetComponent<SelectCardInfo>();
            var cardId = cardInfo.GetCardID();
            var cardData = table.GetCharacterData(cardId);

			if(CharacterManager.Instance.m_CharacterStorage == null)
			{
				Debug.Log("123");
			}

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

	//캐릭터 카드 바꾸기
	public void ChangeCharacterCard()
	{
		var cardInfo = characterCard[selectedFormationIndex].GetComponent<Image>();
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
			cardInfo.sprite = Resources.Load<Sprite>(CharacterManager.Instance.m_CharacterStorage[selectedCharacterID].ImagePath);

			//selectedFormationList�����¿� selectedFormationIndex�ε����� ���̵� �ٲ�
			formationList[selectedFormationList][selectedFormationIndex] = selectedCharacterID;
			//activeFalseList.Add(cardInfo.gameObject);
        }

		//����
		CloseCharacterList();
		UpdatePlayData();
		//UpdateActiveCard();
		GameManager.Instance.SaveExecution();
	}

	//���� ������ ī�� ����
	public void ChangeSelectCharacterCard(SelectCardInfo card)
	{
		var ID = card.GetCardID();

		selectedCharacterID = ID;

		var info = characterTable.GetCharacterData(ID);
		var charInfo = CharacterManager.Instance.m_CharacterStorage[ID];

		var stringTable = StageDataManager.Instance.stringTable; 

		textUiArr[(int)UINumeric.Name].SetText(stringTable.GetString(info.CharacterNameStringID));
		textUiArr[(int)UINumeric.ATK].SetText(charInfo.Damage.ToString());
		textUiArr[(int)UINumeric.Armor].SetText(charInfo.Armor.ToString());
		textUiArr[(int)UINumeric.HP].SetText(charInfo.HP.ToString());
		textUiArr[(int)UINumeric.Speed].SetText("--");
		textUiArr[(int)UINumeric.CriHit].SetText("--");
		textUiArr[(int)UINumeric.CriDamage].SetText("--");

		imageUiArr[(int)UINumeric.CharImage].sprite = Resources.Load<Sprite>(info.ImagePath);
		imageUiArr[(int)UINumeric.CharImage].preserveAspect = false;
	}

	public void ResetSelectCharacterCard()
	{
		selectedCharacterID = 0;
	}

	//Delete Preset
	public void DeleteCurrentFormation()
	{
		for (int i = 0; i < numberOfCharacters; i++)
		{
			formationList[selectedFormationList][i] = 0;
			characterCard[i].GetComponent<SelectCardInfo>().ChangeFormationId(0);
		}

		UpdateActiveCard();
		UpdatePlayData();
		GameManager.Instance.SaveExecution();
	}

	//ĳ���� ���� ����
	public void OpenCharacterInfo(Character info)
	{
		characterInfoPanel.gameObject.SetActive(true);
		characterInfoPanel.GetComponent<CharacterInfoText>().SetCharacter(info);
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
