using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    // �̵� ����
    public GateType gateType;
    public GameObject[] waypoints;

    // ���� ���� ����
    public int numberOfMonsters;
    public float startWaitTime;
    public float spawnDuration;
    public float waveInterval;
    public GameObject enemyPrefab;

    private void Awake()
    {
        // �θ𿡼� WayPoint ������Ʈ �޸� �ڽ� ã��, gateType ��ġ�ϴ��� Ȯ��, �ڽĿ�����Ʈ���� wapoints�� �ֱ�
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
                Debug.Log("����Ʈ1 ��������Ʈ ���� : " + waypoints.Length);
                break;
            }
        }
    }

    // ���� ���� �Լ�
    private void Start()
    {
        
    }

    private IEnumerator CoSpawnEnemyTest()
    {
        yield return new WaitForSeconds(startWaitTime);

        var enemy = GameObject.Instantiate(enemyPrefab);
        enemy.transform.position = transform.position;
        // enemy���� ��������Ʈ �迭 ����

        yield return new WaitForSeconds(spawnDuration);
    }
}
