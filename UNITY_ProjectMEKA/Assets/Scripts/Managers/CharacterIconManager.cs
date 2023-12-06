using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Defines;

public class CharacterIconManager : MonoBehaviour
{
    public GameObject panel;
    public List<GameObject> characterPrefabs = new List<GameObject>();
    public List<GameObject> characterIconPrefabs = new List<GameObject>();
    public int characterCount;

    private CharacterTable characterTable;
    private string characterPrefabPath;
    private string characterIconPath;

    private void Awake()
    {
        characterPrefabPath = "Character";
        characterIconPath = "CharacterIcon/CharacterIconPrefab";
        characterCount = DataHolder.formation.Length;
        characterTable = DataTableMgr.GetTable<CharacterTable>();
        SetCharacters();
        CreateIconGameObjects();
    }

    public void SetCharacters()
    {
        for(int i = 0; i < characterCount; i++)
        {
            // ID에 맞는 캐릭터데이터 가져오기
            var id = DataHolder.formation[i];
            var characterData = characterTable.GetCharacterData(id);
            GameObject[] prefabs = Resources.LoadAll<GameObject>(characterPrefabPath);

            // ID에 맞는 프리팹 가져와서 캐릭터데이터 적용
            foreach (var prefab in prefabs)
            {
                var characterState = prefab.GetComponent<CharacterState>();
                if(characterState.id == id)
                {
                    characterState.name = characterData.CharacterName;
                    characterState.property = (Property)characterData.CharacterProperty;
                    characterState.occupation = (Occupation)characterData.CharacterOccupation;
                    characterState.arrangeCost = characterData.ArrangementCost;
                    characterState.maxCost = characterData.WithdrawCost;
                    characterState.arrangeCoolTime = characterData.ReArrangementCoolDown;
                    characterPrefabs.Add(prefab);
                    break;
                }
            }
        }
    }

    public void CreateIconGameObjects()
    {
        for(int i = 0; i < characterCount; i++)
        {
            var id = DataHolder.formation[i];
            var iconImage = Resources.Load<Sprite>(characterTable.GetCharacterData(id).ImagePath);

            var iconPrefab = Resources.Load<GameObject>(characterIconPath);
            var iconGo = Instantiate(iconPrefab, panel.transform);

            var characterIcon = iconGo.GetComponent<CharacterIcon>();
            var characterIconImage = iconGo.GetComponent<UnityEngine.UI.Image>();

            characterIcon.characterPrefab = characterPrefabs[i];
            characterIconImage.sprite = iconImage;

            characterIconPrefabs.Add(iconGo);
        }
    }
}
