using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class PlayableArrangeState : PlayableBaseState
{
    private RaycastHit hit;
    public Button collecteButton;

    public PlayableArrangeState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        Time.timeScale = 0.2f;

        // Set arrange tile mesh
        if (!playerCtrl.stateManager.firstArranged)
        {
            if(playerCtrl.stateManager.tiles != null)
            {
                foreach (var tile in playerCtrl.stateManager.tiles)
                {
                    tile.GetComponentInChildren<Tile>().SetTileMaterial(true, Tile.TileMaterial.Arrange);
                }
            }
        }
        else
        {
            playerCtrl.joystick.SetActive(true);
            var joystickController = playerCtrl.joystick.GetComponentInChildren<ArrangeJoystick>();
            joystickController.SetPlayer(playerCtrl.transform);
            joystickController.SetFirstArranger(playerCtrl.icon);
            var joystickPos = playerCtrl.transform.position;
            joystickPos.y += joystickController.yOffset;
            joystickController.transform.parent.position = joystickPos;

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
    }

    public override void Update()
    {
        if(!playerCtrl.stateManager.firstArranged)
        {
            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                int backgroundMask = 1 << LayerMask.NameToLayer("Background");
                int lowTileMask = 1 << LayerMask.NameToLayer("LowTile");
                int highTileMask = 1 << LayerMask.NameToLayer("HighTile");
                int layerMask = backgroundMask | lowTileMask | highTileMask;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    var pos = hit.point;
                    if (hit.transform.GetComponentInChildren<Tile>().arrangePossible)
                    {
                        pos = hit.transform.position;
                        pos.y = hit.transform.GetComponentInChildren<Tile>().height;
                    }
                    playerCtrl.transform.position = pos;
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                if (hit.transform != null && hit.transform.GetComponent<Tile>().arrangePossible)
                {
                    Debug.Log("배치가능");
                    hit.transform.GetComponentInChildren<Tile>().arrangePossible = false;
                    playerCtrl.stateManager.firstArranged = true;
                }
                else
                {
                    Debug.Log($"배치불가능: {hit}");
                    playerCtrl.ReturnPool.Invoke();
                }

                foreach (var tile in playerCtrl.stateManager.tiles)
                {
                    tile.GetComponentInChildren<Tile>().SetTileMaterial(false, Tile.TileMaterial.None);
                }
            }
        }
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
