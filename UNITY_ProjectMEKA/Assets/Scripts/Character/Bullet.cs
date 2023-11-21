using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
            }
        }
        if(target == null)
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
