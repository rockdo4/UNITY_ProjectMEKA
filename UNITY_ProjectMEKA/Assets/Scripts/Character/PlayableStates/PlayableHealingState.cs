using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableHealingState : PlayableBaseState
{
    float timer;
    public PlayableHealingState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        timer = 0;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if(timer > playerCtrl.state.attackDelay)
        {
            timer = 0;
            playerCtrl.Healing();
            playerCtrl.SetState(PlayerController.CharacterStates.Idle);
        }
    }
}
