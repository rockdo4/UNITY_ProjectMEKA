using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowTarget : MonoBehaviour
{
    public Transform target;
    
    void Update()
    {
        if(target != null)
        {
            gameObject.transform.position = target.position;
        }
    }
}
