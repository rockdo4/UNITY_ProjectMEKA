using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	public

	- void ChangePanelMain() : 메인화면으로 돌아가기 ex) ESC 버튼 눌러서 돌아가기
	- void ChangePanelCharacterWindow() : 캐릭터 창으로 가기
	- void ChangePanelGacha() : 가챠 창으로 가기

*/

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

	// 12.10, 김민지, 스테이지타입패널 추가
	public RectTransform stageClassPanel;
	private Vector3 stageClassPos;

	public RectTransform storyStageChoicePanel;
	private Vector3 storyStageChoicePos;

	public RectTransform assignmentStageChoicePanel;
	private Vector3 assignmentStageChoicePos;

	public RectTransform challengeStageChoicePanel;
	private Vector3 challengeStageChoicePos;

	public void Awake()
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

        previousPanel = mainPanel;
		previousPos = mainPos;
	}

	public void ChangePanelMain()
	{
		previousPanel.position = previousPos;
		mainPanel.position = mainPos;
	}

	public void ChangePanelCharacterWindow()
	{
		previousPos = characterWindowPos;
		previousPanel = characterWindowPanel;
		characterWindowPanel.position = mainPos;
		mainPanel.position = previousPos;
	}

	public void ChangePanelGacha()
	{
		previousPos = gachaPos;
		previousPanel = gachaPanel;
		gachaPanel.position = mainPos;
		mainPanel.position = previousPos;
	}

	public void ChangePanelFormation()
	{
		previousPos = formationPos;
		previousPanel = formationPanel;
		formationPanel.position = mainPos;
		mainPanel.position = previousPos;
	}

	public void ChangePanelInventory()
	{
        previousPos = inventoryPos;
        previousPanel = inventoryPanel;
        inventoryPanel.position = mainPos;
        mainPanel.position = previousPos;
    }

	//12.10, 김민지, 스테이지종류패널 추가
	public void ChangePanelStageClass()
	{
        previousPos = stageClassPos;
		previousPanel = stageClassPanel;
		stageClassPanel.position = mainPos;
		mainPanel.position = previousPos;
    }

	//12.12, 김민지, 스테이지선택패널 추가
	public void ChangePanelStoryStageChoice()
	{
        previousPos = storyStageChoicePos;
        previousPanel = storyStageChoicePanel;
        storyStageChoicePanel.position = mainPos;
        mainPanel.position = previousPos;
    }
	public void ChangePanelAssignmentStageChoice()
	{
        previousPos = assignmentStageChoicePos;
        previousPanel = assignmentStageChoicePanel;
        assignmentStageChoicePanel.position = mainPos;
        mainPanel.position = previousPos;
    }
	public void ChangePanelChallengeStageChoice()
	{
        previousPos = challengeStageChoicePos;
        previousPanel = challengeStageChoicePanel;
        challengeStageChoicePanel.position = mainPos;
        mainPanel.position = previousPos;
    }

	public void LoadFormation()
	{
		var formation = formationPanel.GetComponent<FormationManager>();
		if (formation == null)
		{
			Debug.LogError("FormationManager 못불러옴");
			return;
		}
		formation.SetHolderFormation();
	}

	public void ExitGame() 
	{
		Application.Quit();
	}
}
