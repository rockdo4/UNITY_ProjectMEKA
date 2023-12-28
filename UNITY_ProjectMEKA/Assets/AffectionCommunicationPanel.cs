using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AffectionCommunicationPanel : MonoBehaviour
{
	[Header("Info")]
	public Image characterImage;
	public TextMeshProUGUI characterName;
	public TextMeshProUGUI affectionText;
	public Image affectionSlider;
	public Button communicationButton;

	[Header("Communication")]
	public RectTransform textPanel;
	public RectTransform infoBox;
	public RectTransform selectPanel;

	[Header("Etc")]
	public Button closeButton;
	public ModalWindow modalWindow;
	public RectTransform cautionDaily;

	[Header("Test")]
	public Button timeReset;

	private Character currCharacter;
	private CommuinicationDictionary commuinicationDict;
	private List<CommunicationData> currentCommunicationList;
	private int count = 0;
	private AffectionPortrait affectionPortrait;

	private void Awake()
	{
		closeButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});

        communicationButton.onClick.AddListener(() =>
		{
            OnClickCommunication();
        });

		timeReset.onClick.AddListener(() =>
		{
			TimeReset();
			CheckTime();
		});
	}

	private void OnDisable()
	{
		commuinicationDict = null;
		currentCommunicationList = null;
		affectionPortrait = null;
	}

	public void SetCharacter(Character character)
	{
		currCharacter = character;

		UpdateCharacter(currCharacter);
		CheckTime();
	}

	public void SetPortrait(AffectionPortrait portrait)
	{
		affectionPortrait = portrait;
	}

	public void UpdatePortrait()
	{
		affectionPortrait.UpdatePortrait();
	}

	public void UpdateCharacter(Character character = null)
	{
		if(character != null)
		{
			currCharacter = character;
		}
		if (currCharacter == null)
		{
			return;
		}

		var info = DataTableMgr.GetTable<AffectionTable>().GetAffectionData(this.currCharacter.affection.AffectionLevel);

		characterImage.sprite = Resources.Load<Sprite>("CharacterIcon/" + currCharacter.ImagePath);
		characterImage.preserveAspect = true;
		characterName.SetText(currCharacter.Name);
		affectionText.SetText($"{currCharacter.affection.AffectionLevel}");

		float ratio = 1;

		if (info == null)
		{
			affectionText.SetText("Max");
		}
		else
		{
			ratio = (float)currCharacter.affection.AffectionPoint / info.AffectionPoint;
		}

		if(ratio == 0) ratio = 0.01f;
		affectionSlider.fillAmount = ratio;

		UpdateLayout();
	}

	public void AddAffectionPoint(int point)
	{
		var info = DataTableMgr.GetTable<AffectionTable>().GetAffectionData(currCharacter.affection.AffectionLevel);

		if(info == null)
		{
			currCharacter.affection.AffectionPoint = 0;
			NoticeMaxAffection();
			return;
		}

		currCharacter.affection.AffectionPoint += point;

		if(info.AffectionPoint <= currCharacter.affection.AffectionPoint)
		{
			currCharacter.affection.AffectionLevel++;
			currCharacter.affection.AffectionPoint -= info.AffectionPoint;
			Debug.Log("호감도 레벨업");
			UpdatePortrait();
		}

		currCharacter.affection.LastTime = System.DateTime.Now;

		UpdateCharacter();
		NoticeAffection(point);
		DisableDailyAffection();
		GameManager.Instance.SaveExecution();
	}

	public void OnClickCommunication()
	{
		if (!LoadCommunication()) return;

		count = 0;

		textPanel.gameObject.SetActive(true);
		NextScript();
		textPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
		{
			NextScript();
		});
	}

	public void NextScript()
	{
		if(count == -1)
		{
			textPanel.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
			textPanel.gameObject.SetActive(false);
			return;
		}

		var info = currentCommunicationList[count];

		if (info.Script[0] == '!') //선택지인 경우
		{
			var selects = SelectScript(); //선택지 끝날때까지 스크립트 List로 받아옴

			selectPanel.gameObject.SetActive(true); //선택지 패널 활성화

			var buttons = selectPanel.GetComponentsInChildren<Button>(); //선택지 버튼들
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].gameObject.SetActive(true);
				if (i >= selects.Count)
				{
					buttons[i].gameObject.SetActive(false);
					continue;
				}
				buttons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(selects[i].Script.Replace("!","")); //선택지 버튼에 스크립트 넣기
				

				var index = i;
				buttons[index].onClick.RemoveAllListeners();
				buttons[index].onClick.AddListener(() => //선택지 버튼에 클릭 이벤트 넣기
				{
					selectPanel.gameObject.SetActive(false);
					//AddAffectionPoint(selects[index].Value); //선택지 호감도 증가

					if (selects[index].Branch != -1)
					{
						count = currentCommunicationList.FindIndex(x => x.ScriptID == selects[index].Branch); 
					}

					NextScript();
				});
				LayoutRebuilder.ForceRebuildLayoutImmediate(buttons[i].GetComponent<RectTransform>());
            }
			LayoutRebuilder.ForceRebuildLayoutImmediate(selectPanel.GetComponent<RectTransform>());
			return;
		}

		textPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(info.Script);

		if (info.Branch == 0)
		{
			AddAffectionPoint(info.Value);
			count = -1;
			return;
		}

		if (info.Branch != -1)
		{
			count = currentCommunicationList.FindIndex(x => x.ScriptID == info.Branch);
			return;
		}
		count++;
	}

	public List<CommunicationData> SelectScript()
	{
		var info = currentCommunicationList[count];
		List<CommunicationData> selectScripts = new List<CommunicationData>();

		while (info.Script[0] == '!')
		{
			selectScripts.Add(info);
			count++;
			info = currentCommunicationList[count];
		}

		return selectScripts;
	}

	public bool LoadCommunication()
	{
		if(commuinicationDict == null)
		{
			commuinicationDict = DataTableMgr.GetTable<AffectionCommunicationTable>().GetAffectionData(currCharacter.CharacterID);
			if (commuinicationDict == null)
			{
				modalWindow.gameObject.SetActive(true);
				modalWindow.Notice($"{currCharacter.Name}와 가능한 대화가 없습니다", "확인");
				return false;
			}
		}

		List<int> keys = new List<int>(commuinicationDict.idCommunicationList.Keys);
		int randomKey = keys[Random.Range(0, keys.Count)];

		currentCommunicationList = commuinicationDict.idCommunicationList[randomKey];

		if (currentCommunicationList == null)
		{
			modalWindow.gameObject.SetActive(true);
			modalWindow.Notice("대화 리스트가 비어 있습니다", "확인");
			return false;
		}
		return true;
	}

	public void NoticeAffection(int point)
	{
		modalWindow.gameObject.SetActive(true);
		modalWindow.Notice($"호감도를 {point} 얻었습니다!", "확인");
	}

	public void NoticeMaxAffection()
	{
		modalWindow.gameObject.SetActive(true);
		modalWindow.Notice($"최대 호감도 레벨입니다", "확인");
	}

	private void UpdateLayout()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(affectionText.rectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(characterName.rectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(affectionSlider.rectTransform);
	}

	private void TimeReset()
	{
		currCharacter.affection.LastTime = default;
	}

	public void CheckTime()
	{
		var day = currCharacter.affection.LastTime.Date;
		var sixAm = day.AddHours(6);
		var tomorrow = sixAm.AddDays(1);

		if(currCharacter.affection.LastTime < sixAm)
		{
			if(DateTime.Now > sixAm)
			{
				EnableDailyAffection();
			}
		}
		else if(DateTime.Now > tomorrow)
		{
			EnableDailyAffection();
		}
		else
		{
			DisableDailyAffection();
		}
	}

	private void EnableDailyAffection()
	{
		cautionDaily.gameObject.SetActive(false);
		communicationButton.gameObject.SetActive(true);
	}

	private void DisableDailyAffection()
	{
		cautionDaily.gameObject.SetActive(true);
		communicationButton.gameObject.SetActive(false);
	}
}
