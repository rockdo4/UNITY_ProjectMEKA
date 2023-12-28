using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Defines;

public class CanDie : MonoBehaviour
{
    private StageManager stageManager;
    private CharacterState state;
    

    public UnityEvent action;
    public UnityEvent updateUI;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        state = GetComponent<CharacterState>();
        
        action = new UnityEvent();
        updateUI = new UnityEvent();

        updateUI.AddListener(() =>
        {
            if(GetComponent<PlayerState>() == null)
            {
                stageManager.killMonsterCount++;
            }
            Debug.Log(Time.time + " killMonsterCount++");
        });
    }
    void Update()
    {
        if (state.Hp <= 0f)
        {
            action.Invoke();
            updateUI.Invoke();

            // if this is a monster
            if (GetComponent<PoolAble>() != null)
            {
                // temp value => need to apply monster state.monsterDieCost(after monster prefabs done)
                stageManager.currentCost += 1f;
                GetComponent<PoolAble>().ReleaseObject();
            }
        }
    }
}
