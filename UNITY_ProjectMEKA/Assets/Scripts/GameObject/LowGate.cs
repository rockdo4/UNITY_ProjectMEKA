using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGate : GateController
{
    private void Awake()
    {
        base.Awake();

        // �Ͽ콺 �Ҵ�
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

        // Ÿ����ġ �ʱ�ȭ & �̵����̵���� Ÿ�̸� �ʱ�ȭ
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
