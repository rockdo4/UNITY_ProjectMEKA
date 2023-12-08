using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CameraManager : MonoBehaviour
{
    //�ʱ� ��ġ�� / temp ��ġ��(currentPlayer �߽� ��ġ)
    private StageManager stageManager;

    public Vector3 initPos;
    public Vector3 tempPos;

    //currentPlayer�� ��ġ �Ϸ� ���� && ���� ������ ��尡 setting�� ��
    //currentPlayer�� �߽����� ����

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        initPos = transform.position;
    }

    private void Update()
    {
        if(stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.Setting)
        {
            transform.position = tempPos;
        }
    }
}
