using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot : SkillBase
{
    public int skillCost = 30;
    public float coolTime = 20f;
    public float duration = 15f;
    private float timer;
    private bool isUsingSkill = false;
    private PlayerController player;


    public void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void Update()
    {
        if(isUsingSkill)
        {
            timer += Time.deltaTime;
            if (timer <= duration)
            {
                if(player.currentState == PlayerController.CharacterStates.Attack)
                {
                    //player.Fire();
                }
                
            }
            else if(timer >duration)
            {
                isUsingSkill = false;
            }
        }
        
        //지속시간동안만 공속이 빨라져야한다
        //지속시간이 끝나면 원래대로 돌아가야함
            
    }

    public override void UseSkill()
    {
        //isUsingSkill = true;
        //속사스킬 구현
        Debug.Log("쿠로카미 속사 발동");
    }
}
