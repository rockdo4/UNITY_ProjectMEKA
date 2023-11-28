using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayableProjectileAttackState : PlayableBaseState
{
    private float timer;
    GameObject[] enemys;
    private int count;
    public PlayableProjectileAttackState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        //timer = 0;
    }

    public override void Update()
    {
        //CheckEnemy();
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = playerCtrl.state.attackDelay;
            //timer = 0;
            playerCtrl.ani.SetTrigger("Attack");
            Debug.Log("ATTACKENEMY");
            //CheckEnemy();
            //playerCtrl.Fire();
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
        if (playerCtrl.target == null)
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }

        
        
    }
    
}
