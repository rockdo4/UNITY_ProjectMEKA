using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public GameObject stageInfoPanel;

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
        });
        assignmentStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Assignment);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelAssignmentStageChoice();
        });
        challengeStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Challenge);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelChallengeStageChoice();
        });
    }

    private void Update()
    {
        //if (stageInfoPanel.activeSelf && Input.GetMouseButtonUp(0))
        //{
        //    CloseStageInfoWindow();
        //}
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

            //Debug.Log(stageTable == null);
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

            var stageButton = stageButtonGo.GetComponent<Button>();
            stageButton.onClick.AddListener(SetStageInfoWindow);

            // active false
            if (!stage.Value.isUnlocked)
            {
                stageButtonGo.SetActive(false);
            }
        }
    }

    public void SetStageInfoWindow()
    {
        stageInfoPanel.SetActive(true);
        // 현재 선택된 스테이지 정보 받아서 ui 세팅
        var stageTableData = stageTable.GetStageData(StageDataManager.Instance.selectedStageData.stageID);
    }

    public void CloseStageInfoWindow()
    {
        stageInfoPanel.SetActive(false);
    }

    public void OnClickBattleStart()
    {
        var stageTableData = stageTable.GetStageData(StageDataManager.Instance.selectedStageData.stageID);

        switch (stageTableData.Class)
        {
            case (int)StageClass.Story:
                var sceneNames1 = Utils.GetSceneNames(StageClass.Story);
                panelManager.LoadFormation();
                SceneManager.LoadScene(sceneNames1[stageTableData.Index]);
                break;
            case (int)StageClass.Assignment:
                var sceneNames2 = Utils.GetSceneNames(StageClass.Assignment);
                panelManager.LoadFormation();
                SceneManager.LoadScene(sceneNames2[stageTableData.Index]);
                break;
            case (int)StageClass.Challenge:
                var sceneNames3 = Utils.GetSceneNames(StageClass.Challenge);
                panelManager.LoadFormation();
                SceneManager.LoadScene(sceneNames3[stageTableData.Index]);
                break;
        }
    }
}
