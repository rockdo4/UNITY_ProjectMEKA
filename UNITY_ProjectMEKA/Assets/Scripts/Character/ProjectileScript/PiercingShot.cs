using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PiercingShot : MonoBehaviour
{
    public float speed = 10f;
    public Transform target;
    public float damage;
    private Vector3 SavePos;
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
    private Vector3 direction;
    private Transform saveTarget;
    public Transform StartPos;
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
    //public void OnEnable()
    //{
    //    speed = maxSpeed;
    //    rb.constraints = RigidbodyConstraints.None;
    //    saveTarget = target;

        
    //}
    public void Init()
    {
        rb = GetComponent<Rigidbody>();
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
        saveTarget = target;

        gameObject.transform.position = StartPos.position;
    }
    void FixedUpdate()
    {
        
        //if (target != null && target.gameObject.activeSelf)
        //{
        //    direction = (target.position - transform.position).normalized;
        //    SavePos = target.position;
        //}

        
        //rb.velocity = direction * speed;
        rb.velocity = transform.forward * speed;

        timer += Time.deltaTime;
        if(timer > 1f)
        {
            timer = 0;
            ReleaseObject();
        }
    }
   
    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
           
    //        IAttackable dd = other.gameObject.GetComponent<IAttackable>();
    //        dd.OnAttack(damage);
            
    //        ContactPoint contact = other.contacts[0];
    //        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //        Vector3 pos = contact.point + contact.normal * hitOffset;

    //        //Spawn hit effect on collision충돌 시 스펀 적중 효과
    //        if (hit != null)
    //        {
    //            var hitInstance = Instantiate(hit, pos, rot);
    //            if (UseFirePointRotation)
    //            {
    //                hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);
    //            }
    //            else if (rotationOffset != Vector3.zero)
    //            {
    //                hitInstance.transform.rotation = Quaternion.Euler(rotationOffset);
    //            }
    //            else
    //            {
    //                hitInstance.transform.LookAt(contact.point + contact.normal);
    //            }

    //            //Destroy hit effects depending on particle Duration time입자 지속 시간에 따른 적중 효과 파괴
    //            var hitPs = hitInstance.GetComponent<ParticleSystem>();
    //            if (hitPs != null)
    //            {
    //                Destroy(hitInstance, hitPs.main.duration);
    //            }
    //            else
    //            {
    //                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
    //                Destroy(hitInstance, hitPsParts.main.duration);
    //            }
    //        }

    //        //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
    //        //섬에 있는 발사체에서 트레일을 제거하거나 부드럽게 제거합니다. 분리된 요소에는 "AutoDestroying 스크립트"가 있어야 합니다
    //        foreach (var detachedPrefab in Detached)
    //        {
    //            if (detachedPrefab != null)
    //            {
    //                detachedPrefab.transform.parent = null;
    //                Destroy(detachedPrefab, 1);
    //            }
    //        }
    //        //Destroy projectile on collision
    //        //Destroy(gameObject);

    //        //Debug.Log(gameObject.name, gameObject);

    //        //ReleaseObject();

    //    }
    //    else if (other.gameObject.CompareTag("Wall"))
    //    {
    //        ReleaseObject();
    //    }
    //}
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 대상에 대미지 처리
            IAttackable attackable = other.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.OnAttack(damage);
            }

            // 충돌 효과 생성
            CreateHitEffect(transform.position, transform.forward);

            // 트레일 제거
            RemoveTrails();
        }
        else if (other.CompareTag("Wall"))
        {
            ReleaseObject();
        }
    }

    private void CreateHitEffect(Vector3 position, Vector3 direction)
    {
        if (hit != null)
        {
            Quaternion rotation = UseFirePointRotation ?
                                  Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180f, 0) :
                                  Quaternion.identity;

            if (rotationOffset != Vector3.zero)
            {
                rotation *= Quaternion.Euler(rotationOffset);
            }

            var hitInstance = Instantiate(hit, position, rotation);
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
    }

    private void RemoveTrails()
    {
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
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
