using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGate : GateController
{
    private void Awake()
    {
        base.Awake();

        // 하우스 할당
        foreach (var houseController in transform.parent.GetComponentsInChildren<HouseController>())
        {
            if (houseController.gateType == gateType)
            {
                house = houseController.transform;
                break;
            }
        }

        foreach (var wave in waveInfos)
        {
            if (wave.waypointGo == null)
            {
                wave.waypoints = new Transform[1];
            }
            wave.waypoints[wave.waypoints.Length - 1] = house.transform;
        }

        // 타겟위치 초기화 & 이동가이드라인 타이머 초기화
        targetPos = waveInfos[0].waypoints[0].position;
        targetPos.y = enemyPathRb.position.y;
        enemyPath.transform.LookAt(targetPos);
        pathDuration = waveInfos[currentWave].pathDuration;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
