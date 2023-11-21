using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnInfo
{
    public Defines.EnemyType type;
    public int count;
    public int interval;
}

[System.Serializable]
public struct WaveInfo
{
    public List<EnemySpawnInfo> enemySpawnInfos;
    public float waveInterval;
}

public class GateController : MonoBehaviour
{
    // 이동 관련
    [HideInInspector]
    public Defines.GateType gateType;
    private GameObject[] waypoints;

    // 몬스터 스폰 관련
    [SerializeField]
    public List<WaveInfo> waveInfos;

    private int currentWave = 0;
    private int currentEnemyType = 0;
    private int currentEnemyCount = 1;
    private float spawnTimer = 0f;
    private float waveTimer = 0f;
    private bool once = false;

    private void Awake()
    {
        // wapoints 할당
        foreach (var waypointParent in transform.parent.parent.GetComponentsInChildren<Waypoint>())
        {
            if(waypointParent.gateType == gateType)
            {
                var waypointChildCount = waypointParent.transform.childCount;
                for (int i = 0; i< waypointChildCount; ++i)
                {
                    waypoints = new GameObject[waypointChildCount];
                    waypoints[i] = waypointParent.transform.GetChild(i).gameObject;
                }
                break;
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (currentWave >= waveInfos.Count)
            return;

        if (waveTimer > 0)
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
        if(!once)
        {
            Debug.Log(enemyInfo.type.ToString());
            var enemyGo = ObjectPoolManager.instance.GetGo(enemyInfo.type.ToString());
            enemyGo.transform.position = transform.position;

            once = true;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer < waveInfos[currentWave].enemySpawnInfos[currentEnemyType].interval)
            return;

        var enemy = ObjectPoolManager.instance.GetGo(enemyInfo.type.ToString());
        enemy.transform.position = transform.position;
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



        //// 기존 코드
        //foreach (var wave in waveInfos)
        //{
        //    foreach(var waveEnemyInfo in wave.enemySpawnInfos)
        //    {
        //        for(int i = 0; i < waveEnemyInfo.count; ++i)
        //        {
        //            var enemy = ObjectPoolManager.instance.GetGo(waveEnemyInfo.type.ToString());
        //            enemy.transform.position = transform.position;
        //            // enemy 웨이포인트 세팅

        //            // interval 후 다음 몬스터 생성
        //        }
        //    }
        //    // spawnInterval 후 다음 웨이브 몬스터 생성
        //}
    }
}
