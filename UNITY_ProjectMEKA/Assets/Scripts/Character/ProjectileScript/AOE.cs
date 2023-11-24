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
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

        if (flash != null)
        {
            //�߻�ü ��ġ�� ���� �÷��� ȿ�� �ν��Ͻ�ȭ
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //���� ���� �ð��� ���� �÷��� ȿ�� �ı�
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

                //Destroy hit effects depending on particle Duration time���� ���� �ð��� ���� ���� ȿ�� �ı�
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
