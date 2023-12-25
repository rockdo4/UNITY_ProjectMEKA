using System;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class StageManager : MonoBehaviour
{
    [HideInInspector]
    public IngameStageUIManager ingameStageUIManager;
    [HideInInspector]
    public CharacterIconManager characterIconManager;
    [HideInInspector]
    public ArrangeJoystick arrangeJoystick;


    [Header("테스트용 필수 작성 2가지")]
    public int stageID;
    public StageClass stageClass;

    [Header("테스트 시 확인 변수들")]
    public PlayerController currentPlayer;
    [HideInInspector]
    public CharacterIcon currentPlayerIcon;
    public GameState gameState;
    public (MissionType, int)[] missionTypes = new (MissionType, int)[3];
    public int tempClearCount = 0;
    public float timer;
    public float defaultCost;
    public float currentCost;
    public float maxCost;
    public float useCost;
    public int allMonsterCount;
    public int allTargetMonsterCount;
    public int killMonsterCount;
    public int killTargetMonsterCount;
    public int leftWaveCount;
    public int currentHouseLife;
    public int maxHouseLife;

    public List<(int,int)> rewardList = new List<(int, int)>();

    public TileManager tileManager;

    private void OnEnable()
    {
        PlayDataManager.Init();
		Init();
	}

	private void Awake()
    {
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (gameState == GameState.Playing)
        {
            // for tilemap test
            CheckGameOver();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            var id = StageDataManager.Instance.selectedStageData.stageID;
            var stageData = StageDataManager.Instance.stageTable.GetStageData(id);
            gameState = GameState.Win;
            StageDataManager.Instance.selectedStageData.isCleared = true;
            StageDataManager.Instance.selectedStageDatas[stageData.NextStageID].isUnlocked = true;
            StageDataManager.Instance.UpdatePlayData();
        }
    }

    public void CheckGameOver()
    {
        // when battle is over, check mission clear
        if (MissionPlayerWin() != GameState.Playing)
        {
            int id;
            StageData stageData;
            StageSaveData stageSaveData;

            // original code
            if (StageDataManager.Instance.selectedStageData != null)
            {
                id = StageDataManager.Instance.selectedStageData.stageID;
                stageID = id;
                stageData = StageDataManager.Instance.stageTable.GetStageData(id);
                stageSaveData = StageDataManager.Instance.selectedStageDatas[stageID];
            }
            else
            {
                // for test
                stageData = StageDataManager.Instance.stageTable.GetStageData(stageID);
                StageDataManager.Instance.SetCurrentStageClass(stageClass);
                StageDataManager.Instance.LoadPlayData();
                stageSaveData = StageDataManager.Instance.selectedStageDatas[stageID];
            }

            for(int i = 0; i< missionTypes.Length; ++i)
            {
                switch(missionTypes[i].Item1)
                {
                    case MissionType.MonsterKillCount:
                        if (MissionMonsterKillCount(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            Debug.Log("***Mission clear*** : MonsterKillCount");
                            tempClearCount++;
                        }
                        break;
                    case MissionType.SurviveTime:
                        if (MissionSurviveTime(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            Debug.Log("***Mission clear*** : SurviveTime");
                            tempClearCount++;
                        }
                        break;
                    case MissionType.ClearTime:
                        if (MissionClearTime(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            Debug.Log("***Mission clear*** : ClearTime");
                            tempClearCount++;
                        }
                        break;
                    case MissionType.CostLimit:
                        if (MissionCostLimit(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            Debug.Log("***Mission clear*** : CostLimit");
                            tempClearCount++;
                        }
                        break;
                    case MissionType.HouseLifeLimit:
                        if (MissionHouseLifeLimit(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            Debug.Log("***Mission clear*** : HouseLifeLimit");
                            tempClearCount++;
                        }
                        break;
                    case MissionType.PlayerWin:
                        if (MissionPlayerWin() == GameState.Win)
                        {
                            Debug.Log("***Mission clear*** : PlayerWin");
                            tempClearCount++;
                        }
                        break;
                }
            }
            if (tempClearCount >= 0 && !stageSaveData.isCleared) // first clear
            {
                gameState = GameState.Win;
                stageSaveData.isCleared = true;
                stageSaveData.clearScore = tempClearCount;
                StageDataManager.Instance.selectedStageDatas[stageData.NextStageID].isUnlocked = true;
                StageDataManager.Instance.UpdatePlayData();
                for(int i = 0; i < rewardList.Count; ++i)
                {
                    ItemInventoryManager.Instance.AddItemByID(rewardList[i].Item1, rewardList[i].Item2);
                }
                PlayDataManager.Save();
            }
            else
            {
                if(tempClearCount == 0)
                {
                    gameState = GameState.Die;
                }
                else
                {
                    gameState = GameState.Win;
                    for (int i = 1; i < rewardList.Count; ++i)
                    {
                        ItemInventoryManager.Instance.AddItemByID(rewardList[i].Item1, rewardList[i].Item2);
                    }
                }
            }
        }
    }

    public GameState DefenseModeWinCondition()
    {
        // win condition : kill all monsters
        // loose condition : house hp 0

        if (currentHouseLife <= 0)
        {
            return GameState.Die;
        }
        else if (killMonsterCount == allMonsterCount)
        {
            return GameState.Win;
        }

        return GameState.Playing;
    }

    //public GameState AnnihilationModeWinCondition()
    //{
    //    // win condition : kill all target monsters in time
    //    // loose condition : time over or target monster get to house

    //    return GameState.Playing;
    //}

    //public GameState SurvivalModeWinCondition()
    //{
    //    // win condition : kill all monsters
    //    // loose condition : house hp 0
    //    return GameState.Playing;
    //}

    public MissionClear MissionMonsterKillCount(int value)
    {
        // win condition : kill monsters * value
        if(killMonsterCount >= value)
        {
            return MissionClear.Clear;
        }

        return MissionClear.Fail;
    }

    public MissionClear MissionSurviveTime(int value)
    {
        if(timer >= value)
        {
            return MissionClear.Clear;
        }

        return MissionClear.Fail;
    }

    public MissionClear MissionClearTime(int value)
    {
        if(MissionPlayerWin() == GameState.Win && timer <= value)
        {
            return MissionClear.Clear;
        }

        return MissionClear.Fail;
    }

    public MissionClear MissionCostLimit(int value)
    {
        if(useCost <= value)
        {
            return MissionClear.Clear;
        }

        return MissionClear.Fail;
    }

    public MissionClear MissionHouseLifeLimit(int value)
    {
        if(currentHouseLife >= value)
        {
            return MissionClear.Clear;
        }

        return MissionClear.Fail;
    }

    public GameState MissionPlayerWin()
    {
        //var id = StageDataManager.Instance.selectedStageData.stageID;
        var stageData = StageDataManager.Instance.stageTable.GetStageData(stageID);

        if (currentHouseLife <= 0)
        {
            return GameState.Die;
        }
        else if (killMonsterCount == allMonsterCount)
        {
            return GameState.Win;
        }

        return GameState.Playing;

        //switch (stageData.Type)
        //{
        //    case (int)StageMode.Deffense:
        //        return DefenseModeWinCondition();
        //    case (int)StageMode.Annihilation:
        //        return AnnihilationModeWinCondition();
        //    case (int)StageMode.Survival:
        //        return SurvivalModeWinCondition();
        //}
    }

    public void Init()
    {
        tileManager = new TileManager();
        ingameStageUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<IngameStageUIManager>();
        characterIconManager = GameObject.FindGameObjectWithTag(Tags.characterIconManager).GetComponent<CharacterIconManager>();
        arrangeJoystick = GameObject.FindGameObjectWithTag(Tags.joystick).GetComponent<ArrangeJoystick>();

        int id;
        StageData stageData;

        // original code
        if (StageDataManager.Instance.selectedStageData != null)
        {
            id = StageDataManager.Instance.selectedStageData.stageID;
            stageID = id;
            stageData = StageDataManager.Instance.stageTable.GetStageData(id);
        }
        else
        {
            // for test
            stageData = StageDataManager.Instance.stageTable.GetStageData(stageID);
            StageDataManager.Instance.SetCurrentStageClass(stageClass);
            StageDataManager.Instance.LoadPlayData();
        }

        defaultCost = stageData.DefaultCost;
        maxCost = stageData.MaxCost;
        currentCost = defaultCost;

        maxHouseLife = stageData.HouseLife;
        currentHouseLife = maxHouseLife;

        for (int i = 0; i < missionTypes.Length; ++i)
        {
            if( i == 0 )
            {
                missionTypes[i] = ((MissionType)stageData.Mission1Type, stageData.Mission1Value);
            }
            else if(i == 1)
            {
                missionTypes[i] = ((MissionType)stageData.Mission2Type, stageData.Mission2Value);
            }
            else
            {
                missionTypes[i] = ((MissionType)stageData.Mission3Type, stageData.Mission3Value);
            }
        }
    }
}
