using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    // 이동 관련
    public GateType gateType;
    public GameObject[] waypoints;

    // 몬스터 스폰 관련
    public int numberOfMonsters;
    public float startWaitTime;
    public float spawnDuration;
    public float waveInterval;
    public GameObject enemyPrefab;

    private void Awake()
    {
        // 부모에서 WayPoint 컴포넌트 달린 자식 찾고, gateType 일치하는지 확인, 자식오브젝트들을 wapoints에 넣기
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
                Debug.Log("게이트1 웨이포인트 개수 : " + waypoints.Length);
                break;
            }
        }
    }

    // 몬스터 스폰 함수
    private void Start()
    {
        
    }

    private IEnumerator CoSpawnEnemyTest()
    {
        yield return new WaitForSeconds(startWaitTime);

        var enemy = GameObject.Instantiate(enemyPrefab);
        enemy.transform.position = transform.position;
        // enemy한테 웨이포인트 배열 전달

        yield return new WaitForSeconds(spawnDuration);
    }
}
