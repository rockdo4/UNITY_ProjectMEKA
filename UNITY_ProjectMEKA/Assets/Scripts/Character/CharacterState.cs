using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public enum Property
    {
        Prime,
        Edila,
        Grieve,
    }
    public enum Occupation
    {
        Guardian,
        Striker,
        Castor,
        Hunter,
        Supporters,
    }
    public Property property;
    public Occupation occupation;

    public float maxHp;
    public float amror;
    public int ID;
    public float cooldown;
    public float range;
    public float minDamage;
    public float damage;
    public float criticalChance; // 0.0 == 0 % ,1.0 == 100%
    public float criticalMultiplier;
    public float Hp;
    public float attackDelay;


    private void Awake()
    {
        Hp = maxHp;
    }
   
}
