using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayableProjectileAttackState : PlayableBaseState
{
    private float timer;
    public PlayableProjectileAttackState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        timer = 0;
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if(timer > playerCtrl.state.attackDelay)
        {
            timer = 0;
            playerCtrl.Fire();
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
        if (playerCtrl.target == null)
        {
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
    }
}
