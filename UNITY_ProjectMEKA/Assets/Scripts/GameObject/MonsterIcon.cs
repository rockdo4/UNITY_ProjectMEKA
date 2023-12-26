using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Defines;

public class MonsterIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int MonsterLevelId;
    public StageUIManager stageUIManager;
    public GameObject monsterInfoPopUpWindow;
    public StringTable stringTable;
    private MonsterLevelData monsterLevelData;

    public void Init(StageUIManager stageUIManager, int id, StringTable stringTable)
    {
        this.stageUIManager = stageUIManager;
        this.stringTable = stringTable;
        monsterInfoPopUpWindow = stageUIManager.monsterInfoPopUpWindow;
        MonsterLevelId = id;
        monsterLevelData = DataTableMgr.GetTable<MonsterLevelTable>().GetMonsterData(MonsterLevelId);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!monsterInfoPopUpWindow.activeSelf)
        {
            monsterInfoPopUpWindow.SetActive(true);
        }

        SetMonsterInfo();

        var rect = GetComponent<RectTransform>().rect;
        var iconPos = transform.position;

        iconPos.x += rect.width * 2f;
        iconPos.y += rect.height * 4f;

        monsterInfoPopUpWindow.transform.position = iconPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 팝업창 액티브 false
        monsterInfoPopUpWindow.SetActive(false);
    }

    public void SetMonsterInfo()
    {
        var damage = monsterLevelData.MonsterAttackDamage;
        var armor = monsterLevelData.MonsterDefense;
        var hp = monsterLevelData.MonsterHP;
        var shield = monsterLevelData.MonsterShield;

        var damageText = $"{stringTable.GetString("damage")} : {damage}";
        var armorText = $"{stringTable.GetString("armor")} : {armor}";
        var hpText = $"{stringTable.GetString("hp")} : {hp}";
        var shieldText = $"{stringTable.GetString("shield")} : {shield}";

        monsterInfoPopUpWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(damageText);
        monsterInfoPopUpWindow.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(armorText);
        monsterInfoPopUpWindow.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(hpText);
        monsterInfoPopUpWindow.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(shieldText);
    }
}
