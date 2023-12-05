using UnityEngine;
using static Defines;

public class CharacterIconManager : MonoBehaviour
{
    public GameObject[] iconPrefabs;
    private int characterCount;


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
            var characterData = characterTable.GetCharacterData(DataHolder.formation[i]);
            
            foreach(var prefab in iconPrefabs)
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
                    break;
                }
            }
        }
    }
}
