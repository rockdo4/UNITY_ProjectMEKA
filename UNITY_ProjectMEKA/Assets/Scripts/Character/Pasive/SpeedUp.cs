using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemyController enemy;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }
    void Start()
    {
        float speed = enemy.state.speed * 2.0f;
        enemy.state.speed = speed;
    }

    
}
