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
        
        //���ӽð����ȸ� ������ ���������Ѵ�
        //���ӽð��� ������ ������� ���ư�����
            
    }

    public override void UseSkill()
    {
        //isUsingSkill = true;
        //�ӻ罺ų ����
        Debug.Log("���ī�� �ӻ� �ߵ�");
    }
}
