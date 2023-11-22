using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    public float speed = 10f;
    public Transform target;
    public float damage;
    private Vector3 pos;
    private GameObject[] enemys;
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            pos = direction;
            transform.position += direction.normalized * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                TakeDamage dd = target.GetComponent<TakeDamage>();
                dd.OnAttack(damage);
                //HitTarget();
                Vector3 pos = transform.position;
                Vector3Int CurrentGridPos = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));

                enemys = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (var enemy in enemys) 
                {
                    if(CurrentGridPos == enemy.GetComponent<EnemyController>().CurrentGridPos)
                    {
                        IAttackable aoeDamage = enemy.GetComponent<IAttackable>();
                        if(aoeDamage != null && enemy.gameObject != target.gameObject)
                        {
                            aoeDamage.OnAttack(damage);//추후 주변 몬스터의 개수만큼 배율변경되도록 수정
                        }
                    }
                }

            }
        }
        if (target == null)
        {
            transform.position += pos.normalized * speed * Time.deltaTime;
            Destroy(gameObject, 1f);
        }

    }

    void HitTarget()
    {
        
        Destroy(gameObject); 
    }
}
