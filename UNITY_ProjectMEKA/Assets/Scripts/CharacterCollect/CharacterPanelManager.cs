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
			card.GetComponent<Image>().sprite = Resources.Load<Sprite>(characterInfo.PortraitPath);
			card.GetComponent<CardInfo>().ChangeCardId(characterInfo.CharacterID);
			card.GetComponentInChildren<TextMeshProUGUI>().SetText("");

			var button = card.GetComponent<Button>();

			if(button != null)
				button.onClick.AddListener(() =>
				{
					OpenCharacterInfo(character.Value);
					if (!character.Value.IsUnlock)
					{
						//CloseInfo();
					}
				});
			else
				Debug.LogError("��ư ���ҷ���");
		}

		enableButton.onClick.AddListener(() =>
		{
			CharacterUnlockUpdate();
		});
	}

	public void CharacterUnlockUpdate()
	{
		var count = transform.childCount;

		for(int i = 0; i < count; i++)
		{
			var card = transform.GetChild(i); 
			var info = card.GetComponent<CardInfo>();
			var id = info.GetCardID();

			if(CharacterManager.Instance.m_CharacterStorage[id].IsUnlock)
			{
				card.GetComponent<Button>().interactable = true;
			}
			else
			{
				card.GetComponent<Button>().interactable = false;
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
