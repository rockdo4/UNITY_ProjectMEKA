using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject prefab;
        public int count;
        public Defines.Property attribute;
        public int level;
        public int interval;
        public Defines.MoveType moveType;
        public int moveRepeat;
    }

    [System.Serializable]
    public class WaveInfo
    {
        public List<EnemySpawnInfo> enemySpawnInfos;
        public float waveInterval;
        public bool pathGuideOn;
        public float pathDuration;
    }

    // 시작 전 대기시간
    public float startInterval;

    // 이동 관련
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
    protected bool firstGetPool = false;

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
        }
        else if (waveInfos[currentWave].pathGuideOn && !pathDone && pathDuration > 0f)
        {
            ShowEnemyPath();
        }
    }

    // 몬스터 스폰 함수
    virtual public void SpawnEnemies()
    {
        var enemyInfo = waveInfos[currentWave].enemySpawnInfos[currentEnemyType];
        var enemyName = enemyInfo.prefab.transform.GetChild(0).GetComponent<CharacterState>().enemyType.ToString();
        if(!firstGetPool)
        {
            var enemyGo = ObjectPoolManager.instance.GetGo(enemyName);
            SetEnemy(enemyGo, enemyInfo);
            currentEnemyCount++;
            firstGetPool = true; 
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
            firstGetPool = false;
            if(currentWave < waveInfos.Count)
            {
                pathDuration = waveInfos[currentWave].pathDuration;
            }
        }
    }


    virtual public void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo)
    {
        enemyGo.GetComponentInChildren<EnemyController>().wayPoint = waypoints;
        enemyGo.GetComponentInChildren<EnemyController>().initPos = transform.position;
        enemyGo.GetComponentInChildren<EnemyController>().moveType = spawnInfo.moveType;
        enemyGo.GetComponentInChildren<EnemyController>().moveRepeatCount = spawnInfo.moveRepeat;
        enemyGo.GetComponentInChildren<CharacterState>().property = spawnInfo.attribute;
        enemyGo.GetComponentInChildren<CharacterState>().level = spawnInfo.level;
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
