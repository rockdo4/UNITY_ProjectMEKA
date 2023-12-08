using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CameraManager : MonoBehaviour
{
    //초기 위치값 / temp 위치값(currentPlayer 중심 위치)
    public float minDistance = 10f;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 5f;
    
    private StageManager stageManager;

    public Quaternion initRotation;
    public Vector3 initPos;
    public Transform target;

    private bool once;

    //현재 윈도우 모드가 setting일 때, currentPlayer를 보도록 함
    //currentPlayer를 중심으로 따라감

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
            // 각도 러프
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 2f);

            // 무빙 러프
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            var targetPosition = target.position - directionToTarget * minDistance;

            if (Vector3.Distance(transform.position, target.position) > minDistance)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, zoomSpeed * Time.deltaTime * 2f);
            }            
            // 줌 다 됐을 때
            //once = true;
        }
        else if(!(stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.Setting)/* && once*/)
        {
            // 각도 러프
            transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, rotationSpeed * Time.deltaTime);

            // 무빙 러프
            transform.position = initPos;

            // 처음 위치,로테이션으로 다 돌아갔을 때
            //once = false;
        }
    }
}
