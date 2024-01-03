using System.Collections.Generic;
using UnityEngine;
using static Defines;


public class PlayableArrangeState : PlayableBaseState
{
    private RaycastHit hit;
    private Tile hitTile;
    private bool isOnPossibleTile;
    private StageManager stageManager;
    public Vector3Int prevGridPos;

    public PlayableArrangeState(PlayerController player) : base(player)
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
    }

    public override void Enter()
    {
        isOnPossibleTile = false;
        Time.timeScale = stageManager.CurrentSpeed * 0.2f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        // Set arrange tile mesh
        if (!playerCtrl.stateManager.firstArranged)
        {
            playerCtrl.transform.forward = Vector3.right;
        }

        Debug.Log($"arrange enter, arragable tiles {playerCtrl.arrangableTiles.Count}, attackable tiles {playerCtrl.attackableTiles.Count}");
    }

    public override void Exit()
    {
        Debug.Log("arrange exit");
        Time.timeScale = stageManager.CurrentSpeed;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        playerCtrl.firstLookPos = playerCtrl.transform;
        SoundManager.instance.PlayerSFXAudio("CharacterPut");

        
    }

    public override void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        playerCtrl.CurrentGridPos = Utils.Vector3ToVector3Int(playerCtrl.transform.position);

        if (!playerCtrl.stateManager.firstArranged)
        {
            if(Input.GetMouseButton(0))
            {
                int backgroundMask = 1 << LayerMask.NameToLayer(Layers.background);
                int lowTileMask = 1 << LayerMask.NameToLayer(Layers.lowTile);
                int highTileMask = 1 << LayerMask.NameToLayer(Layers.highTile);
                int layerMask = backgroundMask | lowTileMask | highTileMask;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    hitTile = hit.transform.GetComponentInChildren<Tile>();
                    var isCurrentPlayerOnTile = playerCtrl.stageManager.ingameStageUIManager.currentPlayerOnTile;
                    var isTileObstacle = hitTile.tileType == TileType.Obstacle;
                    if (!isCurrentPlayerOnTile && !isTileObstacle)
                    {
                        playerCtrl.stageManager.ingameStageUIManager.currentPlayerOnTile = true;
                        isOnPossibleTile = false;
                    }

                    var pos = hit.point;
                    if (hitTile.arrangePossible && playerCtrl.arrangableTiles.Contains(hitTile))
                    {
                        pos = hit.transform.parent.position;
                        pos.y = hit.transform.GetComponentInChildren<Tile>().height;
                        isOnPossibleTile = true;

                        if (prevGridPos != playerCtrl.CurrentGridPos)
                        {
                            Debug.Log("어택타일셋");
                            stageManager.currentPlayer.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                            stageManager.ingameStageUIManager.ChangeAttackableTileMesh(true);
                            prevGridPos = playerCtrl.CurrentGridPos;
                        }
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
                else if(!isOnPossibleTile && playerCtrl.stageManager.ingameStageUIManager.currentPlayerOnTile)
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