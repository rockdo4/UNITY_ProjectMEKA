using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public GameObject ItemParentPanel;

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
    public TextMeshProUGUI chapterText;
    public TextMeshProUGUI stageNumText;
    public TextMeshProUGUI stageNameText;
    public TextMeshProUGUI stageTypeText;
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

    public bool currentPlayerChanged;
    public bool currentPlayerOnTile;
    private bool isInfoWindowOn = true;
    private int prevCost;
    private int prevKillMonsterCount;
    private int prevHouseLife;

    LinkedList<Tile> tempTiles = new LinkedList<Tile>();

    private void OnEnable()
    {
        stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.stageManager).GetComponent<StageManager>();
        joystickHandler = joystick.handler;
        cancelButton = joystick.cancelButton;
        collectButton = joystick.collectButton;
        skillButton = joystick.skillButton;

        isInfoWindowOn = true;

    }

    private void Awake()
    {
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
                windowMode = WindowMode.Loose;
            }
            return;
        }

        var isCurrentPlayer = stageManager.currentPlayer != null;
        var isFirstArranged = isCurrentPlayer ? stageManager.currentPlayer.stateManager.firstArranged : false;
        var isSecondArranged = isCurrentPlayer ? stageManager.currentPlayer.stateManager.secondArranged : false;
        var isSkillUsing = isCurrentPlayer ? stageManager.currentPlayer.GetComponent<SkillBase>().isSkillUsing : false;

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
        else if (isSecondArranged && !isSkillUsing)
        {
            windowMode = WindowMode.Setting;
        }
        else if (isSkillUsing)
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
                skillButton.gameObject.SetActive(true);
                ChangeAttackableTileMesh();
                break;
            case WindowMode.Win:
                ResultPanel.SetActive(true);
                Time.timeScale = 0f;
                break;
            case WindowMode.Loose:
                ResultPanel.SetActive(true);
                Time.timeScale = 0f;
                LooseWindowSet();
                break;
            case WindowMode.Skill:

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
        var isResultMode = windowMode == WindowMode.Win || windowMode == WindowMode.Loose;
        if(isResultMode && Input.GetMouseButtonUp(0))
        {
            CloseScene();
        }
    }

    public void CloseScene()
    {
        Time.timeScale = 1f;
        StageDataManager.Instance.toStageChoicePanel = true;
        SceneManager.LoadScene("GachaSceneDevice");
    }

    public void ChangeCharacterInfo()
    {
        characterOccupation.SetText(stageManager.currentPlayer.state.occupation.ToString());
        characterName.SetText(stageManager.currentPlayer.state.name);
        characterDescription.SetText($"임의 설명글\n박순국바보\n박광훈바보 김주현바보 에베베");
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

    public void LooseWindowSet()
    {
        ItemParentPanel.SetActive(false);
        stageTypeText.SetText("Fail");
        stageTypeText.color = Color.red;
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

            InitResultPanel(stageData);

            switch (stageData.Type)
            {
                case (int)StageMode.Deffense:
                    monsterCountPanel.SetActive(true);
                    break;
                case (int)StageMode.Annihilation:
                    monsterCountPanel.SetActive(true);
                    timeProgressBarPanel.SetActive(true);
                    break;
                case (int)StageMode.Survival:
                    waveCountPanel.SetActive(true);
                    timeProgressBarPanel.SetActive(true);
                    break;
            }
        }
    }

    public void InitResultPanel(StageData stageData)
    {
        SetStageInfo(stageData);

        // reward item setting
        for (int i = 0; i < 5; ++i)
        {
            var rewardData = DataTableMgr.GetTable<RewardTable>().GetStageData(stageData.RewardID);
            int id = 0;
            int count = 0;

            var itemGo = Instantiate(itemPrefab, ItemParentPanel.transform);
            var itemInfo = itemGo.GetComponent<RewardItemInfo>();

            switch(i)
            {
                case 0:
                    if(rewardData.Item1ID != 0)
                    {
                        id = rewardData.Item1ID;
                        count =rewardData.Item1Count;
                        itemInfo.itemCountText.SetText(rewardData.Item1Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 1:
                    if(rewardData.Item2ID != 0)
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
                case 2:
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
                case 3:
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
                case 4:
                    if (rewardData.Item5ID != 0)
                    {
                        id = rewardData.Item5ID;
                        count = rewardData.Item5Count;
                        itemInfo.itemCountText.SetText(rewardData.Item5Count.ToString());
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
        chapterText.SetText(stageData.ChapterNumber);
        stageNumText.SetText(stageData.StageNumber.ToString());
        stageNameText.SetText(stageData.StageName);

        // need to apply string table later
        switch (stageData.Type)
        {
            case (int)StageMode.Deffense:
                stageTypeText.SetText("디펜스 모드");
                break;
            case (int)StageMode.Annihilation:
                stageTypeText.SetText("섬멸 모드");
                break;
            case (int)StageMode.Survival:
                stageTypeText.SetText("생존 모드");
                break;
        }
    }

    public void SetItemInfo(int itemID, RewardItemInfo itemInfo, RewardData rewardData)
    {
        // item info setting : sprite
        // item info setting : name
        if(DataTableMgr.GetTable<ItemInfoTable>().GetItemData(itemID) == null)
        {
            return;
        }
        var rewardItem = DataTableMgr.GetTable<ItemInfoTable>().GetItemData(itemID);
        itemInfo.itemName.SetText(rewardItem.Name);
    }
}
