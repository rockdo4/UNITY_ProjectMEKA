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

    // 11.22, �����, �̵������� �ʿ�
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
