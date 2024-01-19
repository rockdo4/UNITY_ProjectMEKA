using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class EnemyState : CharacterState
{
    
    [SerializeField, Header("�� �нú� ����")]//e
    public List<Passive> passive;

    [SerializeField, Header("�� Ÿ�� ����")]//e
    public Defines.EnemyType enemyType;

   
    // 11.22, �����, �� �̵��� �ʿ�
    [SerializeField, Header("�̵��ӷ� ����,�� �̵��� �ʿ�")]//e
    public float speed;

    [SerializeField, Header("�� ������, ������ ����(��üũ ������)")]//e
    public bool isFly;

    [SerializeField, Header("������?")]
    public bool isMovingShot;
    
    public bool isBlock = true;

}
