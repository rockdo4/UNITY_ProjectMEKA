using System.Collections.Generic;
using TMPro;
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

	private void Start()
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

			var port = card.GetComponent<SetPortrait>();
			if(port != null)
			{
                port.SetCharacterInfo(character.Value);
            }

			//card.GetComponent<Image>().sprite = Resources.Load<Sprite>(characterInfo.PortraitPath);
			//card.GetComponent<CardInfo>().ChangeCardId(characterInfo.CharacterID);

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

			if (characterInfo.PortraitPath == "None")
			{
				card.SetActive(false);
			}
		}

		enableButton.onClick.AddListener(() =>
		{
			CharacterUnlockUpdate();
		});
	}

	public void CharacterUnlockUpdate()
	{
		var list = GetComponentsInChildren<SetPortrait>();

		foreach(var item in list)
		{
            var id = item.GetCardID();
            var info = dict.GetCharacterData(id);

			if (CharacterManager.Instance.m_CharacterStorage[id].IsUnlock)
			{
                item.GetComponent<Button>().interactable = true;
				item.SetUnLock();
            }
            else
			{
                item.GetComponent<Button>().interactable = false;
				item.SetLock();
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
