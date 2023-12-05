using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolAble : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    public void ReleaseObject()
    {
        //Debug.Log("release");
        Pool.Release(gameObject);
    }

    public void ReleaseObject(float timer)
    {
        Invoke("ReleaseObject", timer);
    }
}
