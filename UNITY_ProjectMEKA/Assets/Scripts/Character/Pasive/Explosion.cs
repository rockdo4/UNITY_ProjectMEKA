using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Explosion : MonoBehaviour
{
    float timer = 20f;
    EnemyController enemy;
    GameObject[] players;
    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Boom();
        }
    }
    void Boom()
    {
        
        players = GameObject.FindGameObjectsWithTag("Player");

        Vector3Int playerGridPos = enemy.CurrentGridPos;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int checkPos = new Vector3Int(playerGridPos.x + x, playerGridPos.y, playerGridPos.z + z);

                    foreach (var player in players)
                    {
                        PlayerController pc = player.GetComponent<PlayerController>();
                        if (pc != null && pc.CurrentGridPos == checkPos)
                        {
                            enemy.target = player;
                            enemy.Hit(3f);

                        }
                    }
                }
            }
        }
        //Destroy(gameObject);
        enemy.state.Hp = 0; 


    }
    

}
