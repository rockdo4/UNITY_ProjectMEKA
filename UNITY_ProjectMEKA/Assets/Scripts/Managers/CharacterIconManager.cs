using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CharacterIconManager : MonoBehaviour
{
    public GameObject Iconpanel;
    private StageManager stageManager;
    private List<GameObject> characterPrefabs = new List<GameObject>();
    private List<GameObject> characterIconPrefabs = new List<GameObject>();

    public int currentCharacterCount;

    public CharacterTable characterTable;
    private string characterPrefabPath;
    private string characterIconPath;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        characterPrefabPath = "Character";
        characterIconPath = "CharacterIcon/CharacterIconPrefab";
        currentCharacterCount = DataHolder.formation.Length;
        characterTable = DataTableMgr.GetTable<CharacterTable>();
        SetCharacters();
        CreateIconGameObjects();
    }

    public void SetCharacters()
    {
        for(int i = 0; i < currentCharacterCount; i++)
        {
            var id = DataHolder.formation[i];
            var characterData = characterTable.GetCharacterData(id);
            GameObject[] prefabs = Resources.LoadAll<GameObject>(characterPrefabPath);

            foreach (var prefab in prefabs)
            {
                Debug.Log(prefab.name);

                var characterState = prefab.GetComponent<CharacterState>();

                if(characterState == null)
                {
                    Debug.Log("��");
                }
                else
                {
                    Debug.Log(characterState.id);
                }

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
        for(int i = 0; i < currentCharacterCount; i++)
        {
            var id = DataHolder.formation[i];
            var iconImage = Resources.Load<Sprite>(characterTable.GetCharacterData(id).ImagePath);

            var iconPrefab = Resources.Load<GameObject>(characterIconPath);
            var iconGo = Instantiate(iconPrefab, Iconpanel.transform);

            var characterIcon = iconGo.GetComponent<CharacterIcon>();
            var characterIconImage = iconGo.GetComponent<UnityEngine.UI.Image>();

            characterIcon.characterPrefab = characterPrefabs[i];
            characterIconImage.sprite = iconImage;

            characterIconPrefabs.Add(iconGo);
        }
    }
}
