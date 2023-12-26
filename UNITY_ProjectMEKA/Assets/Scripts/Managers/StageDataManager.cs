using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class StageDataManager
{
    private static StageDataManager instance;
    public static StageDataManager Instance 
    {
        get
        { 
            if(instance == null)
                instance = new StageDataManager();
            return instance;
        }
    }

    public StageSaveData selectedStageData;
    public Dictionary<int,StageSaveData> selectedStageDatas;
    private StageClass currentStageClass;
    public bool toStageChoicePanel;
    
    // tables
    public StageTable stageTable;
    public CharacterTable characterTable;
    public CharacterLevelTable characterLevelTable;
    public StringTable stringTable;
    public ItemInfoTable itemInfoTable;

    public Language language = Language.Kor;

    private StageDataManager()
    {
        stageTable = DataTableMgr.GetTable<StageTable>();
        characterTable = DataTableMgr.GetTable<CharacterTable>();
        characterLevelTable = DataTableMgr.GetTable<CharacterLevelTable>();
        stringTable = DataTableMgr.GetTable<StringTable>();
        itemInfoTable = DataTableMgr.GetTable<ItemInfoTable>();
    }

    public void SetCurrentStageClass(StageClass stageClass)
    {
        currentStageClass = stageClass;
    }

    public void UpdatePlayData()
    {
        switch (currentStageClass)
        {
            case StageClass.None:
                break;
            case StageClass.Story:
                PlayDataManager.data.storyStageDatas = selectedStageDatas;
                break;
            case StageClass.Assignment:
                PlayDataManager.data.assignmentStageDatas = selectedStageDatas;
                break;
            case StageClass.Challenge:
                PlayDataManager.data.challengeStageDatas = selectedStageDatas;
                break;
        }
        PlayDataManager.Save();
    }

    public void LoadPlayData()
    {
        switch (currentStageClass)
        {
            case StageClass.None:
                break;
            case StageClass.Story:
                if(PlayDataManager.data.storyStageDatas != null)
                {
                    selectedStageDatas = PlayDataManager.data.storyStageDatas;
                }
                break;
            case StageClass.Assignment:
                if (PlayDataManager.data.assignmentStageDatas != null)
                {
                    selectedStageDatas = PlayDataManager.data.assignmentStageDatas;
                }
                break;
            case StageClass.Challenge:
                if(PlayDataManager.data.challengeStageDatas != null)
                {
                    selectedStageDatas = PlayDataManager.data.challengeStageDatas;
                }
                break;
        }
    }
}
