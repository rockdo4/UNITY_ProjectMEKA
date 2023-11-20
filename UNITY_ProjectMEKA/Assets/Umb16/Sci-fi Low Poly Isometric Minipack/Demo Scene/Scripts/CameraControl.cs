using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        int cameraMoveFactor = 10;
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            cameraMoveFactor = 7;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles += Vector3.up *Time.deltaTime*40;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles -= Vector3.up * Time.deltaTime * 40;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-Time.deltaTime * cameraMoveFactor, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Time.deltaTime * cameraMoveFactor, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 temp = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0, temp.y, 0);
            transform.Translate(0, 0, -Time.deltaTime * cameraMoveFactor);
            transform.eulerAngles = new Vector3(temp.x, temp.y, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 temp = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0, temp.y, 0);
            transform.Translate(0, 0, Time.deltaTime * cameraMoveFactor);
            transform.eulerAngles = new Vector3(temp.x, temp.y, 0);
        }
        if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height)
            if (Input.GetAxis("Mouse ScrollWheel") != 0) // back
            {
                camera.transform.localPosition +=  Vector3.forward * (Input.GetAxis("Mouse ScrollWheel") * 30);
            }
    }
}
