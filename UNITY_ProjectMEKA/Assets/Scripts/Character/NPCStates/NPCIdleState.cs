using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    private Vector3 targetPos;
    private float rotSpeed = 7f;
    private bool isRotating = true;

    public NPCIdleState(EnemyController enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemyCtrl.ani.SetTrigger("Idle");
        targetPos = enemyCtrl.target.transform.position;
        targetPos.y = enemyCtrl.rb.transform.position.y;
        isRotating = true;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        
        if(isRotating)
        {
            Rotate();
        }
        else
        {
            enemyCtrl.SetState(NPCStates.Attack);
        }
    }

    public void Rotate()
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
}
