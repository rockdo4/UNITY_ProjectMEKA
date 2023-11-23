using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnInfo
{
    public GameObject prefab;
    public int count;
    public Defines.Property attribute;
    public int level;
    public int interval;
}

[System.Serializable]
public struct WaveInfo
{
    public List<EnemySpawnInfo> enemySpawnInfos;
    public float waveInterval;
    public bool pathGuideOn;
    public float pathDuration;
}

public class GateController : MonoBehaviour
{
    // ���� �� ���ð�
    public float startInterval;

    // �̵� ����
    [HideInInspector]
    public Defines.GateType gateType;
    private Transform[] waypoints;

    // ���� ���� ����
    [SerializeField]
    public List<WaveInfo> waveInfos;

    private int currentWave = 0;
    private int currentEnemyType = 0;
    private int currentEnemyCount = 1;
    private float spawnTimer = 0f;
    private float waveTimer = 0f;
    private bool once = false;

    // �̵� ���
    private GameObject enemyPath;
    private Rigidbody enemyPathRb;
    public float pathSpeed;
    private Vector3 targetPos = Vector3.zero;
    private float threshold = 0.1f;
    private int waypointIndex = 0;
    private Vector3 initPos;
    private float pathDuration;
    private bool pathDone = false;

    private void Awake()
    {
        // waypoints �Ҵ�
        foreach (var waypointParent in transform.parent.parent.GetComponentsInChildren<Waypoint>())
        {
            if(waypointParent.gateType == gateType)
            {
                var waypointChildCount = waypointParent.transform.childCount;
                waypoints = new Transform[waypointChildCount + 1];
                for (int i = 0; i< waypointChildCount; ++i)
                {
                    waypoints[i] = waypointParent.transform.GetChild(i).transform;
                }
                break;
            }
        }

        // ¦�� �´� ����Ʈ�� ������ ��������Ʈ�� �Ҵ�
        foreach(var houseController in transform.parent.GetComponentsInChildren<HouseController>())
        {
            if(houseController.gateType == gateType)
            {
                waypoints[waypoints.Length - 1] = houseController.transform;
                break;
            }
        }

        // enemyPath ����
        enemyPath = transform.GetChild(1).gameObject;
        enemyPathRb = enemyPath.GetComponent<Rigidbody>();
        initPos = enemyPathRb.position;
        if (enemyPath == null)
        {
            Debug.Log("enemyPath gameObject is null");
        }
        targetPos = waypoints[waypointIndex].position;
        pathDuration = waveInfos[currentWave].pathDuration;
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        // ���̺� Ÿ�̸�
        if (currentWave >= waveInfos.Count)
        {
            return;
        }
        else if (waveTimer > 0f)
        {
            waveTimer -= Time.deltaTime;
            return;
        }

        // �̵���� ���̵� �� ��� �ð�
        if (startInterval > 0f)
        {
            startInterval -= Time.deltaTime;
            return;
        }
        else
        {
            startInterval = 0f;
        }

        if(pathDone)
        {
            SpawnEnemies();
        }

        // �̵���� ���̵�
        pathDuration -= Time.deltaTime;
        if (pathDuration <= 0f && !pathDone)
        {
            enemyPath.SetActive(false);
            pathDone = true;
        }
        else if (waveInfos[currentWave].pathGuideOn && !pathDone)
        {
            ShowEnemyPath();
        }
    }

    // ���� ���� �Լ�
    private void SpawnEnemies()
    {
        var enemyInfo = waveInfos[currentWave].enemySpawnInfos[currentEnemyType];
        var enemyName = enemyInfo.prefab.transform.GetChild(0).GetComponent<CharacterState>().enemyType.ToString();
        if(!once)
        {
            var enemyGo = ObjectPoolManager.instance.GetGo(enemyName);
            SetEnemy(enemyGo, enemyInfo);
            once = true;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer < waveInfos[currentWave].enemySpawnInfos[currentEnemyType].interval)
            return;

        var enemy = ObjectPoolManager.instance.GetGo(enemyName);
        SetEnemy(enemy, enemyInfo);

        currentEnemyCount++;
        spawnTimer = 0f;

        if(currentEnemyCount >= enemyInfo.count)
        {
            currentEnemyCount = 0;
            currentEnemyType++;
        }

        if (currentEnemyType >= waveInfos[currentWave].enemySpawnInfos.Count)
        {
            currentEnemyType = 0;
            waveTimer = waveInfos[currentWave].waveInterval;
            currentWave++;
            pathDone = false;
            if(currentWave < waveInfos.Count)
            {
                pathDuration = waveInfos[currentWave].pathDuration;
            }
        }
    }

    private void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo)
    {
        enemyGo.transform.position = transform.position;
        enemyGo.transform.GetChild(0).GetComponent<EnemyController>().wayPoint = waypoints;
        enemyGo.transform.GetChild(0).GetComponent<CharacterState>().property = spawnInfo.attribute;
        enemyGo.transform.GetChild(0).GetComponent<CharacterState>().level = spawnInfo.level;
        enemyGo.transform.GetChild(0).GetComponent<EnemyController>().initPos = transform.position;
    }

    private void ShowEnemyPath()
    {
        if (!enemyPath.active)
        {
            enemyPath.SetActive(true);
        }
        targetPos = waypoints[waypointIndex].position;
        targetPos.y = enemyPathRb.position.y;
        enemyPath.transform.LookAt(targetPos);
        var pos = enemyPathRb.position;
        pos += enemyPath.transform.forward * pathSpeed * Time.deltaTime;
        enemyPathRb.MovePosition(pos);

        if(Vector3.Distance(pos, targetPos) < threshold)
        {
            waypointIndex++;

            if(waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
                enemyPath.transform.localPosition = initPos;
            }
            targetPos = waypoints[waypointIndex].position;
        }
    }
}
