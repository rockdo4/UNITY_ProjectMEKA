using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPCBaseState 
{
    protected EnemyController enemyCtrl;
    // 11.22, �����, �׺�޽� �����ϰ� �̵� ��� ����
    //protected NavMeshAgent agent;
    protected Transform[] players;
    protected Transform[] wayPoint;

    abstract public void Enter();

    // 11.22, �����, �̵��� �ʿ�
    abstract public void FixedUpdate();

    abstract public void Update();
    abstract public void Exit();

    public NPCBaseState(EnemyController enemy)
    {
        this.enemyCtrl = enemy;

        // 11.22, �����, �̵� ��� ����

        //agent = enemyCtrl.GetComponent<NavMeshAgent>();

        wayPoint = enemyCtrl.wayPoint;

    }
}
