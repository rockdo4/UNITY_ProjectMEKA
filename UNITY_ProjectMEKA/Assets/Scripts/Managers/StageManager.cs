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
        switch(stageData.Type)
        {
            case (int)GameMode.Deffense:
                DefenseModeWinCondition();
                break;
            case (int)GameMode.Annihilation:
                AnnihilationModeWinCondition();
                break;
            case (int)GameMode.Survival:
                SurvivalModeWinCondition();
                break;
        }
    }

    public void DefenseModeWinCondition()
    {
        if (currentHouseLife <= 0)
        {
            gameState = GameState.Die;
        }
        else if (killMonsterCount == allMonsterCount)
        {
            gameState = GameState.Win;
            StageDataManager.Instance.selectedStageData.isCleared = true;
            // need to mission score apply
        }
    }

    public void AnnihilationModeWinCondition()
    {

    }

    public void SurvivalModeWinCondition()
    {

    }
}
