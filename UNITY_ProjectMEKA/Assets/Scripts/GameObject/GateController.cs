using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static Defines;
using static GateController;
using static UnityEngine.EventSystems.EventTrigger;

public class GateController : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        [SerializeField, Header("몬스터 프리팹")]
        public GameObject prefab;
        [SerializeField, Header("몬스터 수")]
        public int count;
        [SerializeField, Header("몬스터 종류")]
        public Defines.Property attribute;
        [SerializeField, Header("몬스터 레벨")]
        public int level;
        [SerializeField, Header("스폰 주기")]
        public int interval;
    }

    [System.Serializable]
    public class WaveInfo
    {
        [SerializeField, Header("몬스터 세팅")]
        public List<EnemySpawnInfo> enemySpawnInfos;

        [SerializeField, Header("웨이포인트")]
        public GameObject waypointGo;
        [HideInInspector]
        public Transform[] waypoints;
        [SerializeField, Header("이동 타입")]
        public Defines.MoveType moveType;
        [SerializeField, Header("반복 이동 횟수")]
        public int moveRepeat;
        [SerializeField, Header("웨이브 주기")]
        public float waveInterval;
        [SerializeField, Header("이동라인 ON/OFF")]
        public bool pathGuideOn;
        [SerializeField, Header("이동라인 시간")]
        public float pathDuration;
    }

    [SerializeField, Header("처음 대기 시간")]
    public float startInterval;

    [SerializeField, Header("게이트 종류")]
    public Defines.GateType gateType;
    protected Transform house;

    [SerializeField, Header("이동라인  스피드")]
    public float pathSpeed;

    [SerializeField, Header("웨이브 정보 세팅")]
    public List<WaveInfo> waveInfos;

    private StageManager stageManager;

    protected int currentWave = 0;
    protected int currentEnemyType = 0;
    protected int currentEnemyCount = 0;
    protected float spawnTimer = 0f;
    protected float waveTimer = 0f;
    protected bool firstGetPool = false;
    protected Vector3 spawnInitPos;

    protected Vector3 enemyPathInitPos;
    protected GameObject enemyPath;
    protected Rigidbody enemyPathRb;
    protected Vector3 targetPos = Vector3.zero;
    protected Vector3 enemyPathDirection;
    protected float threshold = 0.1f;
    protected int waypointIndex = 0;
    protected int repeatCount = -1;
    protected float pathDuration;
    protected bool pathDone = false;
    protected bool once = false;

    private WaveTimer timerScript;

    virtual protected void Awake()
    {
        spawnInitPos = new Vector3 (transform.position.x, 0.25f, transform.position.z);

        foreach (var wave in  waveInfos)
        {
            var waypointCount = wave.waypointGo.transform.childCount;
            wave.waypoints = new Transform[waypointCount+1];
            for (int i = 0; i < waypointCount; ++i)
            {
                wave.waypoints[i] = wave.waypointGo.transform.GetChild(i).transform;
            }
        }

        enemyPath = transform.GetChild(1).gameObject;
        enemyPathRb = enemyPath.GetComponent<Rigidbody>();
        enemyPathInitPos = enemyPath.transform.localPosition;
        if (enemyPath == null)
        {
            Debug.Log("enemyPath gameObject is null");
        }

        // ui update
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        foreach (var wave in waveInfos)
        {
            foreach(var enemyInfo in wave.enemySpawnInfos)
            {
                stageManager.allMonsterCount += enemyInfo.count;
            }
        }

        timerScript = FindObjectOfType<WaveTimer>();
        if(timerScript == null)
        {
            Debug.Log("timerScript is null");
        }
    }

    private void Start()
    {
    }

    virtual protected void FixedUpdate()
    {
		if (startInterval > 0f)
        {
            startInterval -= Time.deltaTime;
            return;
        }
        else
        {
            startInterval = 0f;
        }

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

        pathDuration -= Time.deltaTime;
        if ((pathDuration <= 0f && !pathDone) || (!waveInfos[currentWave].pathGuideOn && !pathDone))
        {
            waypointIndex = 0;
            enemyPath.transform.localPosition = enemyPathInitPos;
            enemyPath.GetComponent<ParticleSystem>().Clear();
            enemyPath.GetComponent<ParticleSystem>().Stop();
            enemyPath.SetActive(false);
            pathDone = true;
        }
        else if (waveInfos[currentWave].pathGuideOn && !pathDone && pathDuration > 0f)
        {
            switch (waveInfos[currentWave].moveType)
            {
                case Defines.MoveType.AutoTile:
                    break;
                case Defines.MoveType.Waypoint:
                    ShowEnemyPathWaypoint(waveInfos[currentWave]);
                    break;
                case Defines.MoveType.Straight:
                    ShowEnemyPathStraight(waveInfos[currentWave]);
                    break;
                case Defines.MoveType.WaypointRepeat:
                    ShowEnemyPathRepeat(waveInfos[currentWave]);
                    break;
            }
        }
    }

    public void SpawnEnemies()
    {
        var waveInfo = waveInfos[currentWave];
        var enemyInfo = waveInfo.enemySpawnInfos[currentEnemyType];
        var enemyName = enemyInfo.prefab.GetComponent<EnemyState>().name;

        //first spawn

        if (!firstGetPool)
        {
            if (currentEnemyCount == 0)
            {
                Debug.Log($"{currentWave} : {enemyName} 첫 번째 몬스터 스폰");
                timerScript.AddStartWave($"{currentWave+1}번 웨이브");
            }

            var enemyGo = ObjectPoolManager.instance.GetGo(enemyName);
            SetEnemy(enemyGo, enemyInfo, waveInfo);
            if (enemyGo.GetComponent<EnemyController>().states.Count != 0)
            {
                var enemyContoller = enemyGo.GetComponent<EnemyController>();
                enemyContoller.SetState(NPCStates.Move);
                var npcState = enemyContoller.stateManager.currentNPCBase as NPCDestinationStates;
                npcState.InitOnce();
            }
            currentEnemyCount++;
            firstGetPool = true;
        }

        //다음 종류 몹
        if (currentEnemyCount >= enemyInfo.count)
        {
            timerScript.AddEndWave($"{currentWave + 1}:{enemyName}");
            Debug.Log("다음 종류 몬스터");

            currentEnemyCount = 0;
            currentEnemyType++;
            if (currentEnemyType >= waveInfos[currentWave].enemySpawnInfos.Count)
            {
                waveTimer = waveInfos[currentWave].waveInterval;

                currentWave++;
                currentEnemyType = 0;
                currentEnemyCount = 0;
                pathDone = false;
                firstGetPool = false;
                if (currentWave < waveInfos.Count)
                {
                    pathDuration = waveInfos[currentWave].pathDuration;
                }
            }
            return;
        }
        //다음 웨이브
        else if (currentWave < waveInfos.Count)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer < waveInfos[currentWave].enemySpawnInfos[currentEnemyType].interval)
                return;

            var enemy = ObjectPoolManager.instance.GetGo(enemyName);
            SetEnemy(enemy, enemyInfo, waveInfo);
            if (enemy.GetComponent<EnemyController>().states.Count != 0)
            {
                var enemyContoller = enemy.GetComponent<EnemyController>();
                enemyContoller.SetState(NPCStates.Move);
                var npcState = enemyContoller.stateManager.currentNPCBase as NPCDestinationStates;
                npcState.InitOnce();
            }

            currentEnemyCount++;
        }
        spawnTimer = 0f;
    }


    virtual public void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo, WaveInfo waveInfo)
    {
        var enemyController = enemyGo.GetComponent<EnemyController>();

        enemyController.wayPoint = waveInfo.waypoints;
        enemyController.waypointIndex = 0;
        enemyController.initPos = spawnInitPos;
        enemyController.moveType = waveInfo.moveType;
        enemyController.moveRepeatCount = waveInfo.moveRepeat;
        enemyController.state.property = spawnInfo.attribute;
        enemyController.state.level = spawnInfo.level;

        // need to apply monster stats by level
    }

    private void ShowEnemyPathWaypoint(WaveInfo waveInfo)
    {
        if (!enemyPath.activeSelf)
        {
            enemyPath.SetActive(true);
            enemyPath.transform.localPosition = enemyPathInitPos;
            waypointIndex = 0;
            targetPos = waveInfo.waypoints[waypointIndex].position;
            targetPos.y = enemyPath.transform.position.y;
            enemyPath.transform.LookAt(targetPos);
        }

        if (!enemyPath.GetComponent<ParticleSystem>().isPlaying)
        {
            enemyPath.GetComponent<ParticleSystem>().Play();
        }

        var pos = enemyPathRb.position;
        pos += enemyPathRb.transform.forward * pathSpeed * Time.deltaTime;
        enemyPathRb.MovePosition(pos);

        if (Vector3.Distance(new Vector3(pos.x, pos.z), new Vector3(targetPos.x, targetPos.z)) < threshold)
        {
            waypointIndex++;

            if (waypointIndex >= waveInfo.waypoints.Length)
            {
                enemyPath.GetComponent<ParticleSystem>().Clear();
                enemyPath.GetComponent<ParticleSystem>().Stop();
                enemyPath.SetActive(false);
                return;
            }
            targetPos = waveInfo.waypoints[waypointIndex].position;
            targetPos.y = enemyPath.transform.position.y;
        }
        enemyPath.transform.LookAt(targetPos);
    }

    private void ShowEnemyPathStraight(WaveInfo waveInfo)
    {
        if (!enemyPath.activeSelf)
        {
            enemyPath.SetActive(true);
            enemyPath.transform.localPosition = enemyPathInitPos;
        }

        if (!enemyPath.GetComponent<ParticleSystem>().isPlaying)
        {
            enemyPath.GetComponent<ParticleSystem>().Play();
        }

        var targetPosAppliedY = waveInfo.waypoints[waveInfo.waypoints.Length - 1].position;
        targetPosAppliedY.y = enemyPathRb.position.y;
        if (targetPos != targetPosAppliedY)
        {
            targetPos = waveInfo.waypoints[waveInfo.waypoints.Length - 1].position;
            targetPos.y = enemyPathRb.position.y;
            enemyPath.transform.LookAt(targetPos);
            //once = true;
        }

        var pos = enemyPathRb.position;
        pos += enemyPath.transform.forward * pathSpeed * Time.deltaTime;
        enemyPathRb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold)
        {
            enemyPath.GetComponent<ParticleSystem>().Clear();
            enemyPath.GetComponent<ParticleSystem>().Stop();
            enemyPath.SetActive(false);
            return;
        }
    }

    private void ShowEnemyPathRepeat(WaveInfo waveInfo)
    {
        if (!enemyPath.activeSelf)
        {
            enemyPath.SetActive(true);
            enemyPath.transform.localPosition = enemyPathInitPos;
            waypointIndex = 0;
            targetPos = waveInfo.waypoints[waypointIndex].position;
            targetPos.y = enemyPath.transform.position.y;
            enemyPath.transform.LookAt(targetPos);
        }

        if (!enemyPath.GetComponent<ParticleSystem>().isPlaying)
        {
            enemyPath.GetComponent<ParticleSystem>().Play();
        }

        var pos = enemyPathRb.position;
        pos += enemyPath.transform.forward * pathSpeed * Time.deltaTime;
        enemyPathRb.MovePosition(pos);

        if (Vector3.Distance(pos, targetPos) < threshold)
        {
            if (waypointIndex == waveInfo.waypoints.Length - 2)
            {
                waypointIndex = -1;
            }
            else if (waypointIndex == 0)
            {
                repeatCount++;
                if (repeatCount == waveInfo.moveRepeat)
                {
                    waypointIndex = waveInfo.waypoints.Length - 2;
                }
            }
            else if (waypointIndex == waveInfo.waypoints.Length - 1)
            {
                enemyPath.GetComponent<ParticleSystem>().Clear();
                enemyPath.GetComponent<ParticleSystem>().Stop();
                enemyPath.SetActive(false);
                return;
            }
            waypointIndex++;
            targetPos = waveInfo.waypoints[waypointIndex].position;
            targetPos.y = enemyPath.transform.position.y;
            //enemyPath.transform.LookAt(targetPos);
        }
        enemyPath.transform.LookAt(targetPos);
    }
}
