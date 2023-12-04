using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterInfoUIManager : MonoBehaviour
{
    public Defines.CharacterInfoMode windowMode;
    private Defines.CharacterInfoMode prevWindowMode;
    private StageManager stageManager;

    public ArrangeJoystick joystick;
    private ArrangeJoystickHandler joystickHandler;
    private Button cancelButton;
    private Button collectButton;

    LinkedList<Tile> tempTiles = new LinkedList<Tile>();

    private void Awake()
    {
        joystickHandler = joystick.handler;
        cancelButton = joystick.cancelButton;
        collectButton = joystick.collectButton;
        stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
    }

    private void Update()
    {
        // windowMode Update
        prevWindowMode = windowMode;
        WindowModeUpdate();

        if(prevWindowMode != windowMode)
        {
            WindowSet();
        }

        //if(windowMode == Defines.CharacterInfoMode.Arrange && !arrangeModeSet)
        //{
        //    // current player의 배치 가능 타일 표시
        //    Debug.Log("캐릭터인포 타일 메쉬 변경");
        //    foreach (var tile in stageManager.currentPlayer.arrangableTiles)
        //    {
        //        tile.SetTileMaterial(Tile.TileMaterial.Arrange);
        //    }
        //    arrangeModeSet = true;
        //}
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
        Debug.Log("window set");
        switch(windowMode)
        {
            case Defines.CharacterInfoMode.None:
                // 캐릭터 인포 off
                ClearTileMesh();
                break;
            case Defines.CharacterInfoMode.FirstArrange:
                // 캐릭터 인포 on
                joystick.gameObject.SetActive(false);
                stageManager.currentPlayer.ArrangableTileSet(stageManager.currentPlayer.state.occupation);
                ChangeArrangableTileMesh();
                break;
            case Defines.CharacterInfoMode.SecondArrange:
                // 캐릭터 인포 on
                ClearTileMesh();
                joystick.gameObject.SetActive(true);
                joystick.SetPositionToCurrentPlayer(stageManager.currentPlayer.transform);
                joystickHandler.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(false);
                stageManager.currentPlayer.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                ChangeAttackableTileMesh();
                break;
            case Defines.CharacterInfoMode.Setting:
                // 캐릭터 인포 on
                joystick.gameObject.SetActive(true);
                joystickHandler.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(true);
                ChangeAttackableTileMesh();
                break;
        }
    }

    public void ChangeArrangableTileMesh()
    {
        Debug.Log("ChangeArrangableTileMesh");
        ClearTileMesh();
        //var state = stageManager.currentPlayer.stateManager.currentBase as PlayableArrangeState;
        //state.ArrangableTileSet(stageManager.currentPlayer.state.occupation);

        foreach(var tile in stageManager.currentPlayer.arrangableTiles)
        {
            tile.SetTileMaterial(Tile.TileMaterial.Arrange);
            tempTiles.AddLast(tile);
        }
    }

    public void ChangeAttackableTileMesh()
    {
        Debug.Log("ChangeAttackableTileMesh");
        ClearTileMesh();
        //var state = stageManager.currentPlayer.stateManager.currentBase as PlayableArrangeState;
        //state.AttackableTileSet(stageManager.currentPlayer.state.occupation);

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
