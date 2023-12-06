using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CharacterIconManager : MonoBehaviour
{
    public List<GameObject> iconPrefabs = new List<GameObject>();
    public int characterCount;


    private void Awake()
    {
        foreach(var form in DataHolder.formation)
        {
            if (form == 0)
            {
                return;
            }
            characterCount++;
        }
        SetCharactersData();
    }

    public void SetCharactersData()
    {
        var characterTable = DataTableMgr.GetTable<CharacterTable>();
        for(int i = 0; i < characterCount; i++)
        {
            // ID에 맞는 캐릭터데이터 가져오기
            var characterData = characterTable.GetCharacterData(DataHolder.formation[i]);

            GameObject[] prefabs = Resources.LoadAll<GameObject>("Character");

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
                    iconPrefabs.Add(prefab);
                    break;
                }
            }
        }
    }
}
