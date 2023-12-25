using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;

public class IngameStageUIManager : MonoBehaviour
{
    public WindowMode windowMode;
    private WindowMode prevWindowMode;
    private StageManager stageManager;

    public Button exitButton;

    // panels
    public GameObject characterInfoPanel;
    public GameObject timeProgressBarPanel;
    public GameObject waveCountPanel;
    public GameObject monsterCountPanel;
    public GameObject ResultPanel;
    public GameObject itemParentPanel;
    public GameObject starsParentPanel;

    // character info
    public TextMeshProUGUI characterOccupation;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterDescription;

    // cost & wave & monster & life infos
    public TextMeshProUGUI costText;
    public TextMeshProUGUI leftWaveText;
    public TextMeshProUGUI allMonsterCountText;
    public TextMeshProUGUI killMonsterCountText;
    public TextMeshProUGUI houseLifeText;
    public Image costSlider;

    // Result info
    public TextMeshProUGUI stageInfoText;
    public TextMeshProUGUI missionClearText;
    public GameObject starPrefab;
    public GameObject itemPrefab;
    public Image itemImage;
    public TextMeshProUGUI itemCount;
    public TextMeshProUGUI itemName;

    // joystick
    public ArrangeJoystick joystick;
    private ArrangeJoystickHandler joystickHandler;
    private Button cancelButton;
    private Button collectButton;
    private Button skillButton;
    //private Button closeButton;

	// skill
	public TextMeshProUGUI skillTileGuideText;

    public bool currentPlayerChanged;
    public bool currentPlayerOnTile;
    private bool isInfoWindowOn = true;
    private int prevCost;
    private int prevKillMonsterCount;
    private int prevHouseLife;
    public bool isSkillTileWindow;

    // tables
    //private CharacterTable characterTable;
    //private CharacterLevelTable characterLevelTable;
    //private StringTable stringTable;
    //private ItemInfoTable itemInfoTable;

    LinkedList<Tile> tempTiles = new LinkedList<Tile>();

    private void OnEnable()
    {
        //characterTable = DataTableMgr.GetTable<CharacterTable>();
        //characterLevelTable = DataTableMgr.GetTable<CharacterLevelTable>();
        //stringTable = DataTableMgr.GetTable<StringTable>();
        //itemInfoTable = DataTableMgr.GetTable<ItemInfoTable>();

        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        joystickHandler = joystick.handler;
        cancelButton = joystick.cancelButton;
        collectButton = joystick.collectButton;
        skillButton = joystick.skillButton;
        //closeButton = joystick.closeButton;

		isInfoWindowOn = true;
    }

    private void Awake()
    {
        var characterTable = DataTableMgr.GetTable<CharacterTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();
    }

    private void Start()
    {
        Init();
        if(monsterCountPanel.activeSelf)
        {
            allMonsterCountText.SetText(stageManager.allMonsterCount.ToString());
        }
    }

    private void Update()
    {
        // windowMode Update
        prevWindowMode = windowMode;
        WindowModeUpdate();
        CostUpdate();
        KillMonsterCountUpdate();
        HouseLifeUpdate();

        var infoCondition = currentPlayerOnTile && isInfoWindowOn;

        if (prevWindowMode != windowMode || currentPlayerChanged || infoCondition)
        {
            WindowSet();
            if(currentPlayerChanged)
            {
                currentPlayerChanged = false;
                currentPlayerOnTile = false;
            }
            return;
        }

        CloseResultWindow();
        CloseCharacterInfoWindow();
    }

    public void WindowModeUpdate()
    {
        if (stageManager.gameState == GameState.Win || stageManager.gameState == GameState.Die)
        {
            if (stageManager.gameState == GameState.Win)
            {
                windowMode = WindowMode.Win;
            }
            else
            {
                windowMode = WindowMode.Lose;
            }
            return;
        }

        var isCurrentPlayer = stageManager.currentPlayer != null;
        var isFirstArranged = isCurrentPlayer ? stageManager.currentPlayer.stateManager.firstArranged : false;
        var isSecondArranged = isCurrentPlayer ? stageManager.currentPlayer.stateManager.secondArranged : false;

        if (!isCurrentPlayer)
        {
            windowMode = WindowMode.None;
        }
        else if (!isFirstArranged)
        {
            windowMode = WindowMode.FirstArrange;
        }
        else if (isFirstArranged && !isSecondArranged)
        {
            windowMode = WindowMode.SecondArrange;
        }
        else if (isSecondArranged && !isSkillTileWindow)
        {
            windowMode = WindowMode.Setting;
        }
        else if (isSkillTileWindow)
        {
            windowMode = WindowMode.Skill;
        }
    }

