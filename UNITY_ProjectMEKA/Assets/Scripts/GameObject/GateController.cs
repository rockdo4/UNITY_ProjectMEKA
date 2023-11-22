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
}

public class GateController : MonoBehaviour
{
    // 시작 전 대기시간
    public float startInterval;

    // 이동 관련
    [HideInInspector]
    public Defines.GateType gateType;
    private Transform[] waypoints;

    // 몬스터 스폰 관련
    [SerializeField]
    public List<WaveInfo> waveInfos;

    private int currentWave = 0;
    private int currentEnemyType = 0;
    private int currentEnemyCount = 1;
    private float spawnTimer = 0f;
    private float waveTimer = 0f;
    private bool once = false;

    // 이동 경로
    private GameObject enemyPath;
    private Rigidbody enemyPathRb;
    public float pathSpeed;
    private Vector3 targetPos = Vector3.zero;
    private float threshold = 0.1f;
    private int waypointIndex = 0;
    private Vector3 initPos;

    private void Awake()
    {
        // waypoints 할당
        foreach (var waypointParent in transform.parent.parent.GetComponentsInChildren<Waypoint>())
        {
            if(waypointParent.gateType == gateType)
            {
                var waypointChildCount = waypointParent.transform.childCount;
                waypoints = new Transform[waypointChildCount];
                for (int i = 0; i< waypointChildCount; ++i)
                {
                    waypoints[i] = waypointParent.transform.GetChild(i).transform;
                }
                break;
            }
        }

        // enemyPath 연결
        enemyPath = transform.GetChild(1).gameObject;
        enemyPathRb = enemyPath.GetComponent<Rigidbody>();
        initPos = enemyPathRb.position;
        if (enemyPath == null)
        {
            Debug.Log("enemyPath gameObject is null");
        }
        targetPos = waypoints[waypointIndex].position;
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        // 이동경로 가이드 전 대기 시간
        if (startInterval > 0f)
        {
            startInterval -= Time.deltaTime;
            return;
        }
        else
        {
            startInterval = 0f;
        }

        // 이동경로 가이드
        if(waveInfos[currentWave].pathGuideOn && currentWave < waveInfos.Count)
        {
            ShowEnemyPath();
        }

        // 웨이브 타이머
        if (currentWave >= waveInfos.Count)
            return;

        if (waveTimer > 0f)
        {
            waveTimer -= Time.deltaTime;
            return;
        }

        SpawnEnemies();
    }

    // 몬스터 스폰 함수
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
            targetPos = waypoints[waypointIndex].position;
        }

        targetPos.y = initPos.y;
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
                enemyPath.SetActive(false);
                //return;
            }
            targetPos = waypoints[waypointIndex].position;
        }
    }
}
