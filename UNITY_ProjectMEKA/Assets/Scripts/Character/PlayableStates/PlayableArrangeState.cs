using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class PlayableArrangeState : PlayableBaseState
{
    private RaycastHit hit;
    public Button collecteButton;
    public bool settingMode;

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
            switch(occupation)
            {
                case Defines.Occupation.Guardian:
                case Defines.Occupation.Striker:
                    playerCtrl.stateManager.tiles = TileSet("LowTile");
                    break;
                default:
                    playerCtrl.stateManager.tiles = TileSet("HighTile");
                    break;
            }

            if (playerCtrl.stateManager.tiles != null)
            {
                foreach (var tile in playerCtrl.stateManager.tiles)
                {
                    tile.GetComponentInChildren<Tile>().SetTileMaterial(Tile.TileMaterial.Arrange);
                }
            }
        }
        else
        {
            settingMode = true;
            Debug.Log(playerCtrl.joystick == null);
            playerCtrl.joystick.SetActive(true);
            var joystickController = playerCtrl.joystick.GetComponentInChildren<ArrangeJoystick>();
            joystickController.SetPlayer(playerCtrl.transform);
            joystickController.SetFirstArranger(playerCtrl.icon);
            joystickController.SetPositionToCurrentPlayer(playerCtrl.transform);

            for (int i = 0; i < playerCtrl.joystick.transform.childCount; ++i)
            {
                if (string.Equals(playerCtrl.joystick.transform.GetChild(i).gameObject.tag,"Handler"))
                {
                    playerCtrl.joystick.transform.GetChild(i).gameObject.SetActive(false);
                }
                else if(i == playerCtrl.joystick.transform.childCount - 1)
                {
                    playerCtrl.joystick.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        Debug.Log($"arrange enter, tiles {playerCtrl.stateManager.tiles.Count}");
    }

    public override void Exit()
    {
        Debug.Log("arrange exit");
        Time.timeScale = 1f;
        settingMode = false;
        playerCtrl.stageManager.currentPlayer = null;
    }

    public override void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!settingMode && !playerCtrl.stateManager.firstArranged)
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
                    if (hit.transform.GetComponentInChildren<Tile>().arrangePossible && playerCtrl.stateManager.tiles.Contains(hit.transform.parent.gameObject))
                    {
                        pos = hit.transform.parent.position;
                        pos.y = hit.transform.GetComponentInChildren<Tile>().height;
                    }
                    playerCtrl.transform.position = pos;
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                if (hit.transform != null && hit.transform.GetComponent<Tile>().arrangePossible && playerCtrl.stateManager.tiles.Contains(hit.transform.parent.gameObject))
                {
                    Debug.Log("배치가능");
                    //hit.transform.GetComponentInChildren<Tile>().arrangePossible = false;
                    playerCtrl.currentTile = hit.transform.GetComponent<Tile>();
                    playerCtrl.stateManager.firstArranged = true;
                }
                else
                {
                    Debug.Log($"배치불가능: {hit}");
                    playerCtrl.SetState(PlayerController.CharacterStates.Idle);
                    playerCtrl.ReturnPool.Invoke();
                }

                foreach (var tile in playerCtrl.stateManager.tiles)
                {
                    tile.GetComponentInChildren<Tile>().ClearTileMesh();
                }
            }
        }
    }

    public List<GameObject> TileSet(string tag)
    {
        var tileParent = GameObject.FindGameObjectWithTag(tag);
        var tileCount = tileParent.transform.childCount;
        var tiles = new List<GameObject>();
        for (int i = 0; i < tileCount; ++i)
        {
            if (tileParent.transform.GetChild(i).GetComponentInChildren<Tile>().arrangePossible)
            {
                tiles.Add(tileParent.transform.GetChild(i).gameObject);
            }
        }
        return tiles;
    }

    public void UpdateAttackPositions()
    {
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

        if (playerCtrl.attakableTilePositions.Count > 0)
        {
            playerCtrl.attakableTilePositions.Clear();
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

                    playerCtrl.attakableTilePositions.Add(tilePosInt);
                }
            }
        }
        var playerPosInt = new Vector3(characterPosition.x, characterPosition.y, characterPosition.z);
        playerCtrl.attakableTilePositions.Add(playerPosInt);
    }
}
