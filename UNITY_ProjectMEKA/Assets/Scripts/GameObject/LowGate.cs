using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGate : GateController
{
    protected override void Awake()
    {
        base.Awake();
        if (waypoints == null)
        {
            waypoints = new Transform[1];
        }
        waypoints[waypoints.Length - 1] = house.transform;
    }

    protected override void FixedUpdate()
    {
        base .FixedUpdate();
    }
}
