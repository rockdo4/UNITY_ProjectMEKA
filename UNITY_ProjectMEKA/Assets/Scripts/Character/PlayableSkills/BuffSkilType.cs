using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkilType : SkillBase
{
    
    //�ڽſ��Ծ��°�
    //�����ΰ� �����ΰ�
    //� ������ �����ϴ°�
    //����Ʈ�� ����� ���°�
    //�������ΰ�
    //���ӽð�
    //�Ҹ� �ڽ�Ʈ
    //��Ÿ��

    //-����:����Ʈ ��� ��, 12�ʵ��� ���ݷ��� 15% �����Ѵ�.�ڽ�
    //-�̻级��:����Ʈ ��� ��, �����ð����� ���� ���� ������ �Ʊ��� ���ݷ� ���
    //-���ī��: ���ݼӵ� ����
    //-�޸���: ����Ʈ ��� ��, 12�ʵ��� ȸ�� �ӵ��� 1.5�� �����Ѵ� �ڽ�
    public enum buffType
    {
        damage,
        attackSpeed,
    }

    [SerializeField, Header("��ų ���ӽð�")]
    public float skillDuration;

    [SerializeField, Header("�ڽſ��� ���°�")]
    public bool isMe;

    [SerializeField, Header("�����ΰ�")]
    public bool isSingle;

    [SerializeField, Header("����Ʈ �̸��� �����ΰ�")]
    public string effectName;

    [SerializeField, Header("�����ؼ� ����ϴ°�")]
    public bool isChoice;

    [SerializeField, Header("� �ɷ�ġ�� �����ϴ°�")]
    public buffType type;

    private PlayerController player;
    private float timer;
    private float duration;


    private void Start()
    {
        player = GetComponent<PlayerController>();
        timer = skillDuration;
        duration = 0f;
    }
    private void Update()
    {
        
    }
    public override void UseSkill()
    {
    }
}
