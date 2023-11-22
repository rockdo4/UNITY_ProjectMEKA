using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Waypoint : MonoBehaviour
{
    public Defines.GateType gateType;

    void Start()
    {
        foreach(Transform waypoint in transform)
        {
            waypoint.GetComponent<Renderer>().enabled = false;
        }
    }
}
