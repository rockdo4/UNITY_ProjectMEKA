using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using static Defines;

public class IngameStageUIManager : MonoBehaviour
{
    public WindowMode windowMode;
    private WindowMode prevWindowMode;
    private StageManager stageManager;

    // panels
    [SerializeField, Header("�г�")]
    public CharacterInfo characterInfoPanel;
    public GameObject resultPanelWin;
    public GameObject resultPanelLose;
    public GameObject itemParentPanel;

    // character info
    [SerializeField, Header("ĳ���� ����")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterOccupation;
    public TextMeshProUGUI characterDescription;

    // cost & wave & monster & life & time infos
    [SerializeField, Header("�⺻ UI")]
    public Slider timeProgressSlider;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI leftWaveHeaderText;
    public TextMeshProUGUI leftWaveText;
    public TextMeshProUGUI killMonsterHeaderText;
    public TextMeshProUGUI allMonsterCountText;
    public TextMeshProUGUI killMonsterCountText;
    public TextMeshProUGUI houseLifeText;
    public Image costSlider;

    // Result info
    [SerializeField, Header("���â")]
    public TextMeshProUGUI stageInfoText;
    public GameObject[] stars = new GameObject[3];
    public GameObject itemPrefab;
    public Image itemImage;
    public TextMeshProUGUI itemCount;
    public TextMeshProUGUI itemName;
    public Button backButtonWin;
    public Button backButtonLose;
    public Button replayButton;
    //private UnityEvent BackWin;
    //private UnityEvent BackLose;
    //private UnityEvent Replay;

    // joystick
    [SerializeField, Header("���̽�ƽ")]
    public ArrangeJoystick joystick;
    private ArrangeJoystickHandler joystickHandler;
    private Button cancelButton;
    private Button collectButton;
    private Button skillButton;
    //������ �߰� ��ų ��Ÿ�ӹ�
    [SerializeField, Header("��ų")]
    private Image skillTimerBar;
    private List<GameObject> bgs = new List<GameObject>();
	// skill
	public TextMeshProUGUI skillTileGuideText;

    public bool currentPlayerChanged;
    public bool currentPlayerOnTile;
    private bool isInfoWindowOn = true;
    private int prevCost;
    private int prevKillMonsterCount;
    private int prevHouseLife;
    private int prevWave;
    public bool isSkillTileWindow;

    // table
    private StringTable stringTable;
    private CharacterTable characterTable;
    private SkillInfoTable skillInfoTable;

    LinkedList<Tile> tempTiles = new LinkedList<Tile>();
    LinkedList<Tile> tempAttackableTiles = new LinkedList<Tile>();

    private void OnEnable()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        joystick = GameObject.FindGameObjectWithTag(Tags.joystick).GetComponent<ArrangeJoystick>();
        joystickHandler = joystick.handler;
        cancelButton = joystick.cancelButton;
        collectButton = joystick.collectButton;
        skillButton = joystick.skillButton;
        skillTimerBar = joystick.skillTimerBar;
        bgs = joystick.bgs;
        isInfoWindowOn = true;
        timeProgressSlider.value = 1f;
        backButtonWin.onClick.AddListener(CloseScene);
        backButtonLose.onClick.AddListener(CloseScene);
        replayButton.onClick.AddListener(ReplayEvent);
    }

    private void Awake()
    {
        characterTable = StageDataManager.Instance.characterTable;
        stringTable = StageDataManager.Instance.stringTable;
        skillInfoTable = DataTableMgr.GetTable<SkillInfoTable>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // windowMode Update
        prevWindowMode = windowMode;
        UpdateWindowMode();
        UpdateCost();
        UpdateKillMonsterCount();
        UpdateHouseLife();
        UpdateWave();
        UpdateTimeProgress();

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

        //CloseResultWindow();
        CloseCharacterInfoWindow();
    }

