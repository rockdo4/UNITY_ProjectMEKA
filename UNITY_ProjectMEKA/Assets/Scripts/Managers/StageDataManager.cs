using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class StageDataManager : MonoBehaviour
{
    public static StageDataManager Instance { get; private set; }

    public StageSaveData selectedStageData;
    public LinkedList<StageSaveData> selectedStageDatas;
    private StageClass currentStageClass;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
