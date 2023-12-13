using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Defines;

public class IngameStageUIManager : MonoBehaviour
{
    public WindowMode windowMode;
    private WindowMode prevWindowMode;
    private StageManager stageManager;

    // panels
    public GameObject characterInfoPanel;
    public GameObject timeProgressBarPanel;
    public GameObject waveCountPanel;
    public GameObject monsterCountPanel;

    // character info
    public TextMeshProUGUI characterOccupation;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterDescription;

    // cost & wave & monster & life infos
    public TextMeshProUGUI costText;
    public TextMeshProUGUI leftWaveText;
    public TextMeshProUGUI allMonsterCountText;
    public TextMeshProUGUI spawnedMonsterCountText;
    public TextMeshProUGUI houseLifeText;
    public Image costSlider;

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
    private int prevSpawnedMonsterCount;
    private int prevHouseLife;

    LinkedList<Tile> tempTiles = new LinkedList<Tile>();

    private void Awake()
    {
        joystickHandler = joystick.handler;
        cancelButton = joystick.cancelButton;
        collectButton = joystick.collectButton;
        skillButton = joystick.skillButton;
        stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.stageManager).GetComponent<StageManager>();

        isInfoWindowOn = true;
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

        CloseWindow();
    }

    public void WindowModeUpdate()
    {
        if (stageManager.currentPlayer == null)
        {
            windowMode = WindowMode.None;
        }
        else if (!stageManager.currentPlayer.stateManager.firstArranged)
        {
            windowMode = WindowMode.FirstArrange;
        }
        else if (stageManager.currentPlayer.stateManager.firstArranged && !stageManager.currentPlayer.stateManager.secondArranged)
        {
            windowMode = WindowMode.SecondArrange;
        }
        else if (stageManager.currentPlayer.stateManager.secondArranged)
        {
            windowMode = WindowMode.Setting;
        }
        //else if ()
        //{

        //}
    }

    public void WindowSet()
    {
        switch(windowMode)
        {
            case WindowMode.None:
                // ĳ���� ���� off
                ClearTileMesh();
                characterInfoPanel.SetActive(false);
                joystick.gameObject.SetActive(false);
                currentPlayerOnTile = false;
                isInfoWindowOn = true;
                break;
            case WindowMode.FirstArrange:
                // ĳ���� Ÿ���� �ƴϸ� ���� on
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
                // ĳ���� ���� on
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
        }
    }

    public void CloseWindow()
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
                Debug.Log("ĳ���� ���� ����");
                stageManager.currentPlayer.SetState(PlayerController.CharacterStates.Idle);
                stageManager.currentPlayer = null;
                stageManager.currentPlayerIcon = null;
            }
        }
    }

    public void ChangeCharacterInfo()
    {
        characterOccupation.SetText(stageManager.currentPlayer.state.occupation.ToString());
        characterName.SetText(stageManager.currentPlayer.state.name);
        characterDescription.SetText($"���� �����\n�ڼ����ٺ�\n�ڱ��ƹٺ� �������ٺ� ������");
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
        if(prevSpawnedMonsterCount != stageManager.killMonsterCount)
        {
            spawnedMonsterCountText.SetText(stageManager.killMonsterCount.ToString());
            prevSpawnedMonsterCount = stageManager.killMonsterCount;
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
        Debug.Log("���� Ÿ�� �޽� ������Ʈ");
        ClearTileMesh();

        foreach (var tile in stageManager.currentPlayer.attakableTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Attack);
            tempTiles.AddLast(tile);
        }
    }

    public void ClearTileMesh()
    {
        foreach (var tile in tempTiles)
        {
            tile.ClearTileMesh();
        }
        tempTiles.Clear();
    }

    public void Init()
    {
       if(StageDataManager.Instance.selectedStageData != null)
        {
            // �ش� ��忡 ���缭 UI ����
            var id = StageDataManager.Instance.selectedStageData.stageID;
            var stageTable = StageDataManager.Instance.stageTable;
            var stageData = stageTable.GetStageData(id);
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
}
