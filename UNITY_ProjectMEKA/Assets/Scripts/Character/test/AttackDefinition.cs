//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
//public class AttackDefinition : ScriptableObject
//{
//    public float cooldown;
//    public float range;
//    public float minDamage;
//    public float maxDamage;
//    public float criticalChance; // 0.0 -> 0 % ,1.0 -> 100%
//    public float criticalMultiplier;

//    public Attack CreatAttack(CharacterStat attacker, CharacterStat defender)
//    {
//        float damage = attacker.damage;
//        damage += Random.Range(minDamage, maxDamage);

//        bool critical = Random.value < criticalChance; //Random.value 는 0.0 ~ 1.0 사이 값 

//        if (critical)
//        {
//            damage *= criticalMultiplier;
//        }
//        if (defender != null)
//        {
//            damage -= defender.amror;
//        }
//        return new Attack((int)damage, critical);
//    }

//    public virtual void ExeciteAttack(GameObject attacker, GameObject defender)
//    {

//    }
//}
