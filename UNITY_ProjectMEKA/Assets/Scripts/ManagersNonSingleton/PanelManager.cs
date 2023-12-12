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
	public RectTransform stageTypePanel;
	private Vector3 stageTypePos;

	public void Awake()
	{
		mainPos = mainPanel.position;
		characterWindowPos = characterWindowPanel.position;
		gachaPos = gachaPanel.position;
		formationPos = formationPanel.position;
		inventoryPos = inventoryPanel.position;
        //stageTypePos = stageTypePanel.position;

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

	//12.10, �����, �������������г� �߰�
	public void ChangePanelStageType()
	{
        previousPos = stageTypePos;
		previousPanel = stageTypePanel;
		stageTypePanel.position = mainPos;
		mainPanel.position = previousPos;
    }

	public void LoadBattleScene()
	{
		var formation = formationPanel.GetComponent<FormationManager>();
		if(formation == null)
		{
			Debug.LogError("FormationManager ���ҷ���");
			return;
		}
		formation.SetHolderFormation();

		if(DataHolder.isVaild)
		{
			//UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
			//UnityEngine.SceneManagement.SceneManager.LoadScene("BUGLHJ");
			UnityEngine.SceneManagement.SceneManager.LoadScene("Bug_KimMinji");
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
