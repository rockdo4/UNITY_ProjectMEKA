using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class ZoomInZoomOut : MonoBehaviour
{
   
    public float perspectiveZoomSpeed = 0.5f;        // 줌인/아웃 속도
    public float orthoZoomSpeed = 0.5f;              // 정사영 모드에서 줌인/아웃 속도

    public float minZoomDistance = 10.0f;             // 최소 줌 거리
    public float maxZoomDistance = 60.0f;            // 최대 줌 거리

    private Camera cameraComponent;
    private IngameStageUIManager characterInfoUIManager;
    void Start()
    {
        characterInfoUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<IngameStageUIManager>();
        cameraComponent = GetComponent<Camera>();
    }

    void Update()
    {
        if (characterInfoUIManager.windowMode == WindowMode.None)
        {
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                if (cameraComponent.orthographic)
                {
                    // 정사영 카메라 줌인/아웃
                    cameraComponent.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                    cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize, minZoomDistance, maxZoomDistance);
                }
                else
                {
                    // 원근 카메라 줌인/아웃
                    cameraComponent.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                    cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView, minZoomDistance, maxZoomDistance);
                }
            }
        }
    }
    
}
