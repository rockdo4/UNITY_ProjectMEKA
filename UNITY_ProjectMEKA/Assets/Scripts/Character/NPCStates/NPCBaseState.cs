using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPCBaseState 
{
    protected EnemyController enemyCtrl;
    // 11.22, 김민지, 네브메쉬 제거하고 이동 방식 변경
    //protected NavMeshAgent agent;
    protected Transform[] players;
    protected Transform[] wayPoint;

    abstract public void Enter();

    // 11.22, 김민지, 이동시 필요
    abstract public void FixedUpdate();

    abstract public void Update();
    abstract public void Exit();

    public NPCBaseState(EnemyController enemy)
    {
        this.enemyCtrl = enemy;

        // 11.22, 김민지, 이동 기능 변경

        //agent = enemyCtrl.GetComponent<NavMeshAgent>();

        wayPoint = enemyCtrl.wayPoint;

    }
}
