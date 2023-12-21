using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class LowGate : GateController
{
    private void Awake()
    {
        base.Awake();
        spawnInitPos = new Vector3(transform.position.x, 0.25f, transform.position.z);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
