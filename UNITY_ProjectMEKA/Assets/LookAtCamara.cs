using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamara : MonoBehaviour
{
    
    void Update()
    {
        Camera cam = Camera.main;
        transform.LookAt(cam.transform.position);
    }
}
