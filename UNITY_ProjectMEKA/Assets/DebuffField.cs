using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffField : MonoBehaviour
{
    public enum DeBuffType
    {
        DecreasedMaxHealth,
        DecreasedAttackPower,
        DecreasedDefense,
    }

    public List<GameObject> rangeInPlayers = new List<GameObject>();
    public DeBuffType type;
    public ParticleSystemRenderer particleSystem;
    public Color newColor;
    void Start()
    {
        type = (DeBuffType)Random.Range(0, 3);
        //particleSystem = GetComponentInChildren<ParticleSystem>();
        
        switch(type)
        {
            case DeBuffType.DecreasedMaxHealth:
                newColor = Color.red;
                break;
            case DeBuffType.DecreasedAttackPower:
                newColor = Color.gray;
                break;
            case DeBuffType.DecreasedDefense:
                newColor = Color.blue;
                break;
        }
       
        ParticleSystemRenderer[] childRenderers = GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (ParticleSystemRenderer renderer in childRenderers)
        {
            if (renderer.material.HasProperty("_InnerColor")) // 속성 이름이 "_InnerColor"인지 확인
            {
                renderer.material.SetColor("_InnerColor", newColor);
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            //Debug.Log(other, other);
            var ot = other.GetComponentInParent<Transform>();
            var player = other.GetComponentInParent<PlayerController>();
            if (!rangeInPlayers.Contains(ot.gameObject))
            {
                switch(type)
                {
                    case DeBuffType.DecreasedMaxHealth:
                        //캐릭터의 최대 체력을 15% 감소시킨다
                        float sum = player.state.maxHp * (15f / 100f);
                        player.state.maxHp -= sum;
                        player.state.Hp -= sum;

                        if (player.state.Hp < 0)
                        {
                            player.state.Hp = 0;
                        }
                        break;
                    case DeBuffType.DecreasedAttackPower:
                        //캐릭터의 공격력을 10% 감소시킨다
                        float pw = player.state.damage * (10f / 100f);
                        player.state.damage -= pw;
                        
                        if (player.state.damage < 0)
                        {
                            player.state.damage = 1;
                        }
                        break;
                    case DeBuffType.DecreasedDefense:
                        //캐릭터의 방어력을 10% 감소시킨다
                        float de = player.state.armor * (10f / 100f);
                        player.state.armor -= de;

                        if (player.state.armor < 0)
                        {
                            player.state.armor = 0;
                        }
                        break;
                }
                rangeInPlayers.Add(ot.gameObject);
                var obj = other.GetComponentInParent<CanDie>();
                obj.action.AddListener(() =>
                {
                    rangeInPlayers.Remove(ot.gameObject);
                });
                
            }
        }
    }
}
