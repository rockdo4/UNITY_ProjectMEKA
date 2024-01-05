using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PanelManager : MonoBehaviour
{
	public RectTransform mainPanel;
	private Vector3 mainPos;

	public RectTransform characterWindowPanel;
	private Vector3 characterWindowPos;

	public RectTransform gachaPanel;
	private Vector3 gachaPos;

	public RectTransform formationPanel;
	private Vector3 formationPos;

	public RectTransform inventoryPanel;
	private Vector3 inventoryPos;

	private RectTransform previousPanel;
	private Vector3 previousPos;

	// 12.10, �����, ��������Ÿ���г� �߰�
	public RectTransform stageClassPanel;
	private Vector3 stageClassPos;

	public RectTransform storyStageChoicePanel;
	private Vector3 storyStageChoicePos;

	public RectTransform assignmentStageChoicePanel;
	private Vector3 assignmentStageChoicePos;

	public RectTransform challengeStageChoicePanel;
	private Vector3 challengeStageChoicePos;

	public RectTransform affectionPanel;
	private Vector3 affectionPos;

	[Header("PopUp Panel")]
	public ModalWindow popUpPanel;

	public void Awake()
	{
		
	}

    private void Start()
    {
        mainPos = mainPanel.position;
        characterWindowPos = characterWindowPanel.position;
        gachaPos = gachaPanel.position;
        formationPos = formationPanel.position;
        inventoryPos = inventoryPanel.position;
        stageClassPos = stageClassPanel.position;
        storyStageChoicePos = storyStageChoicePanel.position;
        assignmentStageChoicePos = assignmentStageChoicePanel.position;
        challengeStageChoicePos = challengeStageChoicePanel.position;
		affectionPos = affectionPanel.position;

        previousPanel = mainPanel;
        previousPos = mainPos;

        if (StageDataManager.Instance.toStoryStageChoicePanel)
		{
			ChangePanelStoryStageChoice();
			StageDataManager.Instance.toStoryStageChoicePanel = false;
		}
		else if(StageDataManager.Instance.toAssignmentStageChoicePanel)
		{
			ChangePanelAssignmentStageChoice();
			StageDataManager.Instance.toAssignmentStageChoicePanel = false;
		}
		else if(StageDataManager.Instance.toChallengeStageChoicePanel)
		{
			ChangePanelChallengeStageChoice();
			StageDataManager.Instance.toChallengeStageChoicePanel = false;
		}
    }

    public void ChangePanelMain()
	{
		previousPanel.position = previousPos;
		mainPanel.position = mainPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");
        SoundManager.instance.PlayBGM("MainSceneBGM");
	}

	public void ChangePanelCharacterWindow()
	{
		previousPos = characterWindowPos;
		previousPanel = characterWindowPanel;
		characterWindowPanel.position = mainPos;
		mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }

    public void ChangePanelGacha()
	{
		previousPos = gachaPos;
		previousPanel = gachaPanel;
		gachaPanel.position = mainPos;
		mainPanel.position = previousPos;
		SoundManager.instance.PlayerSFXAudio("UIButtonClick");
		SoundManager.instance.PlayBGM("GachaMenuBGM");
	}

	public void ChangePanelFormation()
	{
		previousPos = formationPos;
		previousPanel = formationPanel;
		formationPanel.position = mainPos;
		mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }

    public void ChangePanelInventory()
	{
        previousPos = inventoryPos;
        previousPanel = inventoryPanel;
        inventoryPanel.position = mainPos;
        mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }

    //12.10, �����, �������������г� �߰�
    public void ChangePanelStageClass()
	{
        previousPos = stageClassPos;
		previousPanel = stageClassPanel;
		stageClassPanel.position = mainPos;
		mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }

    //12.12, �����, �������������г� �߰�
    public void ChangePanelStoryStageChoice()
	{
        previousPos = storyStageChoicePos;
        previousPanel = storyStageChoicePanel;
        storyStageChoicePanel.position = mainPos;
        mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }
    public void ChangePanelAssignmentStageChoice()
	{
        previousPos = assignmentStageChoicePos;
        previousPanel = assignmentStageChoicePanel;
        assignmentStageChoicePanel.position = mainPos;
        mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }
    public void ChangePanelChallengeStageChoice()
	{
        previousPos = challengeStageChoicePos;
        previousPanel = challengeStageChoicePanel;
        challengeStageChoicePanel.position = mainPos;
        mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }

    public void ChangePanelAffection()
	{
		previousPos = affectionPos;
		previousPanel = affectionPanel;
		affectionPanel.position = mainPos;
		mainPanel.position = previousPos;
        SoundManager.instance.PlayerSFXAudio("UIButtonClick");

    }

    public bool LoadFormation()
	{
		var formation = formationPanel.GetComponent<FormationManager>();
		if (formation == null)
		{
			Debug.LogError("FormationManager ���ҷ���");
			return false;
		}
		formation.SetHolderFormation();

		if (DataHolder.isVaild)
		{
			return true;
		}
		else
		{
			popUpPanel.gameObject.SetActive(true);
			popUpPanel.Notice("캐릭터를 편성해야 합니다", "확인");
			return false;
		}

	}

	public void SetPopUpPanel(string text, Action yesAction, string yesText = "", string noText = "")
	{
		var panel = popUpPanel.GetComponent<ModalWindow>();
		panel.gameObject.SetActive(true);
		panel.Show(text, yesAction, yesText, noText);
	}

	public void SetNoticePanel(string text, string yesText = "")
	{
		var panel = popUpPanel.GetComponent<ModalWindow>();
		panel.gameObject.SetActive(true);
		panel.Notice(text, yesText);
	}

	public void ExitGame() 
	{
		Application.Quit();
	}
}
