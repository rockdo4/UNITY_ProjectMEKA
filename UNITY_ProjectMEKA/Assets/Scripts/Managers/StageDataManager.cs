using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class StageDataManager : MonoBehaviour
{
    public List<StageSaveData> currentStageData;
    public StageClass currentStageClass;

    public void SetCurrentStageData(StageClass stageClass)
    {
        switch(stageClass)
        {
            case StageClass.None:
                break;
            case StageClass.Story:
                currentStageData = PlayDataManager.data.storyStageInfos;
                break;
            case StageClass.Assignment:
                currentStageData = PlayDataManager.data.assignmentStageInfos;
                break;
            case StageClass.Challenge:
                currentStageData = PlayDataManager.data.challengeStageInfos;
                break;
        }
        currentStageClass = stageClass;
    }

    public void UpdatePlayData()
    {
        switch (currentStageClass)
        {
            case StageClass.None:
                break;
            case StageClass.Story:
                PlayDataManager.data.storyStageInfos = currentStageData;
                break;
            case StageClass.Assignment:
                PlayDataManager.data.assignmentStageInfos = currentStageData;
                break;
            case StageClass.Challenge:
                PlayDataManager.data.challengeStageInfos = currentStageData;
                break;
        }
    }

    public void CheckPlayData()
    {
        switch (currentStageClass)
        {
            case StageClass.None:
                break;
            case StageClass.Story:
                if(PlayDataManager.data.storyStageInfos != null)
                {
                    currentStageData = PlayDataManager.data.storyStageInfos;
                }
                break;
            case StageClass.Assignment:
                if (PlayDataManager.data.assignmentStageInfos != null)
                {
                    currentStageData = PlayDataManager.data.assignmentStageInfos;
                }
                break;
            case StageClass.Challenge:
                if(PlayDataManager.data.challengeStageInfos != null)
                {
                    currentStageData = PlayDataManager.data.challengeStageInfos;
                }
                break;
        }
    }
}
