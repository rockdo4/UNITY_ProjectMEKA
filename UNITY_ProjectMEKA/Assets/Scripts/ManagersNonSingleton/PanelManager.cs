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

	public void Awake()
	{
		mainPos = mainPanel.position;
		characterWindowPos = characterWindowPanel.position;
		gachaPos = gachaPanel.position;
		formationPos = formationPanel.position;
		inventoryPos = inventoryPanel.position;

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

	public void LoadBattleScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Tilemap");
	}

	public void ExitGame() 
	{
		Application.Quit();
	}
}
