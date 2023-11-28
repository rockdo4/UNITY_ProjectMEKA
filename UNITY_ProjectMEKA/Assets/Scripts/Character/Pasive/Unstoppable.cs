using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unstoppable : MonoBehaviour
{
    EnemyController enemy;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    void Start()
    {
        enemy.state.isBlock = true;
    }

    
}
