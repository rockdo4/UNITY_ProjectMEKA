using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class HighGate : GateController
{
    private void Awake()
    {
        // 웨이포인트 할당
        var waypointParentsInMap = transform.parent.parent.GetComponentsInChildren<Waypoint>();
        if (waypointParentsInMap != null)
        {
            foreach (var waypointParent in waypointParentsInMap)
            {
                if (waypointParent.gateType == gateType)
                {
                    var waypointChildCount = waypointParent.transform.childCount;
                    waypoints = new Transform[waypointChildCount + 1];
                    for (int i = 0; i < waypointChildCount; ++i)
                    {
                        waypoints[i] = waypointParent.transform.GetChild(i).transform;
                    }
                    break;
                }
            }
        }
        else
        {
            Debug.Log("There's no waypoints in the map.");
        }

        // enemyPath 연결
        enemyPath = transform.GetChild(1).gameObject;
        enemyPathRb = enemyPath.GetComponent<Rigidbody>();
        initPos = enemyPath.transform.localPosition;
        if (enemyPath == null)
        {
            Debug.Log("enemyPath gameObject is null");
        }

        if (waypoints == null)
        {
            waypoints = new Transform[1];
        }

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
        waypoints[waypoints.Length - 1] = house.transform;

        // 타겟위치 초기화 & 이동가이드라인 타이머 초기화
        targetPos = waypoints[waypointIndex].position;
        pathDuration = waveInfos[currentWave].pathDuration;

        Debug.Log(waypoints.Length);
        Debug.Log(house);
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
