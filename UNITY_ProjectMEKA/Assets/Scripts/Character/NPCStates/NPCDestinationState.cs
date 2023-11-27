using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCDestinationStates : NPCBaseState
{

    private Vector3 targetPos;
    private float threshold = 0.1f;
    private float speed;
    private int repeatCount = -1;
    private Vector3 direction;

    private bool once = false;

    GameObject[] players;
    public NPCDestinationStates(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemyCtrl.transform.position = enemyCtrl.initPos;
        targetPos = enemyCtrl.wayPoint[enemyCtrl.waypointIndex].position;
        targetPos.y = enemyCtrl.transform.position.y;
        direction = (targetPos - enemyCtrl.transform.position).normalized;
        enemyCtrl.transform.LookAt(targetPos);
        speed = enemyCtrl.state.speed;
        repeatCount = -1;
        once = false;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        switch(enemyCtrl.moveType)
        {
            case Defines.MoveType.AutoTile:
                break;
            case Defines.MoveType.Waypoint:
                MoveEnemyWaypoint();
                break;
            case Defines.MoveType.Straight:
                MoveEnemyStraight();
                break;
            case Defines.MoveType.WaypointRepeat:
                MoveEnemyRepeat(enemyCtrl.moveRepeatCount);
                break;
        }

        CheckEnemy();
    }
    void CheckEnemy()
    {
        Vector3 centerPos = new Vector3(enemyCtrl.CurrentGridPos.x + 0.5f, enemyCtrl.CurrentGridPos.y, enemyCtrl.CurrentGridPos.z + 0.5f);
        centerPos.y += enemyCtrl.CurrentPos.y;
        
        if(Vector3.Distance(enemyCtrl.transform.position, centerPos) > 0.1f)
        {
            return;
        }

        players = GameObject.FindGameObjectsWithTag("Player");

        Vector3Int playerGridPos = enemyCtrl.CurrentGridPos;


        int tileRange = Mathf.FloorToInt(enemyCtrl.state.range); 

        for (int i = 1; i <= tileRange; i++)
        {
            Vector3Int forwardGridPos = playerGridPos + Vector3Int.RoundToInt(enemyCtrl.transform.forward) * i;
            Vector3 tileCenterWorldPos = new Vector3(forwardGridPos.x + 0.5f, forwardGridPos.y + 0.5f, forwardGridPos.z + 0.5f);
            foreach (GameObject pl in players)
            {
                PlayerController player = pl.GetComponent<PlayerController>();

                if (player != null)
                {
                    Vector3Int enemyGridPos = player.CurrentGridPos;
                    //Vector3 playerPos = player.transform.position;
                    float distanceToTileCenter = Vector3.Distance(enemyGridPos, tileCenterWorldPos);

                    if (enemyGridPos == forwardGridPos/*distanceToTileCenter <= 0.1f*/)
                    {
                        enemyCtrl.target = pl;
                        if (player.blockCount < player.maxBlockCount)
                        {
                            enemyCtrl.SetState(NPCStates.Attack);
                        }
                        return;
                    }
                }
            }
        }

    }

    public override void Update()
    {

    }

    public void MoveEnemyWaypoint()
    {
        var pos = enemyCtrl.rb.position;
        pos += direction * speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(new Vector3(pos.x,pos.z), new Vector3(targetPos.x,targetPos.z)) < threshold) // 다음 웨이포인트 도착하면
        {
            if (enemyCtrl.waypointIndex >= enemyCtrl.wayPoint.Length - 1)
            {
                //enemyCtrl.waypointIndex = 0;
                //enemyCtrl.transform.position = enemyCtrl.initPos;
                enemyCtrl.GetComponentInParent<PoolAble>().ReleaseObject();
            }
            else
            {
                enemyCtrl.waypointIndex++;
                targetPos = enemyCtrl.wayPoint[enemyCtrl.waypointIndex].position;
                targetPos.y = enemyCtrl.transform.position.y;
                direction = (targetPos - enemyCtrl.transform.position).normalized;
                enemyCtrl.transform.LookAt(targetPos);            
            }
        }
    }

    public void MoveEnemyStraight()
    {
        if(!once)
        {
            targetPos = enemyCtrl.wayPoint[enemyCtrl.wayPoint.Length-1].position;
            targetPos.y = enemyCtrl.transform.position.y;
            enemyCtrl.transform.LookAt(targetPos);
            once = true;
        }

        var pos = enemyCtrl.rb.position;
        pos += enemyCtrl.transform.forward * speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold) // 다음 웨이포인트 도착하면
        {
            enemyCtrl.GetComponentInParent<PoolAble>().ReleaseObject();
        }
    }

    public void MoveEnemyRepeat(int count)
    {
        var pos = enemyCtrl.rb.position;
        pos += enemyCtrl.transform.forward * speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold) // 다음 웨이포인트 도착하면
        {
            if (enemyCtrl.waypointIndex == enemyCtrl.wayPoint.Length - 2) // 마지막-1 웨이포인트 도착하면
            {
                enemyCtrl.waypointIndex = -1;
            }
            else if(enemyCtrl.waypointIndex == 0) // 한바퀴 돌면
            {
                repeatCount++;
                if (repeatCount == count)
                {
                    // 마지막 웨이포인트 할당
                    enemyCtrl.waypointIndex = enemyCtrl.wayPoint.Length - 2;
                }
            }
            else if(enemyCtrl.waypointIndex == enemyCtrl.wayPoint.Length - 1)
            {
                enemyCtrl.GetComponentInParent<PoolAble>().ReleaseObject();
                return;
            }
            enemyCtrl.waypointIndex++;
            targetPos = enemyCtrl.wayPoint[enemyCtrl.waypointIndex].position;
            targetPos.y = enemyCtrl.transform.position.y;
            enemyCtrl.transform.LookAt(targetPos);
        }
    }
}
