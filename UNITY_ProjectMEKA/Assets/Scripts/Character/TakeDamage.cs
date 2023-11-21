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
        state.Hp -= damage;


    }

}
