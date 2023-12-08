using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Defines;


public class PlayableArrangeState : PlayableBaseState
{
    private RaycastHit hit;
    private Tile hitTile;
    private bool isOnPossibleTile;

    public PlayableArrangeState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        isOnPossibleTile = false;
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        // Set arrange tile mesh
        if (!playerCtrl.stateManager.firstArranged)
        {
            playerCtrl.transform.forward = Vector3.forward;
        }

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
                int backgroundMask = 1 << LayerMask.NameToLayer(Layers.background);
                int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
                int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);
                int layerMask = backgroundMask | lowTileMask | highTileMask;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    if (!playerCtrl.stageManager.characterInfoUIManager.currentPlayerOnTile)
                    {
                        playerCtrl.stageManager.characterInfoUIManager.currentPlayerOnTile = true;
                        isOnPossibleTile = false;
                    }

                    var pos = hit.point;
                    hitTile = hit.transform.GetComponentInChildren<Tile>();
                    if (hitTile.arrangePossible && playerCtrl.arrangableTiles.Contains(hitTile))
                    {
                        pos = hit.transform.parent.position;
                        pos.y = hit.transform.GetComponentInChildren<Tile>().height;
                        isOnPossibleTile = true;
                    }
                    else
                    {
                        isOnPossibleTile = false;
                    }
                    playerCtrl.transform.position = pos;
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                var firstCondition = hit.transform != null && hitTile.arrangePossible && playerCtrl.arrangableTiles.Contains(hitTile);
                var secondCondition = playerCtrl.stageManager.currentCost >= playerCtrl.state.arrangeCost;

                if (firstCondition && secondCondition)
                {
                    playerCtrl.currentTile = hitTile;
                    playerCtrl.stateManager.firstArranged = true;
                }
                else if(!isOnPossibleTile && playerCtrl.stageManager.characterInfoUIManager.currentPlayerOnTile)
                {
                    Debug.Log("배치 불가 타일");
                    playerCtrl.stageManager.currentPlayer = null;
                    playerCtrl.PlayerInit.Invoke();
                }
                else
                {
                    playerCtrl.PlayerInit.Invoke();
                }

                // 배치불가능 타일 위에서 뗐을 때 조건 하나 더 만들기
                
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