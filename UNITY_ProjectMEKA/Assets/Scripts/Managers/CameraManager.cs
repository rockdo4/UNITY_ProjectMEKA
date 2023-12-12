using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CameraManager : MonoBehaviour
{
    public float minDistance;
    public float rotationSpeed;
    public float zoomSpeed;
    
    private StageManager stageManager;

    private Quaternion initRotation;
    private Vector3 initPos;
    private Transform target;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        initPos = transform.position;
        initRotation = transform.rotation;
    }

    private void Update()
    {
        if(stageManager.ingameStageUIManager.windowMode == WindowMode.Setting)
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
        }
        else if(!(stageManager.ingameStageUIManager.windowMode == WindowMode.Setting))
        {
            // 각도 러프
            transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, rotationSpeed * Time.deltaTime);

            // 무빙 러프
            transform.position = Vector3.Lerp(transform.position, initPos, zoomSpeed * Time.deltaTime * 2f);
        }
    }
}
