using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class ZoomInZoomOut : MonoBehaviour
{
   
    public float perspectiveZoomSpeed = 0.5f;        
    public float orthoZoomSpeed = 0.5f;           

    public float minZoomDistance = 10.0f;      
    public float maxZoomDistance = 60.0f;        

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
                    // Á¤»ç¿µ Ä«¸Þ¶ó ÁÜÀÎ/¾Æ¿ô
                    cameraComponent.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                    cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize, minZoomDistance, maxZoomDistance);
                }
                else
                {
                    // ¿ø±Ù Ä«¸Þ¶ó ÁÜÀÎ/¾Æ¿ô
                    cameraComponent.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
                    cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView, minZoomDistance, maxZoomDistance);
                }
            }
            //else 
            //{
                
            //    float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            //    float currentFOV = cameraComponent.fieldOfView;

            //    currentFOV -= scrollInput * orthoZoomSpeed;
            //    currentFOV = Mathf.Clamp(currentFOV, minZoomDistance, maxZoomDistance);

            //    cameraComponent.fieldOfView = currentFOV;
            //}
        }
    }
    
}