    public void UpdateWindowMode()
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
                // ĳ���� ���� off
                ClearTileMesh();
                characterInfoPanel.gameObject.SetActive(false);
                joystick.gameObject.SetActive(false);
                skillTileGuideText.gameObject.SetActive(false);
                currentPlayerOnTile = false;
                isInfoWindowOn = true;
                break;
            case WindowMode.FirstArrange:
                // ĳ���� Ÿ���� �ƴϸ� ���� on
                if(!currentPlayerOnTile)
                {
                    characterInfoPanel.gameObject.SetActive(true);
                    characterInfoPanel.SetCharacterInfo();
                }
                else if(currentPlayerOnTile && isInfoWindowOn)
                {
                    characterInfoPanel.gameObject.SetActive(false);
                    isInfoWindowOn = false;
                }
                joystick.gameObject.SetActive(false);
                stageManager.currentPlayer.ArrangableTileSet(stageManager.currentPlayer.state.occupation);
                //stageManager.currentPlayer.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                ChangeArrangableTileMesh();
                //ChangeAttackableTileMesh(true);
                break;
            case WindowMode.SecondArrange:
                //characterInfoPanel.SetActive(false);
                //ChangeCharacterInfo();
                ClearTileMesh();
                joystick.gameObject.SetActive(true);
                foreach (var bg in bgs)
                {
                    bg.SetActive(true);
                }
                joystick.SetPositionToCurrentPlayer(stageManager.currentPlayer.transform);
                joystickHandler.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(false);
                skillButton.gameObject.SetActive(false);
                skillTimerBar.gameObject.SetActive(false);
				//closeButton.gameObject.SetActive(false);
                break;
            case WindowMode.Setting:
                // ĳ���� ���� on
                characterInfoPanel.gameObject.SetActive(true);
                characterInfoPanel.SetCharacterInfo();
                //ChangeCharacterInfo();
                joystick.gameObject.SetActive(true);
                joystick.SetPositionToCurrentPlayer(stageManager.currentPlayer.transform);
                joystickHandler.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(true);
                foreach (var bg in bgs)
                {
                    bg.SetActive(false);
                }
                //closeButton.gameObject.SetActive(true);
                if (stageManager.currentPlayer.skillState.skillType != SkillType.Auto)
                {
                    //���⼭ ��ų ������ �ٲ��ָ� �ɵ� currentPlayer, skillState �޾ƿ� �� �����ϲ�
                    var id = stageManager.currentPlayer.state.id;
                    var info = characterTable.GetCharacterData(id);
                    var skillInfo = skillInfoTable.GetSkillDatas(info.SkillID);

                    skillButton.gameObject.SetActive(true);
                    skillTimerBar.gameObject.SetActive(true);
                    foreach(var bg in bgs)
                    {
                        bg.SetActive(false);
                    }

                    skillButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(skillInfo[0].ImagePath);

                }
                ChangeAttackableTileMesh(false);
                break;
            case WindowMode.Win:
                resultPanelWin.SetActive(true);
                characterInfoPanel.gameObject.SetActive(false);
                WinWindowSet();
                Time.timeScale = 0f;
                break;
            case WindowMode.Lose:
                resultPanelLose.SetActive(true);
                characterInfoPanel.gameObject.SetActive(false);
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
                Debug.Log("ĳ���� ���� ����");
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

        // ���� �������� ���̵� �ι�° �ڸ��� = 2 -> PR
        // ���� �������� ���̵� �ι�° �ڸ��� = 3 -> CN
        // ������ ���丮

        var stageIDString = stageManager.stageID.ToString();
        Debug.Log(stageIDString[1]);
        if(Equals(stageIDString[1],'2'))
        {
            StageDataManager.Instance.toAssignmentStageChoicePanel = true;
        }
        else if (Equals(stageIDString[1], '3'))
        {
            StageDataManager.Instance.toChallengeStageChoicePanel = true;
        }
        else
        {
            StageDataManager.Instance.toStoryStageChoicePanel = true;
        }

