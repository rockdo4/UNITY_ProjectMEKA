using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CameraManager : MonoBehaviour
{
    //초기 위치값 / temp 위치값(currentPlayer 중심 위치)
    private StageManager stageManager;

    public Vector3 initPos;
    public Vector3 tempPos;

    //currentPlayer가 배치 완료 상태 && 현재 윈도우 모드가 setting일 때
    //currentPlayer를 중심으로 따라감

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        initPos = transform.position;
    }

    private void Update()
    {
        if(stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.Setting)
        {
            transform.position = tempPos;
        }
    }
}
