using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int MonsterLevelId;
    public StageUIManager stageUIManager;
    public GameObject monsterInfoPopUpWindow;
    public StringTable stringTable;
    private MonsterLevelData monsterLevelData;
	private bool isDragging = false;

	public void Init(StageUIManager stageUIManager, int id, StringTable stringTable)
    {
        this.stageUIManager = stageUIManager;
        this.stringTable = stringTable;
        monsterInfoPopUpWindow = stageUIManager.monsterInfoPopUpWindow;
        MonsterLevelId = id;
        monsterLevelData = DataTableMgr.GetTable<MonsterLevelTable>().GetMonsterData(MonsterLevelId);
    }

	private void Update()
	{
		if(monsterInfoPopUpWindow.activeSelf)
        {
            var pos = Input.GetTouch(0).position;
			monsterInfoPopUpWindow.transform.position = new Vector3(pos.x, pos.y, monsterInfoPopUpWindow.transform.position.z);
		}
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

        iconPos.x += rect.width * 1.1f;
        iconPos.y += rect.height * 2.2f;

        monsterInfoPopUpWindow.transform.position = iconPos;
	}

	public void OnDrag(PointerEventData eventData)
	{
        if(Utils.IsButtonLayer("UIButton"))
        {
            monsterInfoPopUpWindow.SetActive(false);
        }
	}
	public void OnPointerUp(PointerEventData eventData)
    {
		// ¸¶¿ì½º¸¦ ¶¿ ¶§¸¸ ÆË¾÷Ã¢À» ´ÝÀ½
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
