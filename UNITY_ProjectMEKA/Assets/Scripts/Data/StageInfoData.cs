using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * StageInfoData : 스테이지 저장&로드용 데이터(유동 데이터)
 * StageData : 스테이지 데이터테이블(고정 데이터)
 */
public class StageInfoData
{
    public int stageID;
    public int isUnlocked;
    public int isCleared;
    public int clearScore;
}

public class StageData
{
    public int stageID;
    public int stageClass;
    public int stageType;
    public int chapterNumber;
    public int stageNumber;
    public string mapImagePath;
    public string mapSoundPath;
    public int nextStageID;
    public string failConditionPath;
    public string mission1Path;
    public int[] missionType = new int[3];
    public int[] missionValue = new int[3];
    public string[] missionPath = new string[3];
    public float stageTime;
    public int houseLife;
    public int isConfigurable;
    public int configureID;
    public int eventID;
    public int rewardID;
}