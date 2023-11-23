using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 15f;
    public Transform target;
    public float damage;
    public Vector3 pos;
    private bool isHit = false;
    public float maxSpeed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    private float timer;
    public bool isReleased = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
        if (flash != null)
        {
            //발사체 위치에 대한 플래시 효과 인스턴스화
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //입자 지속 시간에 따라 플래시 효과 파괴
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        //Destroy(gameObject,5);

    }

    private void OnEnable()
    {
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
    }

    void FixedUpdate()
    {
        if (speed != 0&& target.gameObject.activeSelf)
        {
            transform.LookAt(target.position);
            rb.velocity = transform.forward * speed;
            pos = target.position;
           
        }
        else if(!target.gameObject.activeSelf)
        {
            rb.velocity = transform.forward * speed;
            timer += Time.deltaTime;
            if(timer > 1f)
            {
                ReleaseObject();
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IAttackable dd = target.GetComponent<IAttackable>();
            //if (collision.gameObject == target.gameObject)
            //{
            //    isHit = true;
            //    dd.OnAttack(damage);
            //}
            dd.OnAttack(damage);
            //Lock all axes movement and rotation모든 축 이동 및 회전 잠금
            rb.constraints = RigidbodyConstraints.FreezeAll;
            speed = 0;

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * hitOffset;

            //Spawn hit effect on collision충돌 시 스펀 적중 효과
            if (hit != null)
            {
                var hitInstance = Instantiate(hit, pos, rot);
                if (UseFirePointRotation)
                {
                    hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); 
                }
                else if (rotationOffset != Vector3.zero) 
                { 
                    hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); 
                }
                else
                {
                    hitInstance.transform.LookAt(contact.point + contact.normal); 
                }

                //Destroy hit effects depending on particle Duration time입자 지속 시간에 따른 적중 효과 파괴
                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }

            //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
            //섬에 있는 발사체에서 트레일을 제거하거나 부드럽게 제거합니다. 분리된 요소에는 "AutoDestroying 스크립트"가 있어야 합니다
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    detachedPrefab.transform.parent = null;
                    Destroy(detachedPrefab, 1);
                }
            }
            //Destroy projectile on collision
            //Destroy(gameObject);

            //Debug.Log(gameObject.name, gameObject);

            ReleaseObject();

        }
    }
    private void ReleaseObject()
    {
        if(isReleased)
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
