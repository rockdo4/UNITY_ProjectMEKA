using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRotation : MonoBehaviour
{
    private static ScreenRotation instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� ������Ʈ�� �� �ε� �� �ı����� �ʵ��� ����
        }
        else if (instance != this)
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ߺ� �ν��Ͻ� �ı�
        }
    }

    void Update()
    {
        
        float tilt = Input.acceleration.y;

        
        if (tilt > 0.5) 
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (tilt < -0.5)
        {
            Screen.orientation = ScreenOrientation.PortraitUpsideDown;
        }
        else
        {
            
            // Screen.orientation = ScreenOrientation.AutoRotation; 
        }
    }
}
