using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    public float speed = 10f;
    public Transform target;
    public float damage;
    private Vector3 pos;
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
                HitTarget();
                //��������
                Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
                foreach (Collider co in colliders)
                {
                    if (co.CompareTag("Enemy"))
                    {
                        TakeDamage areaDamage = co.GetComponent<TakeDamage>();
                        if (areaDamage != null && co.gameObject != target.gameObject)
                        {
                            areaDamage.OnAttack(damage * 0.7f); 
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
        // Ÿ�ٿ� �������� ���� ���� ����, ��: Ÿ�ٿ� ���� �ֱ�
        Destroy(gameObject); // �߻�ü ����
    }
}
