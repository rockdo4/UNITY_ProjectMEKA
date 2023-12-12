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

    public GameObject stageButtonPanel;
    public GameObject stageButtonPrefab;

    private StageTable stageTable;

    private void Awake()
    {
        PlayDataManager.Init();
        stageTable = DataTableMgr.GetTable<StageTable>();

        storyStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Story);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelStageChoice();
            SetStageButtons();
        });
        assignmentStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Assignment);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelStageChoice();
            SetStageButtons();
        });
        challengeStageButton.onClick.AddListener(() =>
        {
            ClassOnClick(StageClass.Challenge);
            StageDataManager.Instance.LoadPlayData();
            panelManager.ChangePanelMain();
            panelManager.ChangePanelStageChoice();
            SetStageButtons();
        });
    }

    public void ClassOnClick(StageClass stageClass)
    {
        StageDataManager.Instance.SetCurrentStageClass(stageClass);
        if(StageDataManager.Instance.selectedStageDatas == null)
        {
            Debug.Log("selectedStageDatas == null");
        }
    }

    public void SetStageButtons()
    {
        if (StageDataManager.Instance.selectedStageDatas != null)
        {
            // 여기서 해당 패널에 스테이지버튼 추가
            // 전부 생성해두고,
            // 해금 안된 것들은 비활성화

            var stringBuilder = new StringBuilder();
            foreach(var stage in StageDataManager.Instance.selectedStageDatas)
            {
                //instantiate
                var stageButtonGo = Instantiate(stageButtonPrefab, stageButtonPanel.transform);

                // chapter + stage text apply
                var stageButtonText = stageButtonGo.GetComponentInChildren<TextMeshProUGUI>();

                var chapter = stageTable.GetStageData(stage.Key).ChapterNumber;
                var stageNumber = stageTable.GetStageData(stage.Key).StageNumber;

                stringBuilder.Clear();
                stringBuilder.Append(chapter);
                stringBuilder.Append("-");
                stringBuilder.Append(stageNumber.ToString());
                stageButtonText.SetText(stringBuilder.ToString());

                // 해금 안됐으면 비활성화
                if(!stage.Value.isUnlocked)
                {
                    stageButtonGo.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("selected stage datas null!!!");
        }
    }
}
