using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
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

    public GameObject stageInfoParentPanel;
    public GameObject stageInfoPanel;
    public GameObject monsterInfoPanel;
    public GameObject mapInfoPanel;
    public Transform starPanel;
    public TextMeshProUGUI stageButtonText;
    public TextMeshProUGUI monsterButtonText;
    public TextMeshProUGUI mapButtonText;
    public TextMeshProUGUI stageHeaderText;
    public TextMeshProUGUI stageOutlineHeaderText;
    public TextMeshProUGUI stageOutlineText;
    public TextMeshProUGUI stageMissionHeaderText;
    public TextMeshProUGUI stageMission1Text;
    public TextMeshProUGUI stageMission2Text;
    public TextMeshProUGUI stageMission3Text;
    public TextMeshProUGUI recommendedLevelText;
    public GameObject monsterButtonPrefab;
    public GameObject monsterInfoPopUpWindow;

    private void Awake()
    {
        PlayDataManager.Init();
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
            var chapter = StageDataManager.Instance.stageTable.GetStageData(stage.Key).ChapterNumber;
            var stageNumber = StageDataManager.Instance.stageTable.GetStageData(stage.Key).StageNumber;

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
        stageInfoParentPanel.SetActive(true);

        // 현재 선택된 스테이지 정보 받아서 ui 세팅
        var stringTable = StageDataManager.Instance.stringTable;
        var stageData = StageDataManager.Instance.stageTable.GetStageData(StageDataManager.Instance.selectedStageData.stageID);
        SetStageInfoPanel(stringTable, stageData);
        SetMonsterInfoPanel(stringTable, stageData);
        SetMapInfoPanel();

        StageInfoWindowOn();
    }

    public void SetStageInfoPanel(StringTable stringTable, StageData stageTable)
    {
        // 버튼들 텍스트
        stageButtonText.SetText(stringTable.GetString("stage"));
        monsterButtonText.SetText(stringTable.GetString("monsterInfo"));
        mapButtonText.SetText(stringTable.GetString("mapInfo"));

        // 챕터 - 스테이지넘버 스테이지이름
        var stageNameID = stageTable.StageNameStringID;
        var stageOutlineID = stageTable.StageOutlineStringID;
        var stageMission1ID = stageTable.StageMission1StringID;
        var stageMission2ID = stageTable.StageMission2StringID;
        var stageMission3ID = stageTable.StageMission3StringID;

        var stageHeader = new string($"{stageTable.ChapterNumber} - {stageTable.StageNumber} {stringTable.GetString(stageNameID)}");
        stageHeaderText.SetText(stageHeader);



        // 스테이지 인포
        var mission1 = stringTable.GetString(stageMission1ID);
        mission1 = new string(mission1.Select(c => c == '0' ? stageTable.Mission1Value.ToString()[0] : c).ToArray());
        var mission2 = stringTable.GetString(stageMission2ID);
        mission2 = new string(mission2.Select(c => c == '0' ? stageTable.Mission2Value.ToString()[0] : c).ToArray());
        var mission3 = stringTable.GetString(stageMission3ID);
        mission3 = new string(mission3.Select(c => c == '0' ? stageTable.Mission3Value.ToString()[0] : c).ToArray());
        stageOutlineHeaderText.SetText(stringTable.GetString("outline"));
        stageOutlineText.SetText(stringTable.GetString(stageOutlineID));
        stageMissionHeaderText.SetText(stringTable.GetString("mission"));
        stageMission1Text.SetText(mission1);
        stageMission2Text.SetText(mission2);
        stageMission3Text.SetText(mission3);

        // 권장 레벨
        var recommendedLevel = new string($"({stringTable.GetString("recommendedLevel")} : {stageTable.RecommendedLevel})");
        recommendedLevelText.SetText(recommendedLevel);

        // 별
        var count = StageDataManager.Instance.selectedStageData.clearScore;
        for(int i = 0; i < 3; ++i)
        {
            if(count > i)
            {
                starPanel.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                starPanel.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void SetMonsterInfoPanel(StringTable stringTable, StageData stageTable)
    {
        // temp code
        var icons = monsterInfoPanel.GetComponentsInChildren<MonsterIcon>();
        foreach(var icon in icons)
        {
            Destroy(icon.gameObject);
        }

        // 스테이지 테이블의 아이디를 받아와서 / 기준으로 나눠서, 아이디를 저장, 몬스터 레벨 테이블에서 스탯을 가져와서 찍어준다
        var monsterLevelIDArray = stageTable.MonsterLevelID.Split('/');
        // 이 수 만큼 프리팹 생성, 아이콘에 스프라이트 씌워주기
        for (int i = 0; i < monsterLevelIDArray.Length; ++i)
        {
            // sprite는 나중에 몬스터 테이블에 몬스터 기본 id, 이름, 이미지 경로 이렇게 3개만 있게 만들고 가져와서 쓰기
            //var id = monsterLevelIDArray[i].Substring(0, monsterLevelIDArray.Length-2);

            var icon = Instantiate(monsterButtonPrefab, monsterInfoPanel.transform);
            icon.GetComponentInChildren<TextMeshProUGUI>().SetText((i + 1).ToString());

            var iconScript = icon.GetComponent<MonsterIcon>();
            iconScript.Init(this, int.Parse(monsterLevelIDArray[i]), stringTable);
        }
    }

    public void SetMapInfoPanel()
    {

    }

    public void CloseStageInfoWindow()
    {
        stageInfoParentPanel.SetActive(false);
    }

    public void OnClickBattleStart()
    {
        var stageTableData = StageDataManager.Instance.stageTable.GetStageData(StageDataManager.Instance.selectedStageData.stageID);

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

    public void StageInfoWindowOn()
    {
        if(!stageInfoPanel.activeSelf)
        {
            stageInfoPanel.SetActive(true);
            monsterInfoPanel.SetActive(false);
            mapInfoPanel.SetActive(false);
        }
    }

    public void MonsterInfoWindowOn()
    {
        if(!monsterInfoPanel.activeSelf)
        {
            monsterInfoPanel.SetActive(true);
            stageInfoPanel.SetActive(false);
            mapInfoPanel.SetActive(false);
        }
    }

    public void MapInfoWindowOn()
    {
        if(!mapInfoPanel.activeSelf)
        {
            mapInfoPanel.SetActive(true);
            stageInfoPanel.SetActive(false);
            monsterInfoPanel.SetActive(false);
        }
    }
}
