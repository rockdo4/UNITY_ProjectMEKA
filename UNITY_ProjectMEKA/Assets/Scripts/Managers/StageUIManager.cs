using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using static Defines;
using System.Collections;
using Michsky.MUIP;

public class StageUIManager : MonoBehaviour
{
    public PanelManager panelManager;

    // unlock system buttons
    public CharacterInfoText characterInfoText;
    public Button storyStageButton;
    public Button assignmentStageButton;
    public Button challengeStageButton;
    public Button deviceCoreButton;
    public Button deviceEngineButton;
    public Button[] synchroButtons;
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
    public GameObject storyStageButtonPrefab;
    public GameObject assignmentAndChallengeButtonPrefab;

    [Header("StageInfoPanel")]
    public GameObject stageInfoParentPanel;
    public GameObject stageInfoPanel;
    public GameObject monsterInfoPanel;
    public GameObject mapInfoPanel;
    public Transform stageInfoStarPanel;

    [Header("StageInfo Text")]
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

    private StageTable stageTable;
    private RewardTable rewardTable;
    private MonsterTable monsterTable;
    private string stageMonsterImagePath;

    //limhyeoungjun add loading screen
    public GameObject loadingScreen;
    public Image loadingFillImage; 
    public Text loadingText;

    private void Awake()
    {
        PlayDataManager.Init();
        stageTable = StageDataManager.Instance.stageTable;
        rewardTable = StageDataManager.Instance.rewardTable;
        monsterTable = DataTableMgr.GetTable<MonsterTable>();
        stageMonsterImagePath = "StageMonsterImage/0_stageMonsterImage";
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
        if(deviceCoreButton != null)
        {
            deviceCoreButton.onClick.AddListener(() =>
            {
                SetSelectedSystemButtonsByUnlock(SystemForUnlock.Device);
                devicePanel.ShowCore();
            });
        }
        if(deviceEngineButton != null)
        {
            deviceEngineButton.onClick.AddListener(() =>
            {
                SetSelectedSystemButtonsByUnlock(SystemForUnlock.Device);
                devicePanel.ShowEngine();
            });
        }

        foreach(var sunchroButton in synchroButtons)
        {
            sunchroButton.onClick.AddListener(() =>
            {
                SetSelectedSystemButtonsByUnlock(SystemForUnlock.Synchro);
            });
        }
 

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
            // 해당하는 패널로 이동
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
                CreateStoryStageButtons(storyStageButtonPanel.transform, PlayDataManager.data.storyStageDatas);
                break;
            case StageClass.Assignment:
                CreateAssignmentAndChallengeStageButtons(assignmentStageButtonPanel.transform, PlayDataManager.data.assignmentStageDatas);
                break;
            case StageClass.Challenge:
                CreateAssignmentAndChallengeStageButtons(challengeStageButtonPanel.transform, PlayDataManager.data.challengeStageDatas);
                break;
        }
    }

    public void CreateStoryStageButtons(Transform parentTr, Dictionary<int, StageSaveData> selectedStageData)
    {
        foreach (var stage in selectedStageData)
        {
            // instantiate
            var stageButtonGo = Instantiate(storyStageButtonPrefab, parentTr);

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

            if(count >= 1)
				stageButtonGo.GetComponent<Image>().sprite = stageButtonScript.clearOn;
			else
				stageButtonGo.GetComponent<Image>().sprite = stageButtonScript.clearOff;

            //for (int i = 0; i < 3; ++i)
            //         {
            //             if (count > i)
            //             {
            //                 stageButtonGo.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            //             }
            //             else
            //             {
            //                 stageButtonGo.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            //             }
            //         }

            if (!stage.Value.isUnlocked)
            {
                stageButtonGo.SetActive(false);
            }
        }
    }

    public void CreateAssignmentAndChallengeStageButtons(Transform parentTr, Dictionary<int, StageSaveData> selectedStageData)
    {
        foreach (var stage in selectedStageData)
        {
            // instantiate
            var stageButtonGo = Instantiate(assignmentAndChallengeButtonPrefab, parentTr);
            
            var stageButtonScript = stageButtonGo.GetComponent<StageButton>();
            var stageMonsterImage = stageButtonScript.monsterImage;
            var stageIndex = stageButtonScript.stageIndexText;
            var stageRewardGuide = stageButtonScript.rewardGuideText;
            var stageTableData = stageTable.GetStageData(stage.Key);
            var rewardTableData = rewardTable.GetStageData(stageTableData.RewardID);

            // chapter + stage text apply
            var chapter = StageDataManager.Instance.stageTable.GetStageData(stage.Key).ChapterNumber;
            var stageNumber = StageDataManager.Instance.stageTable.GetStageData(stage.Key).StageNumber;
            var stageIndexText = new string($"{chapter} - {stageNumber}");
            stageIndex.SetText(stageIndexText);

            // id apply
            stageButtonScript.stageID = stage.Key;

            // locked stage ui set
            var hardFilter = stageButtonScript.hardBg;
            var hardIcon = stageButtonScript.hardTag;
            var stageData = stageTable.GetStageData(stage.Key);
            if (stageData.Hard)
            {
                hardFilter.SetActive(true);
                hardIcon.SetActive(true);
            }
            else
            {
                hardFilter.SetActive(false);
                hardIcon.SetActive(false);
            }

            // general ui set
            var stageMonsterResource = stageMonsterImagePath.Replace("0", stage.Key.ToString());
            stageMonsterImage.sprite = Resources.Load<Sprite>(stageMonsterResource);
            var stageRewardGuideText = new string($"{stage.Key}_stageRewardGuide");
            stageRewardGuide.SetText(stageRewardGuideText);

            // on click set
            var stageButton = stageButtonGo.GetComponent<Button>();
            stageButton.onClick.AddListener(()=>
            {
                if(stage.Value.isUnlocked)
                {
                    SetStageInfoWindow();
                }
                else
                {
                    SetLockedPopUpWinodw();
                }
            });
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
        SetMapInfoPanel(stringTable, stageData);

        StageInfoWindowOn();
    }

    public void SetStageInfoPanel(StringTable stringTable, StageData stageTable)
    {
        // 버튼들 텍스트
        //stageButtonText.SetText(stringTable.GetString("stage"));
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

        // 권장 레벨
        var recommendedLevel = new string($"({stringTable.GetString("recommendedLevel")} : {stageTable.RecommendedLevel})");
        recommendedLevelText.SetText(recommendedLevel);

        // 별
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

        // 스테이지 테이블의 아이디를 받아와서 / 기준으로 나눠서, 아이디를 저장, 몬스터 레벨 테이블에서 스탯을 가져와서 찍어준다
        var monsterLevelIDArray = stageTable.MonsterLevelID.Split('/');
        // 이 수 만큼 프리팹 생성, 아이콘에 스프라이트 씌워주기
        for (int i = 0; i < monsterLevelIDArray.Length; ++i)
        {
            var monsterID = int.Parse(monsterLevelIDArray[i].Substring(0, monsterLevelIDArray[i].Length-2));

            Debug.Log(monsterID);
            var monsterTableData = monsterTable.GetMonsterData(monsterID);
            var monsterIconPath = monsterTableData.IconPath;
            var icon = Instantiate(monsterButtonPrefab, monsterInfoPanel.GetComponentInChildren<Children>().transform);
            icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(monsterIconPath);
            var iconScript = icon.GetComponent<MonsterIcon>();
            iconScript.Init(this, int.Parse(monsterLevelIDArray[i]), stringTable);
        }
    }

    public void SetMapInfoPanel(StringTable stringTable, StageData stageTable)
    {
        var child = mapInfoPanel.GetComponentInChildren<Children>();
        var mapImage = child.GetComponent<Image>();
        mapImage.sprite = Resources.Load<Sprite>(stageTable.MapImagePath);
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
                //var sceneNames1 = Utils.GetSceneNames(StageClass.Story);
                //panelManager.LoadFormation();
                //SceneManager.LoadScene(sceneNames1[stageTableData.Index]);
                LoadSceneAsync(Utils.GetSceneNames(StageClass.Story)[stageTableData.Index]);
                break;
            case (int)StageClass.Assignment:
                //var sceneNames2 = Utils.GetSceneNames(StageClass.Assignment);
                //panelManager.LoadFormation();
                //SceneManager.LoadScene(sceneNames2[stageTableData.Index]);
                LoadSceneAsync(Utils.GetSceneNames(StageClass.Assignment)[stageTableData.Index]);
                break;
            case (int)StageClass.Challenge:
                //var sceneNames3 = Utils.GetSceneNames(StageClass.Challenge);
                //panelManager.LoadFormation();
                //SceneManager.LoadScene(sceneNames3[stageTableData.Index]);
                LoadSceneAsync(Utils.GetSceneNames(StageClass.Challenge)[stageTableData.Index]);
                break;
        }
    }
    private void LoadSceneAsync(string sceneName)
    {
        panelManager.LoadFormation();
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            loadingFillImage.fillAmount = progress;

            loadingText.text = (progress * 100).ToString("F0") + "%";

            yield return null;
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
            //stageInfoPanel.SetActive(false);
            mapInfoPanel.SetActive(false);
        }
    }

    public void MapInfoWindowOn()
    {
        if(!mapInfoPanel.activeSelf)
        {
            mapInfoPanel.SetActive(true);
            //stageInfoPanel.SetActive(false);
            monsterInfoPanel.SetActive(false);
        }
    }

    public void UpdateSystemUIByUnlock()
    {
        // 시스템 해금 정보 읽어오기
        // 스위치문 : 종류별로 true, false에 따라 패널 활성/비활성 or 팝업 띄우기

        var systems = PlayDataManager.data.systemUnlockData;
        for(int i = 0; i < systems.Count; ++i)
        {
            var systemType = (SystemForUnlock)i;
            switch(systemType)
            {
                case SystemForUnlock.Gacha:
                    if(systems[i]) // 해금 됐으면
                    {
                        gachaUnLockPanel.SetActive(false);
                    }
                    break;
                case SystemForUnlock.Affection:
                    if (systems[i]) // 해금 됐으면
                    {
                        affectionLockPanel.SetActive(false);
                    }
                    break;
            }
        }
    }
    
}
