using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class HighGate : GateController
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

        if (waypoints == null)
        {
            waypoints = new Transform[1];
        }

        // ������ ��������Ʈ �Ͽ콺�� �Ҵ�
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

        // Ÿ����ġ �ʱ�ȭ & �̵����̵���� Ÿ�̸� �ʱ�ȭ
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
        //enemyGo.SetActive(false);
        //enemyGo.SetActive(true);
    }

    public override void SpawnEnemies()
    {
        var enemyInfo = waveInfos[currentWave].enemySpawnInfos[currentEnemyType];
        var enemyName = enemyInfo.prefab.transform.GetChild(0).GetComponent<CharacterState>().enemyType.ToString();
        if (!firstGetPool)
        {
            var enemyGo = ObjectPoolManager.instance.GetGo(enemyName, transform.position);
            Debug.Log($"���� ���� �� !!! {enemyGo.GetComponentInChildren<EnemyController>().initPos}, ����ġ : {enemyGo.transform.position}, {enemyGo.GetComponentInChildren<CharacterState>().property}, ��������Ʈ : {enemyGo.GetComponentInChildren<EnemyController>().wayPoint[0]}");

            SetEnemy(enemyGo, enemyInfo);
            Debug.Log($"���� ���� �� !!! {enemyGo.GetComponentInChildren<EnemyController>().initPos}, ����ġ : {enemyGo.transform.position},{enemyGo.GetComponentInChildren<CharacterState>().property}, ��������Ʈ : {enemyGo.GetComponentInChildren<EnemyController>().wayPoint[0]}");

            currentEnemyCount++;
            firstGetPool = true;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer < waveInfos[currentWave].enemySpawnInfos[currentEnemyType].interval)
            return;

        var enemy = ObjectPoolManager.instance.GetGo(enemyName, transform.position);
        Debug.Log($"���� get ���� !!! {enemy.GetComponentInChildren<EnemyController>().initPos}, ����ġ {enemy.transform.position}, {enemy.GetComponentInChildren<CharacterState>().property}, ��������Ʈ : {enemy.GetComponentInChildren<EnemyController>().wayPoint[0]}");
        SetEnemy(enemy, enemyInfo);
        Debug.Log($"���� ���� �� !!! {enemy.GetComponentInChildren<EnemyController>().initPos}, ����ġ {enemy.transform.position}, {enemy.GetComponentInChildren<CharacterState>().property}, ��������Ʈ : {enemy.GetComponentInChildren<EnemyController>().wayPoint[0]}");

        currentEnemyCount++;
        spawnTimer = 0f;

        if (currentEnemyCount >= enemyInfo.count)
        {
            currentEnemyCount = 0;
            currentEnemyType++;
        }

        if (currentEnemyType >= waveInfos[currentWave].enemySpawnInfos.Count)
        {
            waveTimer = waveInfos[currentWave].waveInterval;

            currentWave++;
            currentEnemyType = 0;
            pathDone = false;
            firstGetPool = false;
            if (currentWave < waveInfos.Count)
            {
                pathDuration = waveInfos[currentWave].pathDuration;
            }
        }

    }
}
