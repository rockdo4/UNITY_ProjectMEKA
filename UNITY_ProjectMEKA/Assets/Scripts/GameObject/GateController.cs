using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GateController;

public class GateController : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        [SerializeField, Header("���� ������")]
        public GameObject prefab;
        [SerializeField, Header("���� ��")]
        public int count;
        [SerializeField, Header("���� ����")]
        public Defines.Property attribute;
        [SerializeField, Header("���� ����")]
        public int level;
        [SerializeField, Header("���� �ֱ�")]
        public int interval;
    }

    [System.Serializable]
    public class WaveInfo
    {
        [SerializeField, Header("���� ����")]
        public List<EnemySpawnInfo> enemySpawnInfos;

        // 11.25 wave ������ �̵�
        [SerializeField, Header("��������Ʈ")]
        public GameObject waypointGo;
        [HideInInspector]
        public Transform[] waypoints;
        [SerializeField, Header("�̵� Ÿ��")]
        public Defines.MoveType moveType;
        [SerializeField, Header("�ݺ� �̵� Ƚ��")]
        public int moveRepeat;
        [SerializeField, Header("���̺� �ֱ�")]
        public float waveInterval;
        [SerializeField, Header("�̵����� ON/OFF")]
        public bool pathGuideOn;
        [SerializeField, Header("�̵����� �ð�")]
        public float pathDuration;
    }

    // ���� �� ���ð�
    [SerializeField, Header("ó�� ��� �ð�")]
    public float startInterval;

    // �̵� ����
    [SerializeField, Header("����Ʈ ����")]
    public Defines.GateType gateType;
    protected Transform house;

    // ���� ���� ����
    [SerializeField, Header("���̺� ���� ����")]
    public List<WaveInfo> waveInfos;

    protected int currentWave = 0;
    protected int currentEnemyType = 0;
    protected int currentEnemyCount = 0;
    protected float spawnTimer = 0f;
    protected float waveTimer = 0f;
    protected bool firstGetPool = false;

    // �̵� ���
    public float pathSpeed;
    protected Vector3 initPos;
    protected GameObject enemyPath;
    protected Rigidbody enemyPathRb;
    protected Vector3 targetPos = Vector3.zero;
    protected float threshold = 0.1f;
    protected int waypointIndex = 0;
    protected int repeatCount = -1;
    protected float pathDuration;
    protected bool pathDone = false;
    protected bool once = false;

    virtual protected void Awake()
    {
        // waypoints �Ҵ�
        foreach(var wave in  waveInfos)
        {
            var waypointCount = wave.waypointGo.transform.childCount;
            wave.waypoints = new Transform[waypointCount+1];
            for (int i = 0; i < waypointCount; ++i)
            {
                wave.waypoints[i] = wave.waypointGo.transform.GetChild(i).transform;
            }
        }

        // enemyPath ����
        enemyPath = transform.GetChild(1).gameObject;
        enemyPathRb = enemyPath.GetComponent<Rigidbody>();
        initPos = enemyPath.transform.localPosition;
        if (enemyPath == null)
        {
            Debug.Log("enemyPath gameObject is null");
        }
    }

    private void Start()
    {
    }

    virtual protected void FixedUpdate()
    {
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

        // ���̺� Ÿ�̸�
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

        // �̵���� ���̵�
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
            switch (waveInfos[currentWave].moveType)
            {
                case Defines.MoveType.AutoTile:
                    break;
                case Defines.MoveType.Waypoint:
                    //ShowEnemyPathWaypoint();
                    break;
                case Defines.MoveType.Straight:
                    //ShowEnemyPathStraight();
                    break;
                case Defines.MoveType.WaypointRepeat:
                    //ShowEnemyPathRepeat();
                    break;
            }
        }
    }

    // ���� ���� �Լ�
    public void SpawnEnemies()
    {
        var waveInfo = waveInfos[currentWave];
        var enemyInfo = waveInfo.enemySpawnInfos[currentEnemyType];
        var enemyName = enemyInfo.prefab.transform.GetChild(0).GetComponent<CharacterState>().enemyType.ToString();

        // ���̺긶�� ù��° ���ʹ� �ð� �� ��ٸ��� ����
        if (!firstGetPool)
        {
            var enemyGo = ObjectPoolManager.instance.GetGo(enemyName);
            SetEnemy(enemyGo, enemyInfo, waveInfo);
            if (enemyGo.GetComponentInChildren<EnemyController>().states.Count != 0)
            {
                enemyGo.GetComponentInChildren<EnemyController>().SetState(NPCStates.Move);
            }
            currentEnemyCount++;
            firstGetPool = true;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer < waveInfos[currentWave].enemySpawnInfos[currentEnemyType].interval)
            return;

        if (currentEnemyCount >= enemyInfo.count)
        {
            Debug.Log("���� ������!");
            currentEnemyCount = 0;
            currentEnemyType++;
            if (currentEnemyType >= waveInfos[currentWave].enemySpawnInfos.Count)
            {
                Debug.Log("���� ���̺��!");
                waveTimer = waveInfos[currentWave].waveInterval; // �Ѿ�� �� ���̺��� interval ����

                currentWave++;
                currentEnemyType = 0;
                currentEnemyCount = 0;
                pathDone = false;
                firstGetPool = false;
                if (currentWave < waveInfos.Count)
                {
                    pathDuration = waveInfos[currentWave].pathDuration;
                }
                spawnTimer = 0f;
            }
            return;
        }
        else if (currentWave < waveInfos.Count)
        {
            var enemy = ObjectPoolManager.instance.GetGo(enemyName);
            SetEnemy(enemy, enemyInfo, waveInfo);
            if (enemy.GetComponentInChildren<EnemyController>().states.Count != 0)
            {
                enemy.GetComponentInChildren<EnemyController>().SetState(NPCStates.Move);
            }

            currentEnemyCount++;
        }
        spawnTimer = 0f;
    }


    virtual public void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo, WaveInfo waveInfo)
    {
        var enemyController = enemyGo.GetComponentInChildren<EnemyController>();
        var enemyCharacterState = enemyGo.GetComponentInChildren<CharacterState>();

        enemyController.wayPoint = waveInfo.waypoints;
        enemyController.waypointIndex = 0;
        enemyController.initPos = transform.position;
        enemyController.moveType = waveInfo.moveType;
        enemyController.moveRepeatCount = waveInfo.moveRepeat;
        enemyCharacterState.property = spawnInfo.attribute;
        enemyCharacterState.level = spawnInfo.level;
    }

    //private void ShowEnemyPathWaypoint()
    //{
    //    if (!enemyPath.activeSelf)
    //    {
    //        enemyPath.SetActive(true);
    //    }

    //    if (!enemyPath.GetComponent<ParticleSystem>().isPlaying)
    //    {
    //        enemyPath.GetComponent<ParticleSystem>().Play();
    //    }

    //    var pos = enemyPathRb.position;
    //    pos += enemyPath.transform.forward * pathSpeed * Time.deltaTime;
    //    enemyPathRb.MovePosition(pos);

    //    if(Vector3.Distance(pos, targetPos) < threshold)
    //    {
    //        waypointIndex++;
    //        targetPos = waypoints[waypointIndex].position;
    //        targetPos.y = enemyPathRb.position.y;
    //        enemyPath.transform.LookAt(targetPos);

    //        if (waypointIndex >= waypoints.Length)
    //        {
    //            waypointIndex = 0;
    //            targetPos = waypoints[waypointIndex].position;
    //            enemyPath.transform.localPosition = initPos;
    //            enemyPath.GetComponent<ParticleSystem>().Clear();
    //            enemyPath.GetComponent<ParticleSystem>().Stop();
    //            return;
    //        }
    //    }
    //}

    //private void ShowEnemyPathStraight()
    //{
    //    if (!enemyPath.activeSelf)
    //    {
    //        enemyPath.SetActive(true);
    //    }

    //    if (!enemyPath.GetComponent<ParticleSystem>().isPlaying)
    //    {
    //        enemyPath.GetComponent<ParticleSystem>().Play();
    //    }

    //    if (!once)
    //    {
    //        targetPos = waypoints[waypoints.Length - 1].position;
    //        targetPos.y = enemyPathRb.position.y;
    //        enemyPath.transform.LookAt(targetPos);
    //        once = true;
    //    }

    //    var pos = enemyPathRb.position;
    //    pos += enemyPath.transform.forward * pathSpeed * Time.deltaTime;
    //    enemyPathRb.MovePosition(pos);

    //    if (Vector3.Distance(pos, targetPos) < threshold) // ���� ��������Ʈ �����ϸ�
    //    {
    //        enemyPath.transform.localPosition = initPos;
    //        enemyPath.transform.LookAt(targetPos);
    //        enemyPath.GetComponent<ParticleSystem>().Clear();
    //        enemyPath.GetComponent<ParticleSystem>().Stop();
    //        return;
    //    }
    //}

    //private void ShowEnemyPathRepeat()
    //{
    //    var pos = enemyPathRb.position;
    //    pos += enemyPath.transform.forward * pathSpeed * Time.deltaTime;
    //    enemyPathRb.MovePosition(pos);

    //    var enemyInfo = waveInfos[currentWave].enemySpawnInfos[currentEnemyType];

    //    if (Vector3.Distance(pos, targetPos) < threshold) // ���� ��������Ʈ �����ϸ�
    //    {
    //        if (waypointIndex == waypoints.Length - 2) // ������-1 ��������Ʈ �����ϸ�
    //        {
    //            waypointIndex = -1;
    //        }
    //        else if (waypointIndex == 0) // �ѹ��� ����
    //        {
    //            repeatCount++;
    //            if (repeatCount == enemyInfo.moveRepeat)
    //            {
    //                // ������ ��������Ʈ �Ҵ�
    //                waypointIndex = waypoints.Length - 2;
    //            }
    //        }
    //        else if (waypointIndex == waypoints.Length - 1)
    //        {
    //            enemyPath.transform.localPosition = initPos;
    //            enemyPath.transform.LookAt(targetPos);
    //            enemyPath.GetComponent<ParticleSystem>().Clear();
    //            enemyPath.GetComponent<ParticleSystem>().Stop();
    //            //enemyCtrl.GetComponentInParent<PoolAble>().ReleaseObject();
    //            return;
    //        }
    //        waypointIndex++;
    //        targetPos = waypoints[waypointIndex].position;
    //        targetPos.y = enemyPathRb.position.y;
    //        enemyPath.transform.LookAt(targetPos);
    //    }
    //}
}
