using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class HouseController : MonoBehaviour
{
    private StageManager stageManager;
    public GateType gateType;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Tags.enemy)
        {
            Debug.Log(other.gameObject);
            stageManager.currentHouseLife -= 1;
        }
    }
}
