using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [SerializeField, Header("��ų �ڽ�Ʈ")]//p
    public float skillCost;

    [SerializeField, Header("��ų ��Ÿ��")]//p
    public float skillCoolTime;

    //� ��ų���� ǥ��: �ڵ�, ���, �������� ����, �������� ���� 

   

    public abstract void UseSkill();
}
