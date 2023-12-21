using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class HouseController : MonoBehaviour
{
    private StageManager stageManager;
    public HouseType gateType;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Tags.enemy)
        {
            other.GetComponent<CharacterState>().Hp = 0;

            if(stageManager.gameState != GameState.Die)
            {
                stageManager.currentHouseLife -= 1;
            }
        }
    }
}
