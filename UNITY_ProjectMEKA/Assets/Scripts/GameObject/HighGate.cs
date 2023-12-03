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

        // �Ͽ콺 �Ҵ�
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

        foreach(var wave in waveInfos)
        {
            if (wave.waypointGo == null)
            {
                wave.waypoints = new Transform[1];
            }
            wave.waypoints[wave.waypoints.Length - 1] = house.transform;
        }

        // Ÿ����ġ �ʱ�ȭ & �̵����̵���� Ÿ�̸� �ʱ�ȭ
        targetPos = waveInfos[currentWave].waypoints[waypointIndex].position;
        targetPos.y = enemyPathInitPos.y;
        enemyPathRb.transform.LookAt(targetPos);
        pathDuration = waveInfos[currentWave].pathDuration;
        pathDone = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo, WaveInfo waveInfo)
    {
        base.SetEnemy(enemyGo, spawnInfo, waveInfo);
        enemyGo.GetComponent<Rigidbody>().useGravity = false;
    }
}
