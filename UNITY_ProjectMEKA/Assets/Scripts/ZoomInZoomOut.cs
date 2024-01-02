using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class ZoomInZoomOut : MonoBehaviour
{
   
    public float perspectiveZoomSpeed = 0.5f;        // ����/�ƿ� �ӵ�
    public float orthoZoomSpeed = 0.5f;              // ���翵 ��忡�� ����/�ƿ� �ӵ�

    public float minZoomDistance = 10.0f;             // �ּ� �� �Ÿ�
    public float maxZoomDistance = 60.0f;            // �ִ� �� �Ÿ�

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
                    // ���翵 ī�޶� ����/�ƿ�
                    cameraComponent.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                    cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize, minZoomDistance, maxZoomDistance);
                }
                else
                {
                    // ���� ī�޶� ����/�ƿ�
                    cameraComponent.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                    cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView, minZoomDistance, maxZoomDistance);
                }
            }
        }
    }
    
}
