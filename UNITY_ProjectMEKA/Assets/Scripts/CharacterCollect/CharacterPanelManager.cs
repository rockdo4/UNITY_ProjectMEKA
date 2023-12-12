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
	private Vector3 characterInfoPos;

	private void Awake()
	{
		dict = DataTableMgr.GetTable<CharacterTable>();
		charDict = dict.GetOriginalTable();
		characterInfoPos = characterInfoPanel.position;

		var charStorage = CharacterManager.Instance.m_CharacterStorage;

		foreach(var character in charStorage)
		{
			var card = Instantiate(characterCardPrefab, transform);
			var characterInfo = dict.GetCharacterData(character.Value.CharacterID);

			card.name = characterInfo.CharacterName;
			card.GetComponentInChildren<TextMeshProUGUI>().SetText($"{characterInfo.CharacterName}");

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
				Debug.LogError("버튼 못불러옴");
		}

		//foreach (var item in charDict)
		//{
		//	var card = Instantiate(characterCardPrefab, transform);

		//	card.name = item.Value.CharacterName.ToString();
		//	card.GetComponentInChildren<TextMeshProUGUI>().SetText($"{item.Value.CharacterName}");

		//	var button = card.GetComponent<Button>();

		//	if (button != null)
		//		button.onClick.AddListener( () => 
		//		{
		//			OpenCharacterInfo(item.Value);
		//		});
		//	else
		//		Debug.LogError("버튼 못불러옴");
		//}
	}

	public void PickUpCharacter(int ID)
	{
		//Debug.Log(ID);
		var chara = dict.GetCharacterData(ID);

		CharacterManager.Instance.m_CharacterStorage[ID].IsUnlock = true;

        Debug.Log("이름 :" + chara.CharacterName);

		//var item = transform.Find($"{chara.CharacterID}");
		//item.GetComponentInChildren<TextMeshProUGUI>().SetText($"{chara.CharacterName}");
	}

	public void ExitCharacterInfo()
	{
		characterInfoPanel.gameObject.SetActive(false);

		characterInfoPanel.position = characterInfoPos;
	}

	public void OpenCharacterInfo(Character info)
	{
		characterInfoPanel.gameObject.SetActive(true);
		characterInfoPanel.GetComponent<CharacterInfoText>().SetText(info);

		var pos = GetComponentInParent<Canvas>().gameObject.transform.position;
		characterInfoPanel.position = pos;
	}

	public void CloseInfo()
	{
		
	}
}
