using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * StageInfoData : 스테이지 저장&로드용 데이터(유동 데이터)
 * StageData : 스테이지 데이터테이블(고정 데이터)
 */
public class StageSaveData
{
    public int stageID = 0;
    public bool isUnlocked = false;
    public bool isCleared = false;
    public int clearScore = 0;
}

public class StageData
{
    public int StageID { get; set; }
    public int Class { get; set; }
    public string ChapterNumber { get; set; }
    public int StageNumber { get; set; }
    public int Index { get; set; }
    public int MaxCost { get; set; }
    public int DefaultCost { get; set; }
    public string MapImagePath { get; set; }
    public string MapSoundPath { get; set; }
    public int NextStageID { get; set; }
    public bool Lock { get; set; }
    public int Mission1Type { get; set; }
    public int Mission1Value { get; set; }
    public int Mission2Type { get; set; }
    public int Mission2Value { get; set; }
    public int Mission3Type { get; set; }
    public int Mission3Value { get; set; }
    public float StageTime { get; set; }
    public int HouseLife { get; set; }
    public int IsConfigurable { get; set; }
    public int ConfigureID { get; set; }
    public int EventID { get; set; }
    public int RewardID { get; set; }
    public int RecommendedLevel { get; set; }
    public string StageClassStringID {  get; set; }
    public string StageNameStringID { get; set; }
    public string StageMission1StringID { get; set; }
    public string StageMission2StringID { get; set; }
    public string StageMission3StringID { get; set; }
    public string StageOutlineStringID { get; set; }
    public string MonsterLevelID { get; set; }
}