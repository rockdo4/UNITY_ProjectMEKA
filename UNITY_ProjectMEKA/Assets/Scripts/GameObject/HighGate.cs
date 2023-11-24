using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using static EnemyController;

public class HighGate : GateController
{
    protected override void Awake()
    {
        base.Awake();

        // 마지막 웨이포인트 하우스로 할당
        var houseParent = GameObject.FindGameObjectWithTag("LowDoor");
        foreach (var houseController in houseParent.GetComponentsInChildren<HouseController>())
        {
            var houseType = houseController.gateType.ToString();
            var gateTypeString = gateType.ToString();
            if (houseType[houseType.Length-1].Equals(gateTypeString[gateTypeString.Length-1]))
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

    public override void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo)
    {
        base.SetEnemy(enemyGo, spawnInfo);
        enemyGo.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
    }
}
