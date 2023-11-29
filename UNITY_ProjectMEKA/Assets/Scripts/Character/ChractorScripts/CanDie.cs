using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CanDie : MonoBehaviour
{
    
    private CharacterState state;
    public UnityEvent action;

    private void Awake()
    {
        state = GetComponent<CharacterState>();
        action = new UnityEvent();
    }
    void Update()
    {
        if(state.Hp <= 0f)
        {
            Debug.Log(gameObject.name);
            action.Invoke();
            //Destroy(gameObject);
            GetComponent<PoolAble>().ReleaseObject();
        }
    }
}
