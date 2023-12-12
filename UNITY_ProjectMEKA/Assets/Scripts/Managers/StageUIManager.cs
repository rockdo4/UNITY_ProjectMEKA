using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Defines;

public class StageUIManager : MonoBehaviour
{
    public PanelManager panelManager;
    public Button storyStageButton;
    public Button assignmentStageButton;
    public Button challengeStageButton;

    public GameObject storyStageButtonPanel;
    public GameObject assignmentStageButtonPanel;
    public GameObject challengeStageButtonPanel;
    public GameObject stageButtonPrefab;

    private StageTable stageTable;

    private void Awake()
    {
        PlayDataManager.Init();
        stageTable = DataTableMgr.GetTable<StageTable>();

        SetStageButtons(StageClass.Story);
        SetStageButtons(StageClass.Assignment);
        SetStageButtons(StageClass.Challenge);

        storyStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Story);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelStoryStageChoice();
            //SetStageButtons(StageClass.Story);
        });
        assignmentStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Assignment);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelAssignmentStageChoice();
            //SetStageButtons(StageClass.Assignment);
        });
        challengeStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Challenge);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelChallengeStageChoice();
            //SetStageButtons(StageClass.Challenge);
        });
    }

    private void Start()
    {
    }

    public void ClassOnClick(StageClass stageClass)
    {
        StageDataManager.Instance.SetCurrentStageClass(stageClass);
    }

    public void SetStageButtons(StageClass stageClass)
    {
        switch (stageClass)
        {
            case StageClass.Story:
                CreatStageButtons(storyStageButtonPanel.transform, PlayDataManager.data.storyStageDatas);
                break;
            case StageClass.Assignment:
                CreatStageButtons(assignmentStageButtonPanel.transform, PlayDataManager.data.assignmentStageDatas);
                break;
            case StageClass.Challenge:
                CreatStageButtons(challengeStageButtonPanel.transform, PlayDataManager.data.challengeStageDatas);
                break;
        }
    }

    public void CreatStageButtons(Transform parentTr, Dictionary<int, StageSaveData> currentStageData)
    {
        var stringBuilder = new StringBuilder();
        foreach (var stage in currentStageData)
        {
            //instantiate
            var stageButtonGo = Instantiate(stageButtonPrefab, parentTr);

            // chapter + stage text apply
            var stageButtonText = stageButtonGo.GetComponentInChildren<TextMeshProUGUI>();

            var chapter = stageTable.GetStageData(stage.Key).ChapterNumber;
            var stageNumber = stageTable.GetStageData(stage.Key).StageNumber;

            stringBuilder.Clear();
            stringBuilder.Append(chapter);
            stringBuilder.Append("-");
            stringBuilder.Append(stageNumber.ToString());
            stageButtonText.SetText(stringBuilder.ToString());

            // id apply
            var stageButtonScript = stageButtonGo.GetComponent<StageButton>();
            stageButtonScript.stageID = stage.Key;

            // active false
            if (!stage.Value.isUnlocked)
            {
                stageButtonGo.SetActive(false);
            }
        }
    }
}
