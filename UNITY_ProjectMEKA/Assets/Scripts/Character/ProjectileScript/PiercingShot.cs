using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingShot : MonoBehaviour
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

        }
        if (target == null)
        {
            transform.position += pos.normalized * speed * Time.deltaTime;
            //Destroy(gameObject, 1f);
        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        target = null;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage dd = collision.gameObject.GetComponent<TakeDamage>();
            dd.OnAttack(damage);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            TakeDamage dd = other.GetComponent<TakeDamage>();
            dd.OnAttack(damage);
        }
        else if(other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

   
}