    public void WindowSet()
    {
        switch(windowMode)
        {
            case WindowMode.None:
                // 캐릭터 인포 off
                ClearTileMesh();
                characterInfoPanel.SetActive(false);
                joystick.gameObject.SetActive(false);
                skillTileGuideText.gameObject.SetActive(false);
                currentPlayerOnTile = false;
                isInfoWindowOn = true;
                break;
            case WindowMode.FirstArrange:
                // 캐릭터 타일위 아니면 인포 on
                if(!currentPlayerOnTile)
                {
                    characterInfoPanel.SetActive(true);
                    ChangeCharacterInfo();
                }
                else if(currentPlayerOnTile && isInfoWindowOn)
                {
                    characterInfoPanel.SetActive(false);
                    isInfoWindowOn = false;
                }
                joystick.gameObject.SetActive(false);
                stageManager.currentPlayer.ArrangableTileSet(stageManager.currentPlayer.state.occupation);
                ChangeArrangableTileMesh();
                break;
            case WindowMode.SecondArrange:
                //characterInfoPanel.SetActive(false);
                //ChangeCharacterInfo();
                ClearTileMesh();
                joystick.gameObject.SetActive(true);
                joystick.SetPositionToCurrentPlayer(stageManager.currentPlayer.transform);
                joystickHandler.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(false);
                skillButton.gameObject.SetActive(false);
				//closeButton.gameObject.SetActive(false);
                break;
            case WindowMode.Setting:
                // 캐릭터 인포 on
                characterInfoPanel.SetActive(true);
                ChangeCharacterInfo();
                joystick.gameObject.SetActive(true);
                joystick.SetPositionToCurrentPlayer(stageManager.currentPlayer.transform);
                joystickHandler.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(true);
                //closeButton.gameObject.SetActive(true);
				if (stageManager.currentPlayer.skillState.skillType != SkillType.Auto)
                {
                    skillButton.gameObject.SetActive(true);
                }
                ChangeAttackableTileMesh();
                break;
            case WindowMode.Win:
                ResultPanel.SetActive(true);
                WinWindowSet();
                Time.timeScale = 0f;
                break;
            case WindowMode.Lose:
                ResultPanel.SetActive(true);
                LoseWindowSet();
                Time.timeScale = 0f;
                break;
            case WindowMode.Skill:
                skillTileGuideText.gameObject.SetActive(true);
                break;
        }
    }

    public void CloseCharacterInfoWindow()
    {
        var isCurrentPlayerNull = stageManager.currentPlayer == null;
        bool isCurrentPlayerArranged = false;
        if (!isCurrentPlayerNull)
        {
            isCurrentPlayerArranged = stageManager.currentPlayer.stateManager.firstArranged;
        }
        var isSettingMode = windowMode == Defines.WindowMode.Setting;

        //Debug.Log("is ui layer :" + Utils.IsUILayer());

        if (!isCurrentPlayerNull && (!isCurrentPlayerArranged || isSettingMode))
        {
            if (!Utils.IsUILayer() && !Utils.IsCurrentPlayer(stageManager.currentPlayer.gameObject) && Input.GetMouseButtonDown(0))
            {
                Debug.Log("캐릭터 인포 닫힘");
                stageManager.currentPlayer.SetState(PlayerController.CharacterStates.Idle);
                stageManager.currentPlayer = null;
                stageManager.currentPlayerIcon = null;
            }
        }
    }

    public void CloseResultWindow()
    {
        var isResultMode = windowMode == WindowMode.Win || windowMode == WindowMode.Lose;
        if(isResultMode && !Utils.IsUILayer() && Input.GetMouseButtonDown(0))
        {
            CloseScene();
        }
    }

