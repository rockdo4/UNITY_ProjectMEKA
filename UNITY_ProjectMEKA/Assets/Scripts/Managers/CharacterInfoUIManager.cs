using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoUIManager : MonoBehaviour
{
    public Defines.CharacterInfoMode windowMode;
    private Defines.CharacterInfoMode prevWindowMode;
    private StageManager stageManager;

    // character info
    public GameObject characterInfoPanel;
    public TextMeshProUGUI characterOccupation;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterDescription;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI leftWaveText;
    public Image costSlider;

    // joystick
    public ArrangeJoystick joystick;
    private ArrangeJoystickHandler joystickHandler;
    private Button cancelButton;
    private Button collectButton;
    private Button skillButton;

    public bool currentPlayerChanged;

    public int prevCost;
    private float timer;


    LinkedList<Tile> tempTiles = new LinkedList<Tile>();

    private void Awake()
    {
        //currentCost = maxCost;
        joystickHandler = joystick.handler;
        cancelButton = joystick.cancelButton;
        collectButton = joystick.collectButton;
        skillButton = joystick.skillButton;
        stageManager = GameObject.FindGameObjectWithTag(Defines.Tags.stageManager).GetComponent<StageManager>();
    }

    private void Update()
    {
        // windowMode Update
        prevWindowMode = windowMode;
        WindowModeUpdate();
        CostUpdate();

        if (prevWindowMode != windowMode || currentPlayerChanged)
        {
            WindowSet();
            currentPlayerChanged = false;
            return;
        }

        CloseWindow();
    }

    public void WindowModeUpdate()
    {
        if(stageManager.currentPlayer == null)
        {
            windowMode = Defines.CharacterInfoMode.None;
        }
        else if(!stageManager.currentPlayer.stateManager.firstArranged)
        {
            windowMode = Defines.CharacterInfoMode.FirstArrange;
        }
        else if (stageManager.currentPlayer.stateManager.firstArranged && !stageManager.currentPlayer.stateManager.secondArranged)
        {
            windowMode = Defines.CharacterInfoMode.SecondArrange;
        }
        else if (stageManager.currentPlayer.stateManager.secondArranged)
        {
            windowMode = Defines.CharacterInfoMode.Setting;
        }
    }

    public void WindowSet()
    {
        switch(windowMode)
        {
            case Defines.CharacterInfoMode.None:
                // 캐릭터 인포 off
                ClearTileMesh();
                characterInfoPanel.SetActive(false);
                joystick.gameObject.SetActive(false);
                break;
            case Defines.CharacterInfoMode.FirstArrange:
                // 캐릭터 인포 on
                characterInfoPanel.SetActive(true);
                ChangeCharacterInfo();
                joystick.gameObject.SetActive(false);
                stageManager.currentPlayer.ArrangableTileSet(stageManager.currentPlayer.state.occupation);
                ChangeArrangableTileMesh();
                break;
            case Defines.CharacterInfoMode.SecondArrange:
                // 캐릭터 인포 on
                characterInfoPanel.SetActive(true);
                ChangeCharacterInfo();
                ClearTileMesh();
                joystick.gameObject.SetActive(true);
                joystick.SetPositionToCurrentPlayer(stageManager.currentPlayer.transform);
                joystickHandler.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(false);
                skillButton.gameObject.SetActive(false);
                break;
            case Defines.CharacterInfoMode.Setting:
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
        var isSettingMode = windowMode == Defines.CharacterInfoMode.Setting;

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
}
