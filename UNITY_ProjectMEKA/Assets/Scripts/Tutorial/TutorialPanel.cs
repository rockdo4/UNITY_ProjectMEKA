using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
	public Transform scrollContent;
	public GameObject tutorialPrefab;

	[Header("Info")]
	public TextMeshProUGUI tutorialText;
	public TextMeshProUGUI tutorialTitle;
	public Image tutorialImage;

	[Header("Buttons")]
	public Button nextButton;
	public Button prevButton;

	private List<List<TutorialData>> tutorialList;
	private StringTable stringTable;
	private List<TutorialData> currentTutorialList;
	private int currentIndex;

	private void Start()
	{
		tutorialList = DataTableMgr.GetTable<TutorialTable>().GetOriginalTable();
		stringTable = DataTableMgr.GetTable<StringTable>();
		currentIndex = 1;

		SetTutorialBook();
	}

	private void OnDisable()
	{
		currentTutorialList = null;
		currentIndex = 1;
	}

	public void SetTutorialBook()
	{
		foreach(var list in tutorialList)
		{
			var obj = Instantiate(tutorialPrefab, scrollContent);
			obj.GetComponent<TutorialTabButton>().data = list;
			obj.GetComponent<Button>().onClick.AddListener(() => 
			{ 
				currentTutorialList = list;
				currentIndex = 1;
				SetCurrentBook();
				SoundManager.instance.PlayerSFXAudio("UIButtonClick");
			});
			obj.GetComponentInChildren<TextMeshProUGUI>().SetText(stringTable.GetString(list[0].Content));
		}
	}

	public void SetCurrentBook()
	{
		if(currentIndex > 1 || currentIndex < currentTutorialList.Count)
		{
			tutorialTitle.SetText($"{stringTable.GetString(currentTutorialList[0].Content)}  {currentIndex}/{currentTutorialList.Count - 1}");
			tutorialText.SetText(stringTable.GetString(currentTutorialList[currentIndex].Content));
			tutorialImage.sprite = Resources.Load<Sprite>(currentTutorialList[currentIndex].ImagePath);
		}
	}

	public void NextPage()
	{
		if (currentTutorialList == null) return;

		currentIndex++;
		if(currentIndex < currentTutorialList.Count)
		{
			SetCurrentBook();
		}
		else
		{
			currentIndex = 1;
			SetCurrentBook();
		}
	}

	public void PrevPage()
	{
		if (currentTutorialList == null) return;

		currentIndex--;
		if(currentIndex >= 1)
		{
			SetCurrentBook();
		}
		else if(currentIndex < 0)
		{
			currentIndex = currentTutorialList.Count - 1;
			SetCurrentBook();
		}
	}
}
