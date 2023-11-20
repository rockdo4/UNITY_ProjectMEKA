using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Attack
{
   public float Damage { get; private set; }
    public bool IsCritical { get; private set; }


    public Attack(float damage, bool Critical)
    {
        Damage = damage;
        IsCritical = Critical;
    }
}
