using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableStateManager 
{
    public PlayableBaseState currentBase;

    public List<GameObject> tiles;
    public bool firstArranged;
    public bool secondArranged;
    public bool created;

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
