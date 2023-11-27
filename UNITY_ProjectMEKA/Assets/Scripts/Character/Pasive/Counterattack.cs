using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counterattack : MonoBehaviour
{
    EnemyController enemy;
    GameObject[] players;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }
    void Update()
    {
        if(enemy.isArrival)
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            foreach(GameObject p in players)
            {
                if(p.gameObject.activeSelf)
                {
                    enemy.target = p;
                    Debug.Log("HIT");
                    enemy.Hit(3f);
                }
            }
            enemy.isArrival = false;
        }
    }
}
