using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChainAttack : MonoBehaviour
{
    public Transform target;
    public float damage;
    public bool isReleased = false;
    private GameObject[] enemys;
    private float timer;
    private float sTimer;
    private bool isOne = false;
    public DamageDeleteTime ddt;
    public void Start()
    {
        ddt = GetComponent<DamageDeleteTime>();
    }
    public void Update()
    {
        
        timer += Time.deltaTime;
        if(timer > ddt.DamageTime && !isOne)
        {
            isOne = true;
            Explosion();
        }
        else
        {
            transform.position = target.position;
        }
        sTimer += Time.deltaTime;
        if(sTimer > ddt.deleteTime)
        {
            sTimer = 0;
            timer = 0;
            isOne = false;
            ReleaseObject();
        }

    }

    public void Explosion()
    {
        Vector3 os = transform.position;
        Vector3Int CurrentGridPos = new Vector3Int(Mathf.FloorToInt(os.x), Mathf.FloorToInt(os.y), Mathf.FloorToInt(os.z));

        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemys)
        {
            if (CurrentGridPos == enemy.GetComponent<EnemyController>().CurrentGridPos&&enemy.activeSelf)
            {
                IAttackable aoeDamage = enemy.GetComponent<IAttackable>();
                if (aoeDamage != null)
                {
                    aoeDamage.OnAttack(damage);//추후 주변 몬스터의 개수만큼 배율변경되도록 수정
                }
            }
        }
        
    }

    private void ReleaseObject()
    {
        if (isReleased)
        {
            return;
        }
        isReleased = true;
        GetComponent<PoolAble>().ReleaseObject();
    }
    public void ResetState()
    {
        isReleased = false;
    }
}
