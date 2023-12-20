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
    [SerializeField, Header("��ų Ÿ��")]
    public Defines.SkillType skillType;

    [HideInInspector]
    public bool isSkillUsing;

    [SerializeField, Header("�⺻ ���� ������ �����ϴ°�")]
    public bool isRangeOut;

    // 12.20, �����, �߰�
    public List<GameObject> targetList = new List<GameObject>();

    public abstract void UseSkill();
}