    public void CloseScene()
    {
        Time.timeScale = 1f;
        StageDataManager.Instance.toStageChoicePanel = true;
        SceneManager.LoadScene("MainScene");
    }

    public void ChangeCharacterInfo()
    {
        var characterId = stageManager.currentPlayer.state.id;
        var characterData = StageDataManager.Instance.characterTable.GetCharacterData(characterId);
        var stringId = characterData.OccupationInfoStringID;
        var occupationInfoString = StageDataManager.Instance.stringTable.GetString(stringId);

        // need to apply string table
        characterOccupation.SetText(stageManager.currentPlayer.state.occupation.ToString());
        characterName.SetText(stageManager.currentPlayer.state.name);

        characterDescription.SetText(occupationInfoString);
    }

    public void CostUpdate()
    {
        stageManager.currentCost += Time.deltaTime * 0.5f;

        if ((prevCost != (int)stageManager.currentCost) && stageManager.currentCost <= stageManager.maxCost + 1)
        {
            costText.SetText(stageManager.currentCost.ToString("0"));
            prevCost = (int)stageManager.currentCost;
        }

        float value;
        if(stageManager.currentCost <= stageManager.maxCost)
        {
            value = stageManager.currentCost % 1;
        }
        else
        {
            stageManager.currentCost = stageManager.maxCost;
            value = 0f;
        }
        costSlider.fillAmount = value;
    }

    public void KillMonsterCountUpdate()
    {
        if(prevKillMonsterCount != stageManager.killMonsterCount)
        {
            killMonsterCountText.SetText(stageManager.killMonsterCount.ToString());
            prevKillMonsterCount = stageManager.killMonsterCount;
        }
    }

    public void HouseLifeUpdate()
    {
        if(prevHouseLife != stageManager.currentHouseLife)
        {
            houseLifeText.SetText(stageManager.currentHouseLife.ToString());
            prevHouseLife = stageManager.currentHouseLife;
        }
    }

    public void LoseWindowSet()
    {
        var missionFailText = StageDataManager.Instance.stringTable.GetString("missionFail");
        itemParentPanel.SetActive(false);
        missionClearText.SetText(missionFailText);
        missionClearText.color = Color.red;
    }

    public void WinWindowSet()
    {
        var starNumber = stageManager.tempClearCount;

        for(int i = 0; i < starNumber; ++i)
        {
            Instantiate(starPrefab, starsParentPanel.transform);
        }
    }

