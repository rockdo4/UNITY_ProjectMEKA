using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGate : GateController
{
    private void Awake()
    {
        // ��������Ʈ �Ҵ�
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

        // enemyPath ����
        enemyPath = transform.GetChild(1).gameObject;
        enemyPathRb = enemyPath.GetComponent<Rigidbody>();
        initPos = enemyPath.transform.localPosition;
        if (enemyPath == null)
        {
            Debug.Log("enemyPath gameObject is null");
        }

        // Ÿ����ġ �ʱ�ȭ & �̵����̵���� Ÿ�̸� �ʱ�ȭ
        targetPos = waypoints[waypointIndex].position;
        pathDuration = waveInfos[currentWave].pathDuration;

        // ������ ��������Ʈ �Ͽ콺�� �Ҵ�
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
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
