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
            //damage -= player.armor;
            //if (damage <= 0)
            //{
            //    damage = 5;
            //}
            if(player.shield > 0f)
            {
                player.shield -= damage;
            }
            else
            {
                player.Hp -= damage;
            }
        }
        else
        {
            //damage -= enemy.armor;
            //if (damage <= 0)
            //{
            //    damage = 5;
            //}
            if(enemy.shield > 0f)
            {
                //enemy.shield -= damage;
                if(damage <=0f)
                {
                    damage = 1f;
                }
                if (damage > enemy.shield)
                {
                    float remainingDamage = damage - enemy.shield;
                    enemy.shield = 0f;
                    enemy.Hp -= remainingDamage;
                }
                else
                {
                    enemy.shield -= damage;
                }
            }
            else
            {
                enemy.Hp -= damage;
            }
            
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
