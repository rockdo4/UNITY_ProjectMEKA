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

    public float currentCost;
    public int maxCost;
    public int allMonsterCount;
    public int allTargetMonsterCount;
    public int killMonsterCount;
    public int killTargetMonsterCount;
    public int leftWaveCount;
    public int currentHouseLife;
    public int maxHouseLife;

    private void Awake()
    {
        currentCost = maxCost;
        currentHouseLife = maxHouseLife;
    }

    private void Update()
    {
        if(gameState == GameState.Playing)
        {
            CheckGameOver();
        }
    }

    public void CheckGameOver()
    {
        // 모드에 따라 구분
        var id = StageDataManager.Instance.selectedStageData.stageID;
        var stageData = StageDataManager.Instance.stageTable.GetStageData(id);
        Debug.Log("stage id: " + id);
        switch(stageData.Type)
        {
            case (int)StageMode.Deffense:
                DefenseModeWinCondition();
                break;
            case (int)StageMode.Annihilation:
                AnnihilationModeWinCondition();
                break;
            case (int)StageMode.Survival:
                SurvivalModeWinCondition();
                break;
        }
    }

    public void DefenseModeWinCondition()
    {
        // win condition : mission clear at least 1
        // loose condition : mission clear 0 or house hp 0

        if (currentHouseLife <= 0)
        {
            gameState = GameState.Die;
            // show die result window
        }
        else if (killMonsterCount == allMonsterCount)
        {
            var id = StageDataManager.Instance.selectedStageData.stageID;
            var nextStageID = StageDataManager.Instance.stageTable.GetStageData(id).NextStageID;

            gameState = GameState.Win;
            StageDataManager.Instance.selectedStageData.isCleared = true;
            StageDataManager.Instance.selectedStageDatas[nextStageID].isUnlocked = true;
            StageDataManager.Instance.UpdatePlayData();
            // need to mission score apply
            // show win result window
        }
    }

    public void AnnihilationModeWinCondition()
    {

    }

    public void SurvivalModeWinCondition()
    {

    }
}
