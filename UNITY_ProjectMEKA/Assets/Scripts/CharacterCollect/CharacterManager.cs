using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
	public


*/

public class CharacterManager : MonoBehaviour
{
    public TestCharacterTable dict;
	public Dictionary<int, TestCharacterInfo> charDict;
	public GameObject characterCardPrefab;

	public RectTransform characterInfoPanel;
	private Vector3 characterInfoPos;

	private void Awake()
	{
		dict = DataTableMgr.GetTable<TestCharacterTable>();
		charDict = dict.GetOriginalTable();
		characterInfoPos = characterInfoPanel.position;

		foreach (var item in charDict)
		{
			var card = Instantiate(characterCardPrefab);
			card.transform.SetParent(transform);

			card.name = item.Key.ToString();
			card.GetComponentInChildren<TextMeshProUGUI>().SetText($"{item.Value.Name}\n{item.Value.count}");

			var button = card.GetComponent<Button>();

			if (button != null)
				button.onClick.AddListener( () => 
				{
					characterInfoPanel.gameObject.SetActive(true);
					characterInfoPanel.GetComponent<CharacterInfoText>().SetText(item.Value);
				});
			else
				Debug.LogError("¹öÆ° ¸øºÒ·¯¿È");
		}
	}

	public void PickUpCharacter(int ID)
	{
		var chara = dict.GetCharacterData(ID);
		chara.count++;
		Debug.Log("ÀÌ¸§ :" + chara.Name + " »ÌÀº È½¼ö : " + chara.count);

		var item = transform.Find($"{chara.ID}");
		item.GetComponentInChildren<TextMeshProUGUI>().SetText($"{chara.Name}\n{chara.count}");
	}

	public void ExitCharacterInfo()
	{
		characterInfoPanel.gameObject.SetActive(false);
	}
}
