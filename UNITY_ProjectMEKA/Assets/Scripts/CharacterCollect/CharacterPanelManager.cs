using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
	public

*/

public class CharacterPanelManager : MonoBehaviour
{
	public CharacterTable dict;
	public Dictionary<int, CharacterData> charDict;
	public GameObject characterCardPrefab;

	public RectTransform characterInfoPanel;
	public RectTransform characterAffectionPanel;
	public Button enableButton;

	private Vector3 characterInfoPos;

	private void Awake()
	{
		dict = DataTableMgr.GetTable<CharacterTable>();
		var stringTable = StageDataManager.Instance.stringTable;
		charDict = dict.GetOriginalTable();
		characterInfoPos = characterInfoPanel.position;

		var charStorage = CharacterManager.Instance.m_CharacterStorage;

		foreach(var character in charStorage)
		{
			var characterInfo = dict.GetCharacterData(character.Value.CharacterID);

			if (characterInfo.PortraitPath == "None")
			{
				continue;
			}

			var card = Instantiate(characterCardPrefab, transform);
			card.name = stringTable.GetString(characterInfo.CharacterNameStringID);
			card.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(characterInfo.PortraitPath);
			card.GetComponentInChildren<CardInfo>().ChangeCardId(characterInfo.CharacterID);

			var button0 = card.GetComponentsInChildren<Button>()[0];
			var button1 = card.GetComponentsInChildren<Button>()[1];

			if(button1 != null)
				button1.onClick.AddListener(() =>
				{
					OpenCharacterInfo(character.Value);
					if (!character.Value.IsUnlock)
					{
						//CloseInfo();
					}
				});
			else
				Debug.LogError("��ư ���ҷ���");

			if(button0 != null)
			{
				button0.onClick.AddListener(() =>
				{
					Debug.Log("캐릭터 창 오픈");
					characterAffectionPanel.gameObject.SetActive(true);
					characterAffectionPanel.GetComponent<AffectionPanel>().SetCharacter(character.Value);
				});
			}

			if (characterInfo.PortraitPath == "None")
			{
				card.SetActive(false);
			}
		}

		enableButton.onClick.AddListener(() =>
		{
			CharacterUnlockUpdate();
		});

		characterAffectionPanel.gameObject.SetActive(false);
	}

	public void CharacterUnlockUpdate()
	{
		var count = transform.childCount;

		for(int i = 0; i < count; i++)
		{
			var card = transform.GetChild(i); 
			var info = card.GetComponentInChildren<CardInfo>();
			var id = info.GetCardID();

			if(CharacterManager.Instance.m_CharacterStorage[id].IsUnlock)
			{
				card.GetComponentsInChildren<Button>()[0].interactable = true;
				card.GetComponentsInChildren<Button>()[1].interactable = true;
			}
			else
			{
				card.GetComponentsInChildren<Button>()[0].interactable = false;
				card.GetComponentsInChildren<Button>()[1].interactable = false;
			}
		}
	}

	public void PickUpCharacter(int ID)
	{
		//Debug.Log(ID);
		var chara = dict.GetCharacterData(ID);

		CharacterManager.Instance.m_CharacterStorage[ID].IsUnlock = true;

        //Debug.Log("�̸� :" + chara.CharacterName);

		//var item = transform.Find($"{chara.CharacterID}");
		//item.GetComponentInChildren<TextMeshProUGUI>().SetText($"{chara.CharacterName}");
	}

	public void ExitCharacterInfo()
	{
		characterInfoPanel.gameObject.SetActive(false);

		//characterInfoPanel.position = characterInfoPos;
	}

	public void OpenCharacterInfo(Character info)
	{
		characterInfoPanel.gameObject.SetActive(true);
		characterInfoPanel.GetComponent<CharacterInfoText>().SetCharacter(info);

		//var pos = GetComponentInParent<Canvas>().gameObject.transform.position;
		//characterInfoPanel.position = pos;
	}
}
