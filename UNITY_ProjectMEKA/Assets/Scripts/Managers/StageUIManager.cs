using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using static Defines;

public class StageUIManager : MonoBehaviour
{
    public PanelManager panelManager;

    // unlock system buttons
    public CharacterInfoText characterInfoText;
    public Button storyStageButton;
    public Button assignmentStageButton;
    public Button challengeStageButton;
    public Button[] deviceButtons = new Button[2];
    public Button synchroButton;
    public Button skillUpdateButton;
    public GameObject gachaUnLockPanel;
    public GameObject affectionLockPanel;
    public SynchroPanel synchroPanel;
    public DevicePanel devicePanel;
    public SkillPanel skillPanel;
    private Action DeviceButtonClickEvent;
    private Action SyncroButtonClickEvent;
    private Action SkillButtonClickEvent;

    public GameObject storyStageButtonPanel;
    public GameObject assignmentStageButtonPanel;
    public GameObject challengeStageButtonPanel;
    public GameObject stageButtonPrefab;

    public GameObject stageInfoParentPanel;
    public GameObject stageInfoPanel;
    public GameObject monsterInfoPanel;
    public GameObject mapInfoPanel;
    public Transform stageInfoStarPanel;
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
        CreateStageButtons(StageClass.Story);
        CreateStageButtons(StageClass.Assignment);
        CreateStageButtons(StageClass.Challenge);
        UpdateSystemUIByUnlock();
        SetStageClassButtonsByUnlock();
        SetSystemButtonsByUnlock();
        SetSystemButtonEvents();
	}

    private void Update()
    {
    }

    public void SetSystemButtonEvents()
    {
        DeviceButtonClickEvent = () => 
        {
            devicePanel.gameObject.SetActive(true);
            devicePanel.SetCharacter(characterInfoText.character);
        };

        SyncroButtonClickEvent = () =>
        {
            synchroPanel.gameObject.SetActive(true);
            synchroPanel.SetCharacter(characterInfoText.character);
        };

        SkillButtonClickEvent = () =>
        {
            skillPanel.gameObject.SetActive(true);
            skillPanel.SetCharacter(characterInfoText.character);
        };
    }

    public void SetStageClassButtonsByUnlock()
    {
        storyStageButton.onClick.AddListener(() =>
        {
            SetSelctedStageClassButtons(SystemForUnlock.None, StageClass.Story);
        });
        assignmentStageButton.onClick.AddListener(() =>
        {
            SetSelctedStageClassButtons(SystemForUnlock.AssignmentStage, StageClass.Assignment);
        });
        challengeStageButton.onClick.AddListener(() =>
        {
            SetSelctedStageClassButtons(SystemForUnlock.ChallengeStage, StageClass.Challenge);
        });
    }

    public void SetSystemButtonsByUnlock()
    {
        foreach(var deviceButton in deviceButtons)
        {
            deviceButton.onClick.AddListener(() =>
            {
                SetSelectedSystemButtonsByUnlock(SystemForUnlock.Device);
            });
        }

        synchroButton.onClick.AddListener(() =>
        {
            SetSelectedSystemButtonsByUnlock(SystemForUnlock.Synchro);
        });

        skillUpdateButton.onClick.AddListener(() =>
        {
            SetSelectedSystemButtonsByUnlock(SystemForUnlock.Skill);
        });
    }

    public void SetSelectedSystemButtonsByUnlock(SystemForUnlock systemType)
    {
        var systems = PlayDataManager.data.systemUnlockData;
        var systemID = (int)systemType;
        if (systems[systemID])
        {
            // �ش��ϴ� �гη� �̵�
            switch(systemType)
            {
                case SystemForUnlock.Device:
                    DeviceButtonClickEvent?.Invoke();
                    break;
                case SystemForUnlock.Synchro:
                    SyncroButtonClickEvent?.Invoke();
                    break;
                case SystemForUnlock.Skill:
                    SkillButtonClickEvent?.Invoke();
                    break;
            }
        }
        else
        {
            SetLockedPopUpWinodw();
        }
    }

    public void SetSelctedStageClassButtons(SystemForUnlock systemType, StageClass stageClass)
    {
        if(systemType != SystemForUnlock.None)
        {
            var systems = PlayDataManager.data.systemUnlockData;
            var systemID = (int)systemType;
            if (systems[systemID])
            {
                IntoStageClassPanel(stageClass);            
            }
            else
            {
                SetLockedPopUpWinodw();
            }
        }
        else
        {
            IntoStageClassPanel(stageClass);
        }
    }

    public void SetLockedPopUpWinodw()
    {
        var stringTable = StageDataManager.Instance.stringTable;
        var lockMessage = stringTable.GetString("lockMessage");
        var ok = stringTable.GetString("ok");

        panelManager.SetNoticePanel(lockMessage, ok);
    }

    public void IntoStageClassPanel(StageClass stageClass)
    {
        ClassOnClick(stageClass);
        panelManager.ChangePanelMain();
        IntoSelectedStageClassPanel(stageClass);
    }

    public void IntoSelectedStageClassPanel(StageClass stageClass)
    {
        switch (stageClass)
        {
            case StageClass.Story:
                panelManager.ChangePanelStoryStageChoice();
                break;
            case StageClass.Assignment:
                panelManager.ChangePanelAssignmentStageChoice();
                break;
            case StageClass.Challenge:
                panelManager.ChangePanelChallengeStageChoice();
                break;
        }
    }

    public void ClassOnClick(StageClass stageClass)
    {
        StageDataManager.Instance.CurrentStageClass = stageClass;
    }

    public void CreateStageButtons(StageClass stageClass)
    {
        switch (stageClass)
        {
            case StageClass.Story:
                CreatSelectedStageButtons(storyStageButtonPanel.transform, PlayDataManager.data.storyStageDatas);
                break;
            case StageClass.Assignment:
                CreatSelectedStageButtons(assignmentStageButtonPanel.transform, PlayDataManager.data.assignmentStageDatas);
                break;
            case StageClass.Challenge:
                CreatSelectedStageButtons(challengeStageButtonPanel.transform, PlayDataManager.data.challengeStageDatas);
                break;
        }
    }

    public void CreatSelectedStageButtons(Transform parentTr, Dictionary<int, StageSaveData> currentStageData)
    {
        foreach (var stage in currentStageData)
        {
            // instantiate
            var stageButtonGo = Instantiate(stageButtonPrefab, parentTr);

            // chapter + stage text apply
            var stageButtonText = stageButtonGo.GetComponentInChildren<TextMeshProUGUI>();

            var chapter = StageDataManager.Instance.stageTable.GetStageData(stage.Key).ChapterNumber;
            var stageNumber = StageDataManager.Instance.stageTable.GetStageData(stage.Key).StageNumber;

            var stageButtonName = new string($"{chapter} - {stageNumber}");
            stageButtonText.SetText(stageButtonName);

            // id apply
            var stageButtonScript = stageButtonGo.GetComponent<StageButton>();
            stageButtonScript.stageID = stage.Key;

            var stageButton = stageButtonGo.GetComponent<Button>();
            stageButton.onClick.AddListener(SetStageInfoWindow);

            // clear score
            var count = stage.Value.clearScore;

            for(int i = 0; i < 3; ++i)
            {
                if(count > i)
                {
                    stageButtonGo.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    stageButtonGo.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                }
            }

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

        // ���� ���õ� �������� ���� �޾Ƽ� ui ����
        var stringTable = StageDataManager.Instance.stringTable;
        var stageData = StageDataManager.Instance.stageTable.GetStageData(StageDataManager.Instance.selectedStageData.stageID);
        SetStageInfoPanel(stringTable, stageData);
        SetMonsterInfoPanel(stringTable, stageData);
        SetMapInfoPanel();

        StageInfoWindowOn();
    }

    public void SetStageInfoPanel(StringTable stringTable, StageData stageTable)
    {
        // ��ư�� �ؽ�Ʈ
        stageButtonText.SetText(stringTable.GetString("stage"));
        monsterButtonText.SetText(stringTable.GetString("monsterInfo"));
        mapButtonText.SetText(stringTable.GetString("mapInfo"));

        // é�� - ���������ѹ� ���������̸�
        var stageNameID = stageTable.StageNameStringID;
        var stageOutlineID = stageTable.StageOutlineStringID;
        var stageMission1ID = stageTable.StageMission1StringID;
        var stageMission2ID = stageTable.StageMission2StringID;
        var stageMission3ID = stageTable.StageMission3StringID;

        var stageHeader = new string($"{stageTable.ChapterNumber} - {stageTable.StageNumber} {stringTable.GetString(stageNameID)}");
        stageHeaderText.SetText(stageHeader);



        // �������� ����
        var mission1 = stringTable.GetString(stageMission1ID);
        mission1 = mission1.Replace("0", stageTable.Mission1Value.ToString());
        var mission2 = stringTable.GetString(stageMission2ID);
        mission2 = mission2.Replace("0", stageTable.Mission2Value.ToString());
        var mission3 = stringTable.GetString(stageMission3ID);
        mission3 = mission3.Replace("0", stageTable.Mission3Value.ToString());
        stageOutlineHeaderText.SetText(stringTable.GetString("outline"));
        stageOutlineText.SetText(stringTable.GetString(stageOutlineID));
        stageMissionHeaderText.SetText(stringTable.GetString("mission"));
        stageMission1Text.SetText(mission1);
        stageMission2Text.SetText(mission2);
        stageMission3Text.SetText(mission3);

        // ���� ����
        var recommendedLevel = new string($"({stringTable.GetString("recommendedLevel")} : {stageTable.RecommendedLevel})");
        recommendedLevelText.SetText(recommendedLevel);

        // ��
        var count = StageDataManager.Instance.selectedStageData.clearScore;
        for(int i = 0; i < 3; ++i)
        {
            if(count > i)
            {
                stageInfoStarPanel.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                stageInfoStarPanel.GetChild(i).gameObject.SetActive(false);
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

        // �������� ���̺��� ���̵� �޾ƿͼ� / �������� ������, ���̵� ����, ���� ���� ���̺��� ������ �����ͼ� ����ش�
        var monsterLevelIDArray = stageTable.MonsterLevelID.Split('/');
        // �� �� ��ŭ ������ ����, �����ܿ� ��������Ʈ �����ֱ�
        for (int i = 0; i < monsterLevelIDArray.Length; ++i)
        {
            // sprite�� ���߿� ���� ���̺� ���� �⺻ id, �̸�, �̹��� ��� �̷��� 3���� �ְ� ����� �����ͼ� ����
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

    public void UpdateSystemUIByUnlock()
    {
        // �ý��� �ر� ���� �о����
        // ����ġ�� : �������� true, false�� ���� �г� Ȱ��/��Ȱ�� or �˾� ����

        var systems = PlayDataManager.data.systemUnlockData;
        for(int i = 0; i < systems.Count; ++i)
        {
            var systemType = (SystemForUnlock)i;
            switch(systemType)
            {
                case SystemForUnlock.Gacha:
                    if(systems[i]) // �ر� ������
                    {
                        gachaUnLockPanel.SetActive(false);
                    }
                    break;
                case SystemForUnlock.Affection:
                    if (systems[i]) // �ر� ������
                    {
                        affectionLockPanel.SetActive(false);
                    }
                    break;
            }
        }
    }
}
