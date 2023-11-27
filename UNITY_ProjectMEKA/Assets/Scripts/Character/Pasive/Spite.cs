using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spite : MonoBehaviour
{
    //�� �нú긦 ���� ���ʹ� ���� ���� ĳ������ 
    //    �ܿ� ü���� 50% �̸��� �� 2���� ������� �ش�. 
    //    (�� ȿ���� 2�� ������ �Ӽ��� ��������� ����� �� ���� �������� ó���Ѵ�.)

    private EnemyController enemy;
    private float saveDamage;
    private float dubleDamage;
    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }
    private void Start()
    {
        saveDamage = enemy.state.damage;
        dubleDamage = enemy.state.damage * 2f;
    }
    private void Update()
    {
        if((enemy.target.GetComponent<PlayerController>().state.Hp/ enemy.target.GetComponent<PlayerController>().state.maxHp) <= 0.5f)
        {
            enemy.state.damage = dubleDamage;
        }
        else
        {
            enemy.state.damage = saveDamage;
        }
    }
}
