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
    private Rigidbody rb;
    public GameObject[] Detached;
    private float timer;
    public bool isReleased = false;
    public GameObject Player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
   
    private void OnEnable()
    {
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
       
        var flahObj = ObjectPoolManager.instance.GetGo(Player.GetComponent<PlayerController>().state.flashName);
        flahObj.transform.position = transform.position;
        flahObj.transform.forward = gameObject.transform.forward;
        var flashPs = flahObj.GetComponent<ParticleSystem>();
        flahObj.SetActive(false);
        flahObj.SetActive(true);
        if (flashPs != null)
        {
            flashPs.GetComponent<PoolAble>().ReleaseObject(flashPs.main.duration);
        }
        else
        {

            var flashPsParts = flahObj.transform.GetChild(0).GetComponent<ParticleSystem>();
            flashPs.GetComponent<PoolAble>().ReleaseObject(flashPsParts.main.duration);
        }
        
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 대상에 대미지 처리
            IAttackable attackable = target.GetComponent<IAttackable>();
            
            attackable.OnAttack(damage);
            

            // 충돌 효과 생성
            CreateHitEffect(transform.position, transform.forward);

            // 트레일 제거
            RemoveTrails();
            ReleaseObject();
        }
       
    }

    private void CreateHitEffect(Vector3 position, Vector3 direction)
    {

        Quaternion rotation = UseFirePointRotation ?
                                Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180f, 0) :
                                Quaternion.identity;

        if (rotationOffset != Vector3.zero)
        {
            rotation *= Quaternion.Euler(rotationOffset);
        }

        //var hitInstance = Instantiate(hit, position, rotation);
        var hitInstance = ObjectPoolManager.instance.GetGo(Player.GetComponent<PlayerController>().state.hitName);
        hitInstance.transform.position = position;
        hitInstance.transform.rotation = rotation;
        hitInstance.SetActive(false);
        hitInstance.SetActive(true);
        var hitPs = hitInstance.GetComponent<ParticleSystem>();
        if (hitPs != null)
        {
            hitInstance.GetComponent<PoolAble>().ReleaseObject(hitPs.main.duration);
        }
        else
        {
            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
            hitInstance.GetComponent<PoolAble>().ReleaseObject(hitPsParts.main.duration);
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
    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        IAttackable dd = target.GetComponent<IAttackable>();
            
    //        dd.OnAttack(damage);
    //        //Lock all axes movement and rotation모든 축 이동 및 회전 잠금
    //        rb.constraints = RigidbodyConstraints.FreezeAll;
    //        speed = 0;

    //        ContactPoint contact = collision.contacts[0];
    //        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //        Vector3 pos = contact.point + contact.normal * hitOffset;

    //        //Spawn hit effect on collision충돌 시 스펀 적중 효과
            
    //        //var hitObj = Instantiate(hit, pos, rot);
    //        var hitObj = ObjectPoolManager.instance.GetGo(Player.GetComponent<PlayerController>().state.hitName);
    //        hitObj.transform.position = pos;
    //        hitObj.transform.rotation = rot;

    //        if (UseFirePointRotation)
    //        {
    //            hitObj.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); 
    //        }
    //        else if (rotationOffset != Vector3.zero) 
    //        { 
    //            hitObj.transform.rotation = Quaternion.Euler(rotationOffset); 
    //        }
    //        else
    //        {
    //            hitObj.transform.LookAt(contact.point + contact.normal); 
    //        }
    //        hitObj.SetActive(false);
    //        hitObj.SetActive(true);

    //        //Destroy hit effects depending on particle Duration time입자 지속 시간에 따른 적중 효과 파괴
    //        var hitPs = hitObj.GetComponent<ParticleSystem>();
    //        if (hitPs != null)
    //        {
    //            hitObj.GetComponent<PoolAble>().ReleaseObject(hitPs.main.duration);
    //        }
    //        else
    //        {
    //            var hitPsParts = hitObj.transform.GetChild(0).GetComponent<ParticleSystem>();
    //            hitObj.GetComponent<PoolAble>().ReleaseObject(hitPsParts.main.duration);
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

    //        ReleaseObject();

    //    }
    //}
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
