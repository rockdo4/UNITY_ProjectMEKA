using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public float time;
    float m_time;
    float m_time2;
    public float MoveSpeed = 10;
    public bool AbleHit;
    public float HitDelay;
    public GameObject m_hitObject;
    private GameObject m_makedObject;
    public string effectName;
    public float MaxLength;
    public float DestroyTime2;
    float m_scalefactor;

    private void Start()
    {
        m_scalefactor = VariousEffectsScene.m_gaph_scenesizefactor;
        m_time = Time.time;
        m_time2 = Time.time;
    }
    private void OnEnable()
    {
        m_scalefactor = VariousEffectsScene.m_gaph_scenesizefactor;
        m_time = Time.time;
        m_time2 = Time.time;
    }

    void LateUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed * m_scalefactor);
        if(AbleHit)
        { 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, MaxLength))
            {
                if (Time.time > m_time2 + HitDelay)
                {
                    m_time2 = Time.time;
                    HitObj(hit);
                }
            }
        }
    }

    void HitObj(RaycastHit hit)
    {
        m_makedObject = ObjectPoolManager.instance.GetGo(effectName);
        m_makedObject.GetComponent<PoolAble>().ReleaseObject(DestroyTime2);
    }
    

}
