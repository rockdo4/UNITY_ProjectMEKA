using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour, IAttackable
{ 
    private CharacterState state;
    private PlayerState player;
    private EnemyState enemy;

    private void Awake()
    {
        //state = GetComponent<CharacterState>();
        player = GetComponent<PlayerState>();
        if(player == null)
        {
            enemy = GetComponent<EnemyState>();

        }
    }
    public void OnAttack(float damage)
    {
       
        //state.Hp -= damage - state.amror;
        if(player != null)
        {
            player.Hp -= damage - player.amror;
        }
        else
        {
            enemy.Hp -= damage - enemy.amror;
        }

    }
   public void OnHealing(float healValue)
    {
        if(player!= null) 
        {
            player.Hp += healValue;
            if (player.Hp >= player.maxHp)
            {
                player.Hp = player.maxHp;
            }
        }
        else
        {
            enemy.Hp += healValue;
            if (enemy.Hp >= enemy.maxHp)
            {
                enemy.Hp = enemy.maxHp;
            }
        }


        
    }

}
