using UnityEngine;
using static Defines;

public class CameraMove : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 difference;
    private Vector3 resetCamera;
    private CameraManager cm;
    public bool IsDragging { get; private set; }
    private IngameStageUIManager characterInfoUIManager;
    public float smoothFactor = 10f;
    private void Start()
    {
        characterInfoUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<IngameStageUIManager>();
        cm = GetComponent<CameraManager>();
        resetCamera = Camera.main.transform.position;
    }

    private void LateUpdate()
    {
        if (characterInfoUIManager.windowMode == WindowMode.None)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

                if (groundPlane.Raycast(ray, out float distance))
                {
                    Vector3 point = ray.GetPoint(distance);

                    if (!IsDragging)
                    {
                        IsDragging = true;
                        origin = point;
                        resetCamera = Camera.main.transform.position;
                    }

                    difference = point - origin;
                }
            }
            else if (IsDragging)
            {
                IsDragging = false;
                resetCamera = Camera.main.transform.position;
                cm.initPos = Camera.main.transform.position;
                cm.initRotation = Camera.main.transform.rotation;
            }

            if (IsDragging)
            {
                Vector3 targetPosition = resetCamera - difference;
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * smoothFactor);

            }

            if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began)
            {
                Camera.main.transform.position = resetCamera;
            }
        }
    }
}

