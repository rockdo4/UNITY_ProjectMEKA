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
            // ���⼭ �ش� �гο� ����������ư �߰�
            // ���� �����صΰ�,
            // �ر� �ȵ� �͵��� ��Ȱ��ȭ

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

                // �ر� �ȵ����� ��Ȱ��ȭ
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
