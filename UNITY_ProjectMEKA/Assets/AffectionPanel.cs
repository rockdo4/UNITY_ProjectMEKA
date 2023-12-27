using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffectionPanel : MonoBehaviour
{
    public RectTransform scrollContent;
    public AffectionPortrait affectionPrefab;
    public AffectionCommunicationPanel communicationPanel;

    private void Awake()
    {
        communicationPanel.gameObject.SetActive(false);

        CreateCard();
    }

    private void CreateCard()
    {
        var charDict = DataTableMgr.GetTable<CharacterTable>();
        var originalDict = charDict.GetOriginalTable();
        var stringTable = StageDataManager.Instance.stringTable;
        var charStorage = CharacterManager.Instance.m_CharacterStorage;

        foreach (var character in charStorage)
        {
            var characterInfo = charDict.GetCharacterData(character.Value.CharacterID);

            if (characterInfo.PortraitPath == "None")
            {
                continue;
            }

            var card = Instantiate(affectionPrefab, scrollContent);
            card.name = stringTable.GetString(characterInfo.CharacterNameStringID);
            card.SetCharacter(character.Value);

            var button = card.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                communicationPanel.gameObject.SetActive(true);
                communicationPanel.SetCharacter(character.Value);
            });
        }
    }
}
