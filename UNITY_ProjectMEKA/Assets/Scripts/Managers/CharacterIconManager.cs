using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Defines;
using static GateController;

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
                //Debug.Log(prefab.name);
                
                //var characterState = prefab.GetComponent<CharacterState>();
                var characterState = prefab.GetComponent<PlayerState>();

                if(characterState == null)
                {
                    Debug.Log("없음");
                }
                else
                {
                    //Debug.Log(characterState.id);
                }

                if(characterState.id == id)
                {
                    var data = CharacterManager.Instance.m_CharacterStorage[id];
                    var stringTable = StageDataManager.Instance.stringTable;
                    //characterState.arrangeCost = data.ArrangementCost;

					characterState.name = stringTable.GetString(characterData.CharacterNameStringID);
                    characterState.property = (Property)characterData.CharacterProperty;
                    characterState.occupation = (Occupation)characterData.CharacterOccupation;
                    characterState.arrangeCost = characterData.ArrangementCost;
                    characterState.arrangeCoolTime = characterData.ReArrangementCoolDown;

					characterState.damage = data.Damage;
					characterState.maxHp = data.HP;
					characterState.Hp = data.HP;
					characterState.armor = data.Armor;
					characterState.arrangeCost = (int)data.ArrangementCost;

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
            var characterIconImage = iconGo.GetComponent<Image>();

            characterIcon.characterPrefab = characterPrefabs[i];
            characterIconImage.sprite = iconImage;

            characterIconPrefabs.Add(iconGo);
        }
    }
}
