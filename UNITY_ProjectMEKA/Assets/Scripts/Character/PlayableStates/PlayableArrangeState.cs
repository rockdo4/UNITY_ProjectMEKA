using System.Collections.Generic;
using UnityEngine;
using static Defines;


public class PlayableArrangeState : PlayableBaseState
{
    private RaycastHit hit;
    private Tile hitTile;
    //public Button collecteButton;
    //public bool settingMode;

    public PlayableArrangeState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        // Set arrange tile mesh
        if (!playerCtrl.stateManager.firstArranged)
        {
            playerCtrl.transform.forward = Vector3.forward;
            //var occupation = playerCtrl.state.occupation;
            //playerCtrl.ArrangableTileSet(occupation);
            //playerCtrl.AttackableTileSet(occupation);
            //playerCtrl.characterInfoUIManager.windowMode = Defines.CharacterInfoMode.FirstArrange;
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
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
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
                    hitTile = hit.transform.GetComponentInChildren<Tile>();
                    if (hitTile.arrangePossible && playerCtrl.arrangableTiles.Contains(hitTile))
                    {
                        pos = hit.transform.parent.position;
                        pos.y = hit.transform.GetComponentInChildren<Tile>().height;
                    }
                    playerCtrl.transform.position = pos;
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                if (hit.transform != null && hitTile.arrangePossible && playerCtrl.arrangableTiles.Contains(hitTile))
                {
                    playerCtrl.currentTile = hitTile;
                    playerCtrl.stateManager.firstArranged = true;
                }
                else
                {
                    playerCtrl.ReturnPool.Invoke();
                }

                //foreach (var arrnageTile in playerCtrl.arrangableTiles)
                //{
                //    arrnageTile.ClearTileMesh();
                //}
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