using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Waypoint : MonoBehaviour
{
    void Start()
    {
        foreach(Transform waypoint in transform)
        {
            waypoint.GetComponent<Renderer>().enabled = false;
        }
    }
}
