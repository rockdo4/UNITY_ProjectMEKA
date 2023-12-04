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

		characterWindowPanel.gameObject.SetActive(false);
		gachaPanel.gameObject.SetActive(false);
		formationPanel.gameObject.SetActive(false);
		inventoryPanel.gameObject.SetActive(false);

		previousPanel = mainPanel;
		previousPos = mainPos;
	}

	public void ChangePanelMain()
	{
		previousPanel.position = previousPos;
		mainPanel.position = mainPos;

		previousPanel.gameObject.SetActive(false);
	}

	public void ChangePanelCharacterWindow()
	{
		characterWindowPanel.gameObject.SetActive(true);

		previousPos = characterWindowPos;
		previousPanel = characterWindowPanel;
		characterWindowPanel.position = mainPos;
		mainPanel.position = previousPos;
	}

	public void ChangePanelGacha()
	{
		gachaPanel.gameObject.SetActive(true);

		previousPos = gachaPos;
		previousPanel = gachaPanel;
		gachaPanel.position = mainPos;
		mainPanel.position = previousPos;
	}

	public void ChangePanelFormation()
	{
		formationPanel.gameObject.SetActive(true);

		previousPos = formationPos;
		previousPanel = formationPanel;
		formationPanel.position = mainPos;
		mainPanel.position = previousPos;
	}

	public void ChangePanelInventory()
	{
		inventoryPanel.gameObject.SetActive(true);

        previousPos = inventoryPos;
        previousPanel = inventoryPanel;
        inventoryPanel.position = mainPos;
        mainPanel.position = previousPos;
    }

	public void LoadBattleScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Bug_KimMinji");
	}

	public void ExitGame() 
	{
		Application.Quit();
	}
}
