using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPCBaseState 
{
    protected EnemyController enemyCtrl;
    //protected NavMeshAgent agent;
    protected Transform[] players;
    protected Transform[] wayPoint;

    abstract public void Enter();
    abstract public void Update();
    abstract public void Exit();

    public NPCBaseState(EnemyController enemy)
    {
        this.enemyCtrl = enemy;
        //agent = enemyCtrl.GetComponent<NavMeshAgent>();

        wayPoint = enemyCtrl.wayPoint;

    }
}
