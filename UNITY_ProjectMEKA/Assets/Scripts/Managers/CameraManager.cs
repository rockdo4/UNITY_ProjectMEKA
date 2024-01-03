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
    public float initZoom;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        initPos = transform.position;
        initRotation = transform.rotation;
        camMove = GetComponent<CameraMove>();
        initZoom = Camera.main.fieldOfView;
    }

    private void Update()
    {
        if (!camMove.IsDragging)
        {
            if (stageManager.ingameStageUIManager.windowMode == WindowMode.Setting)
            {
                if (stageManager.currentPlayer == null)
                {
                    return;
                }

                target = stageManager.currentPlayer.transform;
                
                Vector3 cameraOffset = Quaternion.AngleAxis(-10, Vector3.right) * Vector3.back;
                cameraOffset *= minDistance;
                Vector3 cameraPosition = target.position + cameraOffset + Vector3.up * minDistance;
                Quaternion targetRotation = Quaternion.LookRotation(target.position - cameraPosition);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 2f);

                transform.position = Vector3.Lerp(transform.position, cameraPosition, zoomSpeed * Time.deltaTime * 2f);

                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initZoom, 5 * Time.deltaTime * 2f); //initZoom;
            }
            else if (stageManager.ingameStageUIManager.windowMode == WindowMode.Skill)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, rotationSpeed * Time.deltaTime * 4f);

                transform.position = Vector3.Lerp(transform.position, initPos, zoomSpeed * Time.deltaTime * 4f);

                if (Quaternion.Angle(transform.rotation, initRotation) < threshold && Vector3.Distance(transform.position, initPos) < threshold)
                {
                    Time.timeScale = 0f;
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, initRotation, rotationSpeed * Time.deltaTime * 2f);

                transform.position = Vector3.Lerp(transform.position, initPos, zoomSpeed * Time.deltaTime * 2f);

                //Camera.main.fieldOfView = initZoom;
            }
        }
    }



}
