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
        if (Player == null)
        {
            return; // Player가 null이면 메소드를 종료합니다.
        }
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
        if (Player != null)
        {
            
            //var flahObj = ObjectPoolManager.instance.GetGo(Player.GetComponent<PlayerController>().state.flashName);
            var flahObj = ObjectPoolManager.instance.GetGo(Player.GetComponent<EnemyController>().state.flashName);
            if(flahObj == null)
            {
                return;
            }
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
    }

    void FixedUpdate()
    {
        if (target.gameObject.activeInHierarchy)
        {
            transform.LookAt(new Vector3(target.position.x,target.position.y + 0.5f,target.position.z));
            rb.velocity = transform.forward * speed;
            pos = target.position;
           
        }
        else
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
        PlayerController pl = Player.GetComponent<PlayerController>();
        EnemyController en = Player.GetComponent<EnemyController>();

        if (other.CompareTag("EnemyCollider") && pl != null)
        {
            // 대상에 대미지 처리
            IAttackable attackable = target.GetComponentInParent<IAttackable>();
            
            attackable.OnAttack(damage);
            

            // 충돌 효과 생성
            CreateHitEffect(transform.position, transform.forward);

            // 트레일 제거
            RemoveTrails();
            ReleaseObject();
        }
        else if(other.CompareTag("PlayerCollider")&& en != null)
        {
            IAttackable attackable = target.GetComponentInParent<IAttackable>();
            Debug.Log($"{damage},{target.GetComponentInParent<PlayerController>().state.Hp}");
            attackable.OnAttack(damage);
            
            //attackable.OnAttack((Player.GetComponent<PlayerController>().state.damage + Player.GetComponent<PlayerController>().Rockpaperscissors() * 1f * 1f) - (target.GetComponentInParent<EnemyController>().state.amror + 1f) * 1f);


            CreateHitEffect(transform.position, transform.forward);

            
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
        if(Player.tag == "Player")
        {
            var hitInstance = ObjectPoolManager.instance.GetGo(Player.GetComponent<PlayerController>().state.hitName);
            if (hitInstance != null)
            {
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
        }
        else if(Player.tag == "Enemy")
        {
            var hitInstanceEn = ObjectPoolManager.instance.GetGo(Player.GetComponent<EnemyController>().state.hitName);
            if (hitInstanceEn != null)
            {
                hitInstanceEn.transform.position = position;
                hitInstanceEn.transform.rotation = rotation;
                hitInstanceEn.SetActive(false);
                hitInstanceEn.SetActive(true);
            }
            var hitPs = hitInstanceEn.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                hitInstanceEn.GetComponent<PoolAble>().ReleaseObject(hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstanceEn.transform.GetChild(0).GetComponent<ParticleSystem>();
                hitInstanceEn.GetComponent<PoolAble>().ReleaseObject(hitPsParts.main.duration);
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
