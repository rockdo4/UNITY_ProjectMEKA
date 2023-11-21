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
    public Defines.GateType gateType;
    public GameObject[] waypoints;

    // 몬스터 스폰 관련
    [SerializeField]
    public List<WaveInfo> waveInfos;

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

    // 몬스터 스폰 함수
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
