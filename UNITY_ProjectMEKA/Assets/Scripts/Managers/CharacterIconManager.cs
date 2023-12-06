using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Defines;

public class CharacterIconManager : MonoBehaviour
{
    public GameObject panel;
    public List<GameObject> characterPrefabs = new List<GameObject>();
    public List<GameObject> characterIconPrefabs = new List<GameObject>();
    public int characterCount;
    private string characterPrefabPath;
    private string characterIconPath;

    private void Awake()
    {
        characterPrefabPath = "Character";
        characterIconPath = "CharacterIcon/CharacterIconPrefab";
        characterCount = DataHolder.formation.Length;
        SetCharacters();
    }

    public void SetCharacters()
    {
        var characterTable = DataTableMgr.GetTable<CharacterTable>();
        for(int i = 0; i < characterCount; i++)
        {
            // ID에 맞는 캐릭터데이터 가져오기
            var characterData = characterTable.GetCharacterData(DataHolder.formation[i]);

            GameObject[] prefabs = Resources.LoadAll<GameObject>(characterPrefabPath);

            // ID에 맞는 프리팹 가져와서 캐릭터데이터 적용
            foreach (var prefab in prefabs)
            {
                var characterState = prefab.GetComponent<CharacterState>();
                var id = characterState.id;
                if(id == characterData.CharacterID)
                {
                    characterState.name = characterData.CharacterName;
                    characterState.property = (Property)characterData.CharacterProperty;
                    characterState.occupation = (Occupation)characterData.CharacterOccupation;
                    characterState.arrangeCost = characterData.ArrangementCost;
                    characterState.maxCost = characterData.WithdrawCost;
                    characterState.arrangeCoolTime = characterData.ReArrangementCoolDown;
                    characterPrefabs.Add(prefab);

                    SetCharacterIcon(prefab, characterData.ImagePath);
                    break;
                }
            }
        }
    }

    public void SetCharacterIcon(GameObject characterPrefab, string imagePath)
    {
        var iconPrefab = Resources.Load<GameObject>(characterIconPath);
        var iconImage = Resources.Load<Sprite>(imagePath);

        // 컴포넌트 세팅
        var characterIcon = iconPrefab.GetComponent<CharacterIcon>();
        var characterIconImage = iconPrefab.GetComponent<Image>();
        
        characterIcon.characterPrefab = characterPrefab;
        characterIconImage.sprite = iconImage;

        characterIconPrefabs.Add(iconPrefab);
    }

    public void CreateIcons()
    {

    }
}
