using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableArrangeState : PlayableBaseState
{
    private bool secondArranged;

    public PlayableArrangeState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        Time.timeScale = 0.2f;
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
    }

    public override void Update()
    {
    }

    public void SetDirection()
    {

    }
}
