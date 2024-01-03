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
            DontDestroyOnLoad(gameObject); // 현재 오브젝트를 씬 로드 시 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복 인스턴스 파괴
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
