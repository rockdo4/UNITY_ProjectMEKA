using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outlander : MonoBehaviour
{
    EnemyController enemy;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    private void Start()
    {
        enemy.state.property = Defines.Property.None;
    }
    
}
