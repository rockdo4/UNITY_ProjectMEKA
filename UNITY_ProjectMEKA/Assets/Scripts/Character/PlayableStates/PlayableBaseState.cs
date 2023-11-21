using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayableBaseState
{
    protected PlayerController playerCtrl;
    protected Transform[] enemy;

    abstract public void Enter();
    abstract public void Update();
    abstract public void Exit();

    public PlayableBaseState(PlayerController player)
    {
        playerCtrl = player;


    }
}
