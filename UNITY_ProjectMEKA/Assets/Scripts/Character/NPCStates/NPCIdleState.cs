using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    public NPCIdleState(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        // 11.22, �����, �̵���� ����
        //agent.isStopped = true;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}
