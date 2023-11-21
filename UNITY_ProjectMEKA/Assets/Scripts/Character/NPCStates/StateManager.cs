using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager 
{
    private NPCBaseState currentNPCBase;
    public void ChangeState(NPCBaseState newState)
    {
        if (currentNPCBase != null)
        {
            currentNPCBase.Exit();
        }
        currentNPCBase = newState;
        currentNPCBase.Enter();
    }

    public void Update()
    {
        if (currentNPCBase != null)
        {
            currentNPCBase.Update();
        }
    }
}
