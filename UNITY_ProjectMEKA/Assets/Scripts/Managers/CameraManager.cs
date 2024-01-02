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

    public Quaternion initRotation;
    public Vector3 initPos;
    private Transform target;
    private float threshold = 0.1f;
    private CameraMove camMove;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        initPos = transform.position;
        initRotation = transform.rotation;
        camMove = GetComponent<CameraMove>();
    }

    private void Update()
    {
        //Debug.Log(stageManager.ingameStageUIManager == null);
        if (!camMove.IsDragging)
        {
            if (stageManager.ingameStageUIManager.windowMode == WindowMode.Setting)
            {
                if (stageManager.currentPlayer == null)
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
            else if (stageManager.ingameStageUIManager.windowMode == WindowMode.Skill)
            {
                // 각도 러프
                transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, rotationSpeed * Time.deltaTime * 4f);

                // 무빙 러프
                transform.position = Vector3.Lerp(transform.position, initPos, zoomSpeed * Time.deltaTime * 4f);

                if (Quaternion.Angle(transform.rotation, initRotation) < threshold && Vector3.Distance(transform.position, initPos) < threshold)
                {
                    Time.timeScale = 0f;
                }
            }
            else
            {
                // 각도 러프
                transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, rotationSpeed * Time.deltaTime * 2f);

                // 무빙 러프
                transform.position = Vector3.Lerp(transform.position, initPos, zoomSpeed * Time.deltaTime * 2f);
            }
        }
    }
}
