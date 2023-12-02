using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class NPCDestinationStates : NPCBaseState
{

    private Vector3 targetPos;
    private float threshold = 0.1f;
    //private float speed;
    private int repeatCount = -1;
    private Vector3 direction;

    private bool once = false;
    private bool isOne = false;
    GameObject[] players;
    public NPCDestinationStates(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        //enemyCtrl.transform.position = enemyCtrl.initPos;
        //targetPos = enemyCtrl.wayPoint[enemyCtrl.waypointIndex].position;
        //targetPos.y = enemyCtrl.transform.position.y;
        //direction = (targetPos - enemyCtrl.transform.position).normalized;
        //enemyCtrl.transform.LookAt(targetPos);
        ////speed = enemyCtrl.state.speed;
        //repeatCount = -1;
        //once = false;
    }
    public void Init()
    {
        // 다른 상태 -> 이 상태로 들어왔을 때 포지션이 이전 포지션과 동일해야 함
        enemyCtrl.transform.position = enemyCtrl.initPos;
        targetPos = enemyCtrl.wayPoint[enemyCtrl.waypointIndex].position;
        targetPos.y = enemyCtrl.transform.position.y;
        direction = (targetPos - enemyCtrl.transform.position).normalized;
        enemyCtrl.transform.LookAt(targetPos);
        //speed = enemyCtrl.state.speed;
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

        Vector3Int gridPosition = Vector3Int.FloorToInt(enemyCtrl.transform.position);
        Vector3 tileCenter = new Vector3(gridPosition.x + 0.5f, gridPosition.y, gridPosition.z + 0.5f);

        if (Vector3.Distance(enemyCtrl.transform.position, tileCenter) < 0.1f)
        {
            CheckPlayer();
        }
    }
    void CheckPlayer()
    {
       
        foreach (var pl in enemyCtrl.rangeInPlayers)
        {
            
            PlayerController player = pl.GetComponentInParent<PlayerController>();
            //float distance = Vector3.Distance(enemyCtrl.transform.position,player.transform.position);
            if (player.blockCount < player.maxBlockCount && enemyCtrl.state.isBlock && player != null /*&& distance >0.4f*/)
            {
                enemyCtrl.target = pl;
                enemyCtrl.SetState(NPCStates.Attack);
                return;
            }
            
        }

    }
    
    public override void Update()
    {
        if(!isOne)
        {
            Init();
            isOne = true;
        }
    }

    public void MoveEnemyWaypoint()
    {
        var pos = enemyCtrl.rb.position;
        pos += direction * enemyCtrl.state.speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(new Vector3(pos.x,pos.z), new Vector3(targetPos.x,targetPos.z)) < threshold) // 다음 웨이포인트 도착하면
        {
            if (enemyCtrl.waypointIndex >= enemyCtrl.wayPoint.Length - 1)
            {
                //enemyCtrl.waypointIndex = 0;
                //enemyCtrl.transform.position = enemyCtrl.initPos;
                enemyCtrl.isArrival = true;
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
        enemyCtrl.transform.LookAt(targetPos);
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
        pos += enemyCtrl.transform.forward * enemyCtrl.state.speed * Time.deltaTime;
        enemyCtrl.rb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold) // 다음 웨이포인트 도착하면
        {
            enemyCtrl.isArrival = true;
            enemyCtrl.GetComponentInParent<PoolAble>().ReleaseObject();
        }
    }

    public void MoveEnemyRepeat(int count)
    {
        var pos = enemyCtrl.rb.position;
        pos += enemyCtrl.transform.forward * enemyCtrl.state.speed * Time.deltaTime;
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
                enemyCtrl.isArrival = true;
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