        SceneManager.LoadScene("MainScene");
    }

    public void ReplayEvent()
    {
        Time.timeScale = 1f;
        var currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ChangeCharacterInfo()
    {
        var stringTable = StageDataManager.Instance.stringTable;
        var characterId = stageManager.currentPlayer.state.id;
        var characterData = StageDataManager.Instance.characterTable.GetCharacterData(characterId);

        var characterOccupationStringId = characterData.OccupationStringID;
        var occupation = stringTable.GetString(characterOccupationStringId);
        characterOccupation.SetText(occupation);

        var characterNameStringId = characterData.CharacterNameStringID;
        var name = stringTable.GetString(characterNameStringId);
        characterName.SetText(name);

        var characterOccupationInfoStringId = characterData.OccupationInfoStringID;
        var occupationInfo = StageDataManager.Instance.stringTable.GetString(characterOccupationInfoStringId);
        characterDescription.SetText(occupationInfo);
    }

    public void UpdateCost()
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

    public void UpdateKillMonsterCount()
    {
        if(prevKillMonsterCount != stageManager.killMonsterCount)
        {
            killMonsterCountText.SetText(stageManager.killMonsterCount.ToString());
            prevKillMonsterCount = stageManager.killMonsterCount;
        }
    }

    public void UpdateHouseLife()
    {
        if(prevHouseLife != stageManager.currentHouseLife)
        {
            houseLifeText.SetText(stageManager.currentHouseLife.ToString());
            prevHouseLife = stageManager.currentHouseLife;
        }
    }

    public void UpdateWave()
    {
        if(prevWave != stageManager.leftWaveCount)
        {
            leftWaveText.SetText(stageManager.leftWaveCount.ToString());
            prevWave = stageManager.leftWaveCount;
        }
    }

    public void UpdateTimeProgress()
    {
        timeProgressSlider.value = (float)((stageManager.maxTime - stageManager.timer) / stageManager.maxTime);
    }

    public void LoseWindowSet()
    {
        //var missionFailText = StageDataManager.Instance.stringTable.GetString("missionFail");
        //itemParentPanel.SetActive(false);
    }

    public void WinWindowSet()
    {
        var starNumber = stageManager.tempClearCount;
        var tempNumber = 0;

        for(int i = 0; i < stars.Length; ++i)
        {
            tempNumber++;
            if(tempNumber <= starNumber)
            {
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
            }
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

    public void ChangeAttackableTileMesh(bool isFirstArranging)
    {
        if(isFirstArranging == false)
        {
            ClearTileMesh();
        }
        else
        {
            ClearAttackableTileMesh();
        }

        foreach (var tile in stageManager.currentPlayer.attackableTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Attack);
            tempTiles.AddLast(tile);
            tempAttackableTiles.AddLast(tile);
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

    public void ClearAttackableTileMesh()
    {
        foreach(var tile in tempAttackableTiles)
        {
            // ��� ���鼭 ����� �������� üũ �� ������� �ٲٱ�
            if(stageManager.currentPlayer.arrangableTiles.Contains(tile))
            {
                tile.SetTileMaterial(Tile.TileMaterial.Arrange);
            }
            else
            {
                tile.ClearTileMesh();
            }
        }
        tempAttackableTiles.Clear();
    }

    public void Init()
    {
        var joystickButtonsParentTr = joystick.transform.GetChild(5);
        //cancelText = joystickButtonsParentTr.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        //collectText = joystickButtonsParentTr.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        //skillText = joystickButtonsParentTr.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();

        var arrangement = stringTable.GetString("arrangement");
        var cancel = stringTable.GetString("cancel");
        var arrangeCancel = new string($"{arrangement}\n{cancel}");
        //cancelText.SetText(arrangeCancel);
        //collectText.SetText(stringTable.GetString("collection"));
        //skillText.SetText(stringTable.GetString("skill"));
        killMonsterHeaderText.SetText(stringTable.GetString("killMonsterCount"));

        var wave = stringTable.GetString("wave");
        var waveTextAdditional = stringTable.GetString("waveTextAdditional");
        var waveHeaderText = new string($"{wave}:       {waveTextAdditional}");
        leftWaveHeaderText.SetText(waveHeaderText);

        if (StageDataManager.Instance.selectedStageData != null)
        {
            // �ش� ��忡 ���缭 UI ����
            var id = StageDataManager.Instance.selectedStageData.stageID;
            var stageTable = StageDataManager.Instance.stageTable;
            var stageData = stageTable.GetStageData(id);
            var stageSaveData = StageDataManager.Instance.selectedStageData;

            InitResultPanel(stageData, stageSaveData);
        }

        allMonsterCountText.SetText(stageManager.allMonsterCount.ToString());
    }

    public void InitResultPanel(StageData stageData, StageSaveData stageSaveData)
    {
        SetStageInfo(stageData);

        // reward item setting
        for (int i = 0; i < 9; ++i)
        {
            if ((i == 0) && stageSaveData.isCleared)
            {
                continue;
            }

            var rewardData = StageDataManager.Instance.rewardTable.GetStageData(stageData.RewardID);
            int id = 100;
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
                case 5:
                    if(rewardData.Item5ID != 0)
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
                case 6:
                    if (rewardData.Item6ID != 0)
                    {
                        id = rewardData.Item6ID;
                        count = rewardData.Item6Count;
                        itemInfo.itemCountText.SetText(rewardData.Item6Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 7:
                    if (rewardData.Item7ID != 0)
                    {
                        id = rewardData.Item7ID;
                        count = rewardData.Item7Count;
                        itemInfo.itemCountText.SetText(rewardData.Item7Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 8:
                    if (rewardData.Item8ID != 0)
                    {
                        id = rewardData.Item8ID;
                        count = rewardData.Item8Count;
                        itemInfo.itemCountText.SetText(rewardData.Item8Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 9:
                    if (rewardData.Item9ID != 0)
                    {
                        id = rewardData.Item9ID;
                        count = rewardData.Item9Count;
                        itemInfo.itemCountText.SetText(rewardData.Item9Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
                case 10:
                    if (rewardData.Item10ID != 0)
                    {
                        id = rewardData.Item10ID;
                        count = rewardData.Item10Count;
                        itemInfo.itemCountText.SetText(rewardData.Item10Count.ToString());
                    }
                    else
                    {
                        itemGo.SetActive(false);
                    }
                    break;
            }
            SetItemInfo(id, itemInfo);
            stageManager.rewardList.Add((id, count));
        }
    }

    public void SetStageInfo(StageData stageData)
    {
        // chapter
        var chapter = stageData.ChapterNumber;
        var number = stageData.StageNumber;
        var name = StageDataManager.Instance.stringTable.GetString(stageData.StageNameStringID);
        stageInfoText.SetText($"{chapter} - {number}\n{name}");
        var clearText = StageDataManager.Instance.stringTable.GetString("missionClear");
    }

    public void SetItemInfo(int itemID, RewardItemInfo itemInfo)
    {
        // item info setting : sprite
        // item info setting : name
        var isItem = StageDataManager.Instance.itemInfoTable.GetItemData(itemID) != null;
        var isCharacter = StageDataManager.Instance.characterLevelTable.GetLevelData(itemID) != null;
        var isSystem = Enum.IsDefined(typeof(SystemForUnlock), itemID);
        var stringTable = StageDataManager.Instance.stringTable;
        var itemInfoTable = StageDataManager.Instance.itemInfoTable;

        if(!isItem && !isCharacter && !isSystem)
        {
            return;
        }

        if(isItem || isSystem)
        {
            var rewardItem = StageDataManager.Instance.itemInfoTable.GetItemData(itemID);
            var nameStringID = rewardItem.NameStringID;
            var name = stringTable.GetString(nameStringID);
            itemInfo.itemName.SetText(name);

            var itemData = itemInfoTable.GetItemData(itemID);
            Sprite itemSprite = Resources.Load<Sprite>(itemData.ImagePath);
            itemInfo.itemImage.sprite = itemSprite;
        }
        else if(isCharacter)
        {
            var itemIDString = itemID.ToString();
            var characterId = int.Parse(itemIDString.Substring(0, itemIDString.Length - 2));
            var characterData = StageDataManager.Instance.characterTable.GetCharacterData(characterId);
            var characterName = stringTable.GetString(characterData.CharacterNameStringID);
            itemInfo.itemName.SetText(characterName);

            Sprite itemSprite = Resources.Load<Sprite>(characterData.ImagePath);
            itemInfo.itemImage.sprite = itemSprite;
        }
    }
}