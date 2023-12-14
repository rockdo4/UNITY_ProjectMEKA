using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class NPCDestinationStates : NPCBaseState
{
    private float rotSpeed = 7f;
    private bool isRotating;
    private Vector3 targetPos;
    private float threshold = 0.1f;
    //private float speed;
    private int repeatCount = -1;
    private Vector3 direction;

    private bool isOne = false;
    private float timer;
    GameObject[] players;
    private float distance;
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

        distance = Random.Range(0.3f,0.5f);
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

        if(enemyCtrl.state.isFly)
        {
            timer += Time.deltaTime;
            if(timer > /*enemyCtrl.state.attackDelay*/2f)
            {
                timer = 0;
                foreach (var pl in enemyCtrl.rangeInPlayers)
                {
                    if (pl.GetComponentInParent<PlayerController>().state.occupation == Defines.Occupation.Castor ||
                        pl.GetComponentInParent<PlayerController>().state.occupation == Defines.Occupation.Hunter ||
                        pl.GetComponentInParent<PlayerController>().state.occupation == Defines.Occupation.Supporters)
                    {
                        //if(enemyCtrl.forwardGrid == pl.GetComponentInParent<PlayerController>().CurrentGridPos)
                        //{
                        //    enemyCtrl.target = pl;
                        //    enemyCtrl.SetState(NPCStates.Idle);
                        //    return;
                        //}
                        enemyCtrl.target = pl;
                        enemyCtrl.SetState(NPCStates.Idle);
                        return;
                    }

                }
            }
            
        }
        //원래는 0.3
        if (Vector3.Distance(enemyCtrl.transform.position, tileCenter) < 0.3f && !enemyCtrl.state.isFly)
        {
            CheckPlayer();
        }
    }
    void CheckPlayer()
    {
       
        foreach (var pl in enemyCtrl.rangeInPlayers)
        {
            PlayerController player = pl.GetComponentInParent<PlayerController>();
            //float distance = Vector3.Distance(enemyCtrl.transform.position,player.transform.position);/*&& distance > 0.4f*/
            if (player.blockCount < player.maxBlockCount && 
                enemyCtrl.state.isBlock && player != null &&
                player.currentState != PlayerController.CharacterStates.Arrange)
            {
                if(enemyCtrl.state.isFly)
                {
                    if(pl.GetComponentInParent<PlayerController>().state.occupation == Defines.Occupation.Castor||
                        pl.GetComponentInParent<PlayerController>().state.occupation == Defines.Occupation.Hunter ||
                        pl.GetComponentInParent<PlayerController>().state.occupation == Defines.Occupation.Supporters)
                    {
                        //if (enemyCtrl.forwardGrid == pl.GetComponentInParent<PlayerController>().CurrentGridPos)
                        //{
                        //    enemyCtrl.target = pl;
                        //    enemyCtrl.SetState(NPCStates.Idle);
                        //    return;
                        //}
                        enemyCtrl.target = pl;
                        enemyCtrl.SetState(NPCStates.Idle);
                        return;

                    }
                }
                else
                {
                    if (enemyCtrl.forwardGrid == pl.GetComponentInParent<PlayerController>().CurrentGridPos)
                    {
                        enemyCtrl.target = pl;
                        //enemyCtrl.SetState(NPCStates.Attack);
                        enemyCtrl.SetState(NPCStates.Idle);
                        return;
                    }
                        
                }
               
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
        if (isRotating)
        {
            var targetRotation = Quaternion.LookRotation(targetPos - enemyCtrl.rb.position);
            enemyCtrl.rb.rotation = Quaternion.Slerp(enemyCtrl.rb.rotation, targetRotation, rotSpeed * Time.deltaTime);

            float angleDifference = Quaternion.Angle(enemyCtrl.rb.rotation, targetRotation); // 현재 각도와 목표 각도 사이의 차이

            if (angleDifference < 1f)
            {
                enemyCtrl.transform.LookAt(targetPos);
                isRotating = false;
            }
        }

        var pos = enemyCtrl.rb.position;
        if(!isRotating)
        {
            pos += direction * enemyCtrl.state.speed * Time.deltaTime;
            enemyCtrl.rb.MovePosition(pos);
        }

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
                isRotating = true;

                enemyCtrl.waypointIndex++;
                targetPos = enemyCtrl.wayPoint[enemyCtrl.waypointIndex].position;
                targetPos.y = enemyCtrl.transform.position.y;
                direction = (targetPos - enemyCtrl.transform.position).normalized;
            }
        }
    }

    public void MoveEnemyStraight()
    {
        var targetPosAppliedY = enemyCtrl.wayPoint[enemyCtrl.wayPoint.Length - 1].position;
        targetPosAppliedY.y = enemyCtrl.transform.position.y;
        if (targetPos != targetPosAppliedY)
        {
            targetPos = enemyCtrl.wayPoint[enemyCtrl.wayPoint.Length-1].position;
            targetPos.y = enemyCtrl.transform.position.y;
            enemyCtrl.transform.LookAt(targetPos);
            direction = (targetPos - enemyCtrl.transform.position).normalized;
        }

        var pos = enemyCtrl.rb.position;
        pos += direction * enemyCtrl.state.speed * Time.deltaTime;
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

        if (isRotating)
        {
            var targetRotation = Quaternion.LookRotation(targetPos - enemyCtrl.rb.position);
            enemyCtrl.rb.rotation = Quaternion.Slerp(enemyCtrl.rb.rotation, targetRotation, rotSpeed * Time.deltaTime);

            float angleDifference = Quaternion.Angle(enemyCtrl.rb.rotation, targetRotation); // 현재 각도와 목표 각도 사이의 차이

            if (angleDifference < 1f)
            {
                enemyCtrl.transform.LookAt(targetPos);
                isRotating = false;
            }
        }
        else
        {
            pos += direction * enemyCtrl.state.speed * Time.deltaTime;
            enemyCtrl.rb.MovePosition(pos);
        }        

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
            direction = (targetPos - enemyCtrl.transform.position).normalized;
            isRotating = true;
        }
    }
}
