using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour, IAttackable
{ 
    private CharacterState state;

    private void Awake()
    {
        state = GetComponent<CharacterState>();
    }
    public void OnAttack(float damage)
    {
       
        state.Hp -= damage - state.amror;

    }
   public void OnHealing(float healValue)
    {
        state.Hp += healValue;
        if (state.Hp >= state.maxHp)
        {
            state.Hp = state.maxHp;
        }
    }

}
