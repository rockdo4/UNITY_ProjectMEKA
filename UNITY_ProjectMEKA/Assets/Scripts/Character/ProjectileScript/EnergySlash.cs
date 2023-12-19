using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySlash : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 10f;
    public GameObject player;
    private float timer;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = player.transform.forward * speed;
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            GetComponent<PoolAble>().ReleaseObject();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCollider"))
        {
            IAttackable dd = other.GetComponentInParent<IAttackable>();
            //dd.OnAttack((player.GetComponent<PlayerController>().state.damage + player.GetComponent<PlayerController>().Rockpaperscissors() * 1f * 1f) - (other.GetComponentInParent<EnemyController>().state.amror + 1f) * 1f);
            var pl = player.GetComponent<PlayerController>();

            
            dd.OnAttack(pl.state.damage);
        }
        
    }
}
