using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager 
{
    public NPCBaseState currentNPCBase;
    public void ChangeState(NPCBaseState newState)
    {
        if (currentNPCBase != null)
        {
            currentNPCBase.Exit();
        }
        currentNPCBase = newState;
        currentNPCBase.Enter();
    }

    // 11.22, 김민지, 이동구현시 필요
    public void FixedUpdate()
    {
        if (currentNPCBase != null)
        {
            currentNPCBase.FixedUpdate();
        }
    }

    public void Update()
    {
        if (currentNPCBase != null)
        {
            currentNPCBase.Update();
        }
    }
}
