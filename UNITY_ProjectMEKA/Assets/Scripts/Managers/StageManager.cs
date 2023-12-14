using System;
using Unity.VisualScripting;
using UnityEngine;
using static Defines;

public class StageManager : MonoBehaviour
{
    //[HideInInspector]
    public IngameStageUIManager ingameStageUIManager;
    public CharacterIconManager characterIconManager;
    public ArrangeJoystick arrangeJoystick;

    public PlayerController currentPlayer;

    public CharacterIcon currentPlayerIcon;

    public GameState gameState;
    public (MissionType, int)[] missionTypes = new (MissionType, int)[3];

    public float timer;
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

    private void OnEnable()
    {
        PlayDataManager.Init();
		//Init();
		ingameStageUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<IngameStageUIManager>();
		characterIconManager = GameObject.FindGameObjectWithTag(Tags.characterIconManager).GetComponent<CharacterIconManager>();
		arrangeJoystick = GameObject.FindGameObjectWithTag(Tags.joystick).GetComponent<ArrangeJoystick>();

	}

	private void Awake()
    {
        currentCost = maxCost;
        currentHouseLife = maxHouseLife;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (gameState == GameState.Playing)
        {
            // for tilemap test
            //CheckGameOver();
        }
    }

    public void CheckGameOver()
    {
        // when battle is over, check mission clear
        if (MissionPlayerWin() != GameState.Playing)
        {
            var id = StageDataManager.Instance.selectedStageData.stageID;
            var stageData = StageDataManager.Instance.stageTable.GetStageData(id);

            for(int i = 0; i< missionTypes.Length; ++i)
            {
                switch(missionTypes[i].Item1)
                {
                    case MissionType.MonsterKillCount:
                        if (MissionMonsterKillCount(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            StageDataManager.Instance.selectedStageData.clearScore++;
                        }
                        break;
                    case MissionType.SurviveTime:
                        if (MissionSurviveTime(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            StageDataManager.Instance.selectedStageData.clearScore++;
                        }
                        break;
                    case MissionType.ClearTime:
                        if (MissionClearTime(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            StageDataManager.Instance.selectedStageData.clearScore++;
                        }
                        break;
                    case MissionType.CostLimit:
                        if (MissionCostLimit(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            StageDataManager.Instance.selectedStageData.clearScore++;
                        }
                        break;
                    case MissionType.HouseLifeLimit:
                        if (MissionHouseLifeLimit(missionTypes[i].Item2) == MissionClear.Clear)
                        {
                            StageDataManager.Instance.selectedStageData.clearScore++;
                        }
                        break;
                    case MissionType.PlayerWin:
                        if (MissionPlayerWin() == GameState.Win)
                        {
                            StageDataManager.Instance.selectedStageData.clearScore++;
                        }
                        break;
                }
            }
            if (StageDataManager.Instance.selectedStageData.clearScore > 0)
            {
                gameState = GameState.Win;
                StageDataManager.Instance.selectedStageData.isCleared = true;
                StageDataManager.Instance.selectedStageDatas[stageData.NextStageID].isUnlocked = true;
                StageDataManager.Instance.UpdatePlayData();
            }
            else
            {
                gameState = GameState.Die;
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

    public GameState AnnihilationModeWinCondition()
    {
        // win condition : kill all target monsters in time
        // loose condition : time over or target monster get to house

        return GameState.Playing;
    }

    public GameState SurvivalModeWinCondition()
    {
        // win condition : kill all monsters
        // loose condition : house hp 0
        return GameState.Playing;
    }

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
        if( maxHouseLife-currentHouseLife >= value)
        {
            return MissionClear.Clear;
        }

        return MissionClear.Fail;
    }

    public GameState MissionPlayerWin()
    {
        var id = StageDataManager.Instance.selectedStageData.stageID;
        var stageData = StageDataManager.Instance.stageTable.GetStageData(id);
        switch (stageData.Type)
        {
            case (int)StageMode.Deffense:
                return DefenseModeWinCondition();
            case (int)StageMode.Annihilation:
                return AnnihilationModeWinCondition();
            case (int)StageMode.Survival:
                return SurvivalModeWinCondition();
        }

        return GameState.Playing;
    }

    public void Init()
    {
        ingameStageUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<IngameStageUIManager>();
        characterIconManager = GameObject.FindGameObjectWithTag(Tags.characterIconManager).GetComponent<CharacterIconManager>();
        arrangeJoystick = GameObject.FindGameObjectWithTag(Tags.joystick).GetComponent<ArrangeJoystick>();

        for (int i = 0; i < missionTypes.Length; ++i)
        {
            var id = StageDataManager.Instance.selectedStageData.stageID;
            var stageData = StageDataManager.Instance.stageTable.GetStageData(id);

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
