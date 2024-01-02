using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Defines;
using static GateController;

public class CharacterIconManager : MonoBehaviour
{
    public GameObject Iconpanel;
    private StageManager stageManager;
    public GameObject characterIconPrefab;
    private List<GameObject> characterPrefabs = new List<GameObject>();
    private List<GameObject> characterIconPrefabs = new List<GameObject>();

    public int currentCharacterCount;
    private string characterPrefabPath;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        characterPrefabPath = "Character";
        currentCharacterCount = DataHolder.formation.Length;
        SetCharacters();
        CreateIconGameObjects();
    }

    public void SetCharacters()
    {
        for(int i = 0; i < currentCharacterCount; i++)
        {
            var id = DataHolder.formation[i];
            var characterData = StageDataManager.Instance.characterTable.GetCharacterData(id);
            GameObject[] prefabs = Resources.LoadAll<GameObject>(characterPrefabPath);

            foreach (var prefab in prefabs)
            {
                //Debug.Log(prefab.name);
                
                //var characterState = prefab.GetComponent<CharacterState>();
                var characterState = prefab.GetComponent<PlayerState>();
                var skillState = prefab.GetComponent<SkillBase>();

                if(characterState == null)
                {
                    Debug.Log("없음");
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
                    characterState.level = data.CharacterLevel;
                    skillState.skillLevel = data.SkillLevel;
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
            var iconGo = Instantiate(characterIconPrefab, Iconpanel.transform);
            var characterIcon = iconGo.GetComponent<CharacterIcon>();
            characterIcon.characterPrefab = characterPrefabs[i];
            characterIconPrefabs.Add(iconGo);
        }
    }
}
