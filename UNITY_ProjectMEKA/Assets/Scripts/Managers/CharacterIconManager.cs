using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Defines;

public class CharacterIconManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI personnel;
    private StageManager stageManager;
    private List<GameObject> characterPrefabs = new List<GameObject>();
    private List<GameObject> characterIconPrefabs = new List<GameObject>();

    public int currentCharacterCount;
    private int prevCharacterCount;
    public float currentCost;
    public int prevCost;

    public CharacterTable characterTable;
    private string characterPrefabPath;
    private string characterIconPath;
    public int maxCost = 20;

    private float timer;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        currentCost = maxCost;
        characterPrefabPath = "Character";
        characterIconPath = "CharacterIcon/CharacterIconPrefab";
        currentCharacterCount = DataHolder.formation.Length;
        characterTable = DataTableMgr.GetTable<CharacterTable>();
        SetCharacters();
        CreateIconGameObjects();
    }

    private void Update()
    {
        if(prevCharacterCount != currentCharacterCount)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("배치 가능 인원 : ");
            stringBuilder.Append(currentCharacterCount);
            personnel.SetText(stringBuilder.ToString());
            prevCharacterCount = currentCharacterCount;
        }

        CostUpdate();
    }

    public void CostUpdate()
    {
        currentCost += Time.deltaTime * 0.5f;

        if((prevCost != (int)currentCost) && currentCost <= maxCost+1)
        {
            costText.SetText(currentCost.ToString("0"));
            prevCost = (int)currentCost;
        }

        var value = currentCost % 1f;
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
            var iconGo = Instantiate(iconPrefab, panel.transform);

            var characterIcon = iconGo.GetComponent<CharacterIcon>();
            var characterIconImage = iconGo.GetComponent<UnityEngine.UI.Image>();

            characterIcon.characterPrefab = characterPrefabs[i];
            characterIconImage.sprite = iconImage;

            characterIconPrefabs.Add(iconGo);
        }
    }
}