    public void ChangeArrangableTileMesh()
    {
        ClearTileMesh();

        foreach(var tile in stageManager.currentPlayer.arrangableTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Arrange);
            tempTiles.AddLast(tile);
        }
    }

    public void ChangeAttackableTileMesh()
    {
        ClearTileMesh();

        foreach (var tile in stageManager.currentPlayer.attackableTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Attack);
            tempTiles.AddLast(tile);
        }
    }

    public void ChangeSkillTileMesh()
    {
        ClearTileMesh();

        foreach (var tile in stageManager.currentPlayer.attackableTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Attack);
            tempTiles.AddLast(tile);
        }

        foreach (var tile in stageManager.currentPlayer.attackableSkillTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Skill);
            tempTiles.AddLast(tile);
        }
    }

    public void ChangeUnActiveTileMesh()
    {
        ClearTileMesh();

        foreach (var tile in stageManager.currentPlayer.attackableSkillTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.UnActive);
            tempTiles.AddLast(tile);
        }

        foreach (var tile in stageManager.currentPlayer.attackableTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Attack);
            tempTiles.AddLast(tile);
        }
    }

    public void ClearTileMesh()
    {
        var isWindowModeSkill = windowMode == WindowMode.Skill;
        foreach (var tile in tempTiles)
        {
            var isMaterialAttack = tile.currentTileMaterial == Tile.TileMaterial.Attack;
            if(isWindowModeSkill && isMaterialAttack)
            {
                continue;
            }
            else if(!isWindowModeSkill || (isWindowModeSkill && !isMaterialAttack))
            {
                tile.ClearTileMesh();
            }
        }
        tempTiles.Clear();
    }

    public void Init()
    {
        exitButton.onClick.AddListener(() =>
        {
            CloseScene();
        });

        if (StageDataManager.Instance.selectedStageData != null)
        {
            // 해당 모드에 맞춰서 UI 세팅
            var id = StageDataManager.Instance.selectedStageData.stageID;
            var stageTable = StageDataManager.Instance.stageTable;
            var stageData = stageTable.GetStageData(id);
            var stageSaveData = StageDataManager.Instance.selectedStageData;

            InitResultPanel(stageData, stageSaveData);

            //switch (stageData.Type)
            //{
            //    case (int)StageMode.Deffense:
            //        monsterCountPanel.SetActive(true);
            //        break;
            //    case (int)StageMode.Annihilation:
            //        monsterCountPanel.SetActive(true);
            //        timeProgressBarPanel.SetActive(true);
            //        break;
            //    case (int)StageMode.Survival:
            //        waveCountPanel.SetActive(true);
            //        timeProgressBarPanel.SetActive(true);
            //        break;
            //}
        }
    }

    public void InitResultPanel(StageData stageData, StageSaveData stageSaveData)
    {
        SetStageInfo(stageData);

        // reward item setting
        for (int i = 0; i < 5; ++i)
        {
            if(i == 0 && stageSaveData.isCleared)
            {
                continue;
            }

            var rewardData = DataTableMgr.GetTable<RewardTable>().GetStageData(stageData.RewardID);
            int id = 0;
            int count = 0;

            var itemGo = Instantiate(itemPrefab, itemParentPanel.transform);
            var itemInfo = itemGo.GetComponent<RewardItemInfo>();

            switch(i)
            {
                case 0:
                    if(rewardData.FirstItemID != 0)
                    {
                        id = rewardData.FirstItemID;
                        count = rewardData.FirstItemCount;
                        itemInfo.itemCountText.SetText(rewardData.FirstItemCount.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 1:
                    if (rewardData.Item1ID != 0)
                    {
                        id = rewardData.Item1ID;
                        count = rewardData.Item1Count;
                        itemInfo.itemCountText.SetText(rewardData.Item1Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 2:
                    if (rewardData.Item2ID != 0)
                    {
                        id = rewardData.Item2ID;
                        count = rewardData.Item2Count;
                        itemInfo.itemCountText.SetText(rewardData.Item2Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 3:
                    if (rewardData.Item3ID != 0)
                    {
                        id = rewardData.Item3ID;
                        count = rewardData.Item3Count;
                        itemInfo.itemCountText.SetText(rewardData.Item3Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 4:
                    if (rewardData.Item4ID != 0)
                    {
                        id = rewardData.Item4ID;
                        count = rewardData.Item4Count;
                        itemInfo.itemCountText.SetText(rewardData.Item4Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
            }
            SetItemInfo(id, itemInfo, rewardData);
            stageManager.rewardList.Add((id, count));
        }
    }

    public void SetStageInfo(StageData stageData)
    {
        // chapter
        var chapter = stageData.ChapterNumber;
        var number = stageData.StageNumber;
        var name = StageDataManager.Instance.stringTable.GetString(stageData.StageNameStringID);
        stageInfoText.SetText($"{chapter} - {number} {name}");
        var clearText = StageDataManager.Instance.stringTable.GetString("missionClear");
        missionClearText.SetText(clearText);
    }

    public void SetItemInfo(int itemID, RewardItemInfo itemInfo, RewardData rewardData)
    {
        // item info setting : sprite
        // item info setting : name
        var isItem = StageDataManager.Instance.itemInfoTable.GetItemData(itemID) != null;
        var isCharacter = StageDataManager.Instance.characterLevelTable.GetLevelData(itemID) != null;

        if (!isItem && !isCharacter)
        {
            return;
        }

        if(isItem)
        {
            var rewardItem = StageDataManager.Instance.itemInfoTable.GetItemData(itemID);
            itemInfo.itemName.SetText(rewardItem.Name);
        }
        else
        {
            var itemIDString = itemID.ToString();
            var characterId = int.Parse(itemIDString.Substring(0, itemIDString.Length - 2));
            var characterData = StageDataManager.Instance.characterTable.GetCharacterData(characterId);
            var characterName = characterData.CharacterName;
            itemInfo.itemName.SetText(characterName);
        }
    }
}
