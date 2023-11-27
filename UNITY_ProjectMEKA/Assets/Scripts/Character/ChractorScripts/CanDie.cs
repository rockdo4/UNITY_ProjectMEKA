using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            Debug.Log(gameObject.name);
            //GetComponentInParent<PoolAble>().ReleaseObject(0.3f);
            Destroy(gameObject);
        }
    }
}
