using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDie : MonoBehaviour
{
    
    private CharacterState state;
    
    private void Awake()
    {
        state = GetComponent<CharacterState>();
    }
    void Update()
    {
        if(state.Hp <= 0f)
        {
            Destroy(gameObject);
            //GetComponent<PoolAble>().ReleaseObject();
        }
    }
}
