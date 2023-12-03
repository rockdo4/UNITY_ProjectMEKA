using System.Collections.Generic;
using UnityEngine;
using static Defines;


public class PlayableArrangeState : PlayableBaseState
{
    private RaycastHit hit;
    //public Button collecteButton;
    //public bool settingMode;

    public PlayableArrangeState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        Time.timeScale = 0.2f;

        // Set arrange tile mesh
        if (!playerCtrl.stateManager.firstArranged)
        {
            playerCtrl.transform.forward = Vector3.forward;
            var occupation = playerCtrl.state.occupation;
            ArrangableTileSet(occupation);
            AttackableTileSet(occupation);
            //if (playerCtrl.stateManager.tiles != null)
            //{
            //    foreach (var tile in playerCtrl.stateManager.tiles)
            //    {
            //        tile.GetComponentInChildren<Tile>().SetTileMaterial(Tile.TileMaterial.Arrange);
            //    }
            //}
        }
        //else
        //{
        //    settingMode = true;
        //    playerCtrl.joystick.SetActive(true);
        //    var joystickController = playerCtrl.joystick.GetComponentInChildren<ArrangeJoystickHandler>();
        //    joystickController.SetFirstArranger(playerCtrl.icon);
        //    joystickController.SetPositionToCurrentPlayer(playerCtrl.transform);

        //    for (int i = 0; i < playerCtrl.joystick.transform.childCount; ++i)
        //    {
        //        if (string.Equals(playerCtrl.joystick.transform.GetChild(i).gameObject.tag,"Handler"))
        //        {
        //            playerCtrl.joystick.transform.GetChild(i).gameObject.SetActive(false);
        //        }
        //        else if(i == playerCtrl.joystick.transform.childCount - 1)
        //        {
        //            playerCtrl.joystick.transform.GetChild(i).gameObject.SetActive(true);
        //        }
        //    }
        //}

        Debug.Log($"arrange enter, arragable tiles {playerCtrl.arrangableTiles.Count}, attackable tiles {playerCtrl.attakableTiles.Count}");
    }

    public override void Exit()
    {
        Debug.Log("arrange exit");
        Time.timeScale = 1f;
        //settingMode = false;
        playerCtrl.stageManager.currentPlayer = null;
    }

    public override void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!playerCtrl.stateManager.firstArranged)
        {
            if(Input.GetMouseButton(0))
            {
                int backgroundMask = 1 << LayerMask.NameToLayer("Background");
                int lowTileMask = 1 << LayerMask.NameToLayer("LowTile");
                int highTileMask = 1 << LayerMask.NameToLayer("HighTile");
                int layerMask = backgroundMask | lowTileMask | highTileMask;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    var pos = hit.point;
                    var tile = hit.transform.GetComponentInChildren<Tile>();
                    if (tile.arrangePossible && playerCtrl.arrangableTiles.Contains(tile))
                    {
                        pos = hit.transform.parent.position;
                        pos.y = hit.transform.GetComponentInChildren<Tile>().height;
                    }
                    playerCtrl.transform.position = pos;
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                var tile = hit.transform.GetComponentInChildren<Tile>();
                if (hit.transform != null && tile.arrangePossible && playerCtrl.arrangableTiles.Contains(tile))
                {
                    Debug.Log("배치가능");
                    playerCtrl.currentTile = tile;
                    playerCtrl.stateManager.firstArranged = true;
                }
                else
                {
                    Debug.Log($"배치불가능: {hit}");
                    //playerCtrl.SetState(PlayerController.CharacterStates.Idle);
                    playerCtrl.ReturnPool.Invoke();
                }

                foreach (var arrnageTile in playerCtrl.arrangableTiles)
                {
                    arrnageTile.ClearTileMesh();
                }
            }
        }
    }

    public void ArrangableTileSet(Defines.Occupation occupation)
    {
        string tag;
        switch (occupation)
        {
            case Defines.Occupation.Guardian:
            case Defines.Occupation.Striker:
                tag = "LowTile";
                break;
            default:
                tag = "HighTile";
                break;
        }

        var tileParent = GameObject.FindGameObjectWithTag(tag);
        var tileCount = tileParent.transform.childCount;
        var tiles = new List<Tile>();
        for (int i = 0; i < tileCount; ++i)
        {
            if (tileParent.transform.GetChild(i).GetComponentInChildren<Tile>().arrangePossible)
            {
                tiles.Add(tileParent.transform.GetChild(i).GetComponentInChildren<Tile>());
            }
        }
        playerCtrl.arrangableTiles = tiles;
    }

    public void AttackableTileSet(Defines.Occupation occupation)
    {
        int layerMask = 0;
        int lowTileMask = 1 << LayerMask.NameToLayer("LowTile");
        int highTileMask = 1 << LayerMask.NameToLayer("HighTile");

        switch (occupation)
        {
            case Defines.Occupation.Guardian:
            case Defines.Occupation.Striker:
                layerMask = lowTileMask;
                break;
            default:
                layerMask = lowTileMask | highTileMask;
                break;
        }

        Vector3 characterPosition = playerCtrl.transform.position;
        Vector3 forward = -playerCtrl.transform.forward;
        Vector3 right = playerCtrl.transform.right;
        int characterRow = 0;
        int characterCol = 0;

        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 2)
                {
                    characterRow = i;
                    characterCol = j;
                }
            }
        }

        if (playerCtrl.attakableTiles.Count > 0)
        {
            playerCtrl.attakableTiles.Clear();
        }

        for (int i = 0; i < playerCtrl.state.AttackRange.GetLength(0); i++)
        {
            for (int j = 0; j < playerCtrl.state.AttackRange.GetLength(1); j++)
            {
                if (playerCtrl.state.AttackRange[i, j] == 1)
                {
                    Vector3 relativePosition = (i - characterRow) * forward + (j - characterCol) * right;
                    Vector3 tilePosition = characterPosition + relativePosition;
                    var tilePosInt = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z);

                    RaycastHit hit;
                    var tempPos = new Vector3(tilePosInt.x, tilePosInt.y - 10f, tilePosInt.z);
                    if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
                    {
                        var tileContoller = hit.transform.GetComponent<Tile>();
                        playerCtrl.attakableTiles.Add(tileContoller);
                    }
                }
            }
        }
    }

    public void RotatePlayer(Defines.RotationDirection direction)
    {
        switch ((int)direction)
        {
            case (int)Defines.RotationDirection.Up:
                playerCtrl.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case (int)Defines.RotationDirection.Right:
                playerCtrl.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case (int)Defines.RotationDirection.Down:
                playerCtrl.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case (int)Defines.RotationDirection.Left:
                playerCtrl.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                break;
        }
    }
}