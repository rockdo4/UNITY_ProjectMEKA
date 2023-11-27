using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackRange.asset", menuName = "AttackRange/Range")]
public class AttackRange : ScriptableObject
{
    public int[,] range = new int[5,5];
    
}
