using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	public

	- void ChangePanelMain() : ����ȭ������ ���ư��� ex) ESC ��ư ������ ���ư���
	- void ChangePanelCharacterWindow() : ĳ���� â���� ����
	- void ChangePanelGacha() : ��í â���� ����

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

	// 12.10, �����, ��������Ÿ���г� �߰�
	public RectTransform stageClassPanel;
	private Vector3 stageClassPos;

	public RectTransform storyStageChoicePanel;
	private Vector3 storyStageChoicePos;

	public RectTransform assignmentStageChoicePanel;
	private Vector3 assignmentStageChoicePos;

	public RectTransform challengeStageChoicePanel;
	private Vector3 challengeStageChoicePos;

	[Header("PopUp Panel")]
	public RectTransform popUpPanel;

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

    private void Start()
    {
        if(StageDataManager.Instance.toStageChoicePanel)
		{
			ChangePanelStoryStageChoice();
			StageDataManager.Instance.toStageChoicePanel = false;
		}
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

	//12.10, �����, �������������г� �߰�
	public void ChangePanelStageClass()
	{
        previousPos = stageClassPos;
		previousPanel = stageClassPanel;
		stageClassPanel.position = mainPos;
		mainPanel.position = previousPos;
    }

	//12.12, �����, �������������г� �߰�
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
			Debug.LogError("FormationManager ���ҷ���");
			return;
		}
		formation.SetHolderFormation();

		if (DataHolder.isVaild)
		{
			//UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
			//UnityEngine.SceneManagement.SceneManager.LoadScene("BUGLHJ");
			//UnityEngine.SceneManagement.SceneManager.LoadScene("Bug_KimMinji");
		}
		else
		{
			Debug.Log("�÷��̾ ������ ĳ���Ͱ� �����ϴ�.");
		}

	}

	public void ExitGame() 
	{
		Application.Quit();
	}
}
