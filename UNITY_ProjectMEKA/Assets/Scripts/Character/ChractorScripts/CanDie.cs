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
    private PlayerState player;
    private EnemyState enemy;

    public UnityEvent action;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        state = GetComponent<CharacterState>();
        player = GetComponent<PlayerState>();
        if(player == null)
        {
            enemy = GetComponent<EnemyState>();
        }
        action = new UnityEvent();
    }
    void Update()
    {
        if(player != null) 
        {
            if (player.Hp <= 0f)
            {
                action.Invoke();

                // if this is a monster
                if (GetComponent<PoolAble>() != null)
                {
                    // temp value => need to apply monster state.monsterDieCost(after monster prefabs done)
                    stageManager.currentCost += 1f;
                    GetComponent<PoolAble>().ReleaseObject();
                }
            }
        }
        else
        {
            if (enemy.Hp <= 0f)
            {
                action.Invoke();

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
}
