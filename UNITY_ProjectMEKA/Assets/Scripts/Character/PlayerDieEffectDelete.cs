using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieEffectDelete : MonoBehaviour
{
    public PlayerController player;
    
    void Update()
    {
        if(player.state.Hp <= 0 || !player.gameObject.activeInHierarchy)
        {
            gameObject.GetComponent<PoolAble>().ReleaseObject();
        }
    }
}
