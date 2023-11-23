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
    public MoveType moveType;
}

[System.Serializable]
public struct WaveInfo
{
    public List<EnemySpawnInfo> enemySpawnInfos;
    public float waveInterval;
    public bool pathGuideOn;
    public float pathDuration;
}

public enum MoveType
{
    AutoTile,
    AutoStraight,
    Waypoint,
}

public class GateController : MonoBehaviour
{
    // 시작 전 대기시간
    public float startInterval;

    // 이동 관련
    [HideInInspector]
    public Defines.GateType gateType;
    protected Transform house;
    protected Transform[] waypoints;

    // 몬스터 스폰 관련
    [SerializeField]
    public List<WaveInfo> waveInfos;

    protected int currentWave = 0;
    protected int currentEnemyType = 0;
    protected int currentEnemyCount = 0;
    protected float spawnTimer = 0f;
    protected float waveTimer = 0f;
    protected bool once = false;

    // 이동 경로
    protected Vector3 initPos;
    protected GameObject enemyPath;
    protected Rigidbody enemyPathRb;
    public float pathSpeed;
    protected Vector3 targetPos = Vector3.zero;
    protected float threshold = 0.1f;
    protected int waypointIndex = 0;
    protected float pathDuration;
    protected bool pathDone = false;

    virtual protected void Awake()
    {
        // waypoints 할당
        var waypointParentsInMap = transform.parent.parent.GetComponentsInChildren<Waypoint>();
        if (waypointParentsInMap != null)
        {
            foreach (var waypointParent in waypointParentsInMap)
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
        }
        else
        {
            Debug.Log("There's no waypoints in the map.");
        }

        // 짝이 맞는 게이트를 마지막 웨이포인트로 할당
        //foreach(var houseController in transform.parent.GetComponentsInChildren<HouseController>())
        //{
        //    if(houseController.gateType == gateType)
        //    {
        //        if(waypoints == null)
        //        {
        //            waypoints = new Transform[1];
        //        }
        //        waypoints[waypoints.Length - 1] = houseController.transform;
        //        break;
        //    }
        //}
        foreach (var houseController in transform.parent.GetComponentsInChildren<HouseController>())
        {
            if (houseController.gateType == gateType)
            {
                house = houseController.transform;
            }
        }

        // enemyPath 연결
        enemyPath = transform.GetChild(1).gameObject;
        enemyPathRb = enemyPath.GetComponent<Rigidbody>();
        initPos = enemyPath.transform.localPosition;
        if (enemyPath == null)
        {
            Debug.Log("enemyPath gameObject is null");
        }
        targetPos = waypoints[waypointIndex].position;
        pathDuration = waveInfos[currentWave].pathDuration;
    }

    public void Start()
    {
    }

    virtual protected void FixedUpdate()
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

        // 웨이브 타이머
        if (currentWave >= waveInfos.Count)
        {
            return;
        }

        if (waveTimer > 0f)
        {
            Debug.Log($"{Time.time} 왜? {waveTimer}");
            waveTimer -= Time.deltaTime;
            return;
        }

        if (pathDone)
        {
            SpawnEnemies();
            return;
        }

        // 이동경로 가이드
        pathDuration -= Time.deltaTime;
        if (pathDuration <= 0f && !pathDone)
        {
            waypointIndex = 0;
            enemyPath.transform.localPosition = initPos;
            enemyPath.GetComponent<ParticleSystem>().Clear();
            enemyPath.GetComponent<ParticleSystem>().Stop();
            enemyPath.SetActive(false);
            pathDone = true;
            Debug.Log(waveTimer);
        }
        else if (waveInfos[currentWave].pathGuideOn && !pathDone && pathDuration > 0f)
        {
            ShowEnemyPath();
        }
    }

    // 몬스터 스폰 함수
    private void SpawnEnemies()
    {
        Debug.Log("스폰 에너미");
        var enemyInfo = waveInfos[currentWave].enemySpawnInfos[currentEnemyType];
        var enemyName = enemyInfo.prefab.transform.GetChild(0).GetComponent<CharacterState>().enemyType.ToString();
        if(!once)
        {
            var enemyGo = ObjectPoolManager.instance.GetGo(enemyName);
            SetEnemy(enemyGo, enemyInfo);
            currentEnemyCount++;
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
            waveTimer = waveInfos[currentWave].waveInterval;

            currentWave++;
            currentEnemyType = 0;
            pathDone = false;
            once = false;
            Debug.Log("다음 웨이브로!");
            if(currentWave < waveInfos.Count)
            {
                pathDuration = waveInfos[currentWave].pathDuration;
            }
        }
    }


    virtual public void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo)
    {
        enemyGo.transform.position = transform.position;
        enemyGo.transform.GetChild(0).GetComponent<EnemyController>().wayPoint = waypoints;
        enemyGo.transform.GetChild(0).GetComponent<CharacterState>().property = spawnInfo.attribute;
        enemyGo.transform.GetChild(0).GetComponent<CharacterState>().level = spawnInfo.level;
        enemyGo.transform.GetChild(0).GetComponent<EnemyController>().initPos = transform.position;
    }

    private void ShowEnemyPath()
    {
        if (!enemyPath.activeSelf)
        {
            enemyPath.SetActive(true);
        }

        if (!enemyPath.GetComponent<ParticleSystem>().isPlaying)
        {
            enemyPath.GetComponent<ParticleSystem>().Play();
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
                targetPos = waypoints[waypointIndex].position;
                enemyPath.transform.localPosition = initPos;
                enemyPath.GetComponent<ParticleSystem>().Clear();
                enemyPath.GetComponent<ParticleSystem>().Stop();
                return;
            }
            targetPos = waypoints[waypointIndex].position;
        }
    }
}
