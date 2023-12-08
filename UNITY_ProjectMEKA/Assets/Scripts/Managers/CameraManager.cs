using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CameraManager : MonoBehaviour
{
    //�ʱ� ��ġ�� / temp ��ġ��(currentPlayer �߽� ��ġ)
    public float minDistance = 10f;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 5f;
    
    private StageManager stageManager;

    public Quaternion initRotation;
    public Vector3 initPos;
    public Transform target;

    private bool once;

    //���� ������ ��尡 setting�� ��, currentPlayer�� ������ ��
    //currentPlayer�� �߽����� ����

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        initPos = transform.position;
        initRotation = transform.rotation;
    }

    private void Update()
    {
        if(stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.Setting /*&& !once*/)
        {
            if(stageManager.currentPlayer == null)
            {
                return;
            }

            target = stageManager.currentPlayer.transform;
            // ���� ����
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 2f);

            // ���� ����
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            var targetPosition = target.position - directionToTarget * minDistance;

            if (Vector3.Distance(transform.position, target.position) > minDistance)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, zoomSpeed * Time.deltaTime * 2f);
            }            
            // �� �� ���� ��
            //once = true;
        }
        else if(!(stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.Setting)/* && once*/)
        {
            // ���� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, rotationSpeed * Time.deltaTime);

            // ���� ����
            transform.position = initPos;

            // ó�� ��ġ,�����̼����� �� ���ư��� ��
            //once = false;
        }
    }
}
