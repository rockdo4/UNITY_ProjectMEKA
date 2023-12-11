using UnityEngine;

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
    //public GameObject hit;
    //public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    private float timer;
    public bool isReleased = false;
    private Vector3 direction;
    private Transform saveTarget;
    public Transform StartPos;
    public GameObject Player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void OnEnable()
    {
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
        saveTarget = target;
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
        if (Player != null)
        {
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
    }
    public void Init()
    {
        //rb = GetComponent<Rigidbody>();
        speed = maxSpeed;
        rb.constraints = RigidbodyConstraints.None;
        saveTarget = target;
        //gameObject.transform.position = StartPos.position;
    }
    void FixedUpdate()
    {
       
        rb.velocity = transform.forward * speed;

        timer += Time.deltaTime;
        if(timer > 1f)
        {
            timer = 0;
            ReleaseObject();
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
        }
        else if (other.CompareTag("PlayerCollider") && en != null)
        {
            IAttackable attackable = target.GetComponentInParent<IAttackable>();

            attackable.OnAttack(damage);


            CreateHitEffect(transform.position, transform.forward);


            RemoveTrails();
        }

        
        //else if (other.CompareTag("Wall"))
        //{
        //    ReleaseObject();
        //}
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
        if (Player.tag == "Player")
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
        else if (Player.tag == "Enemy")
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
