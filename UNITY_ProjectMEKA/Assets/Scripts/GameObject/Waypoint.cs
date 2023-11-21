using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateType
{
    Gate1,
    Gate2,
    Gate3,
    Gate4,
}

public class Waypoint : MonoBehaviour
{
    public GateType gateType;

    void Start()
    {
        foreach(Transform waypoint in transform)
        {
            waypoint.GetComponent<Renderer>().enabled = false;
        }
    }
}
