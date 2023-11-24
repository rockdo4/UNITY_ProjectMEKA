using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGate : GateController
{
    private void Awake()
    {
        base.Awake();

        // 마지막 웨이포인트 하우스로 할당
        foreach (var houseController in transform.parent.GetComponentsInChildren<HouseController>())
        {
            if (houseController.gateType == gateType)
            {
                house = houseController.transform;
                break;
            }
        }

        if (waypoints == null)
        {
            waypoints = new Transform[1];
        }
        waypoints[waypoints.Length - 1] = house.transform;

        // 타겟위치 초기화 & 이동가이드라인 타이머 초기화
        targetPos = waypoints[waypointIndex].position;
        pathDuration = waveInfos[currentWave].pathDuration;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
