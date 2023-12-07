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
            // 배치가능인원 텍스트 업데이트
            // 쿨타임 돌아가는 애는 배치가능인원에서 제외
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
        // 2초에 1씩 코스트 회복
        currentCost += Time.deltaTime * 0.5f;

        if((prevCost != (int)currentCost) && currentCost <= maxCost+1)
        {
            costText.SetText(currentCost.ToString("0"));
            prevCost = (int)currentCost;
        }

        // 프로그레스바 나중에 적용하기
        var value = currentCost % 1f;
    }

    public void SetCharacters()
    {
        for(int i = 0; i < currentCharacterCount; i++)
        {
            // ID에 맞는 캐릭터데이터 가져오기
            var id = DataHolder.formation[i];
            var characterData = characterTable.GetCharacterData(id);
            GameObject[] prefabs = Resources.LoadAll<GameObject>(characterPrefabPath);

            // ID에 맞는 프리팹 가져와서 캐릭터데이터 적용
            foreach (var prefab in prefabs)
            {
                Debug.Log(prefab.name);

                var characterState = prefab.GetComponent<CharacterState>();

                if(characterState == null)
                {
                    Debug.Log("널");
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
