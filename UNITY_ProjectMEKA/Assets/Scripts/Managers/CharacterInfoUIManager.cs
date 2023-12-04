using UnityEngine;
using System.Collections.Generic;

public class CharacterInfoUIManager : MonoBehaviour
{
    public Defines.CharacterInfoMode windowMode;
    private StageManager stageManager;
    public bool arrangeModeSet;
    public bool settingModeSet;
    LinkedList<Tile> tempTiles = new LinkedList<Tile>();

    private void Awake()
    {
        stageManager = stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
    }

    private void Update()
    {
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
