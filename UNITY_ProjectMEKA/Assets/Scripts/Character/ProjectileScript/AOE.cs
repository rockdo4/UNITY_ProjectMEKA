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
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    private float timer;
    public bool isReleased = false;
    public float maxSpeed = 15f;
    public GameObject Player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
        if (flash != null)
        {
            Debug.Log("P Not Null");
            var flahObj = ObjectPoolManager.instance.GetGo(Player.GetComponent<PlayerController>().state.flashName);
            flahObj.transform.position = transform.position;
            flahObj.transform.forward = gameObject.transform.forward;
            var flashPs = flahObj.GetComponent<ParticleSystem>();
            flahObj.SetActive(false);
            flahObj.SetActive(true);
            if (flashPs != null)
            {
                Debug.Log("Returne Not Null");
                flashPs.GetComponent<PoolAble>().ReleaseObject(flashPs.main.duration);
            }
            else
            {
                Debug.Log("Returne Null");

                var flashPsParts = flahObj.transform.GetChild(0).GetComponent<ParticleSystem>();
                flashPs.GetComponent<PoolAble>().ReleaseObject(flashPsParts.main.duration);
            }
        }
    }
    void FixedUpdate()
    {
        if (speed != 0 && target.gameObject.activeSelf)
        {
            transform.LookAt(target.position);
            rb.velocity = transform.forward * speed;
            pos = target.position;

        }
        else if (!target.gameObject.activeSelf)
        {
            rb.velocity = transform.forward * speed;
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                ReleaseObject();
            }
        }
    }
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            pos = direction;
            transform.position += direction.normalized * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                
            }
        }
        if (target == null)
        {
            transform.position += pos.normalized * speed * Time.deltaTime;
            //Destroy(gameObject, 1f);
        }
        if(!target.gameObject.activeSelf)
        {
            ReleaseObject();
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IAttackable dd = target.GetComponent<IAttackable>();
            dd.OnAttack(damage);
            //HitTarget();
            Vector3 os = transform.position;
            Vector3Int CurrentGridPos = new Vector3Int(Mathf.FloorToInt(os.x), Mathf.FloorToInt(os.y), Mathf.FloorToInt(os.z));

            enemys = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var enemy in enemys)
            {
                if (CurrentGridPos == enemy.GetComponent<EnemyController>().CurrentGridPos)
                {
                    IAttackable aoeDamage = enemy.GetComponent<IAttackable>();
                    if (aoeDamage != null && enemy.gameObject != target.gameObject)
                    {
                        aoeDamage.OnAttack(damage);//���� �ֺ� ������ ������ŭ ��������ǵ��� ����
                    }
                }
            }

            
            //Lock all axes movement and rotation��� �� �̵� �� ȸ�� ���
            rb.constraints = RigidbodyConstraints.FreezeAll;
            speed = 0;

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * hitOffset;

            //Spawn hit effect on collision�浹 �� ���� ���� ȿ��
            if (hit != null)
            {
                //var hitInstance = Instantiate(hit, pos, rot);

                var hitObj = ObjectPoolManager.instance.GetGo(Player.GetComponent<PlayerController>().state.hitName);
                hitObj.transform.position = pos;
                hitObj.transform.rotation = rot;
                if (UseFirePointRotation)
                {
                    hitObj.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);
                }
                else if (rotationOffset != Vector3.zero)
                {
                    hitObj.transform.rotation = Quaternion.Euler(rotationOffset);
                }
                else
                {
                    hitObj.transform.LookAt(contact.point + contact.normal);
                }
                hitObj.SetActive(false);
                hitObj.SetActive(true);

                //Destroy hit effects depending on particle Duration time���� ���� �ð��� ���� ���� ȿ�� �ı�
                var hitPs = hitObj.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    //Destroy(hitObj, hitPs.main.duration);
                    hitObj.GetComponent<PoolAble>().ReleaseObject(hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitObj.transform.GetChild(0).GetComponent<ParticleSystem>();
                    //Destroy(hitObj, hitPsParts.main.duration);
                    hitObj.GetComponent<PoolAble>().ReleaseObject(hitPsParts.main.duration);
                }
            }

            //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
            //���� �ִ� �߻�ü���� Ʈ������ �����ϰų� �ε巴�� �����մϴ�. �и��� ��ҿ��� "AutoDestroying ��ũ��Ʈ"�� �־�� �մϴ�
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
