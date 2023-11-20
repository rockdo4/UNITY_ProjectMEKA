using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableStateManager 
{
    private PlayableBaseState currentBase;
    public void ChangeState(PlayableBaseState newState)
    {
        if (currentBase != null)
        {
            currentBase.Exit();
        }
        currentBase = newState;
        currentBase.Enter();
    }

    public void Update()
    {
        if (currentBase != null)
        {
            currentBase.Update();
        }
    }
}
