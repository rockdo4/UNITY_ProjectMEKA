using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AffectionPanel : MonoBehaviour
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

	private Character currCharacter;
	private CommuinicationDictionary commuinicationDict;
	private List<CommunicationData> currentCommunicationList;
	private int count = 0;

	private void Awake()
	{
		closeButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
	}

	private void OnDisable()
	{
		commuinicationDict = null;
		currentCommunicationList = null;
	}

	public void SetCharacter(Character character)
	{
		currCharacter = character;

		UpdateCharacter(currCharacter);
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
		affectionText.SetText($"Lv : {currCharacter.affection.AffectionLevel}");

		float ratio = 1;

		if (info == null)
		{
			affectionText.SetText($"Lv : Max");
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

			Debug.Log("ȣ���� ������");
		}

		currCharacter.affection.LastTime = System.DateTime.Now;

		UpdateCharacter();
		NoticeAffection(point);
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

		if (info.Script[0] == '!') //�������� ���
		{
			var selects = SelectScript(); //������ ���������� ��ũ��Ʈ List�� �޾ƿ�

			selectPanel.gameObject.SetActive(true); //������ �г� Ȱ��ȭ

			var buttons = selectPanel.GetComponentsInChildren<Button>(); //������ ��ư��
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].gameObject.SetActive(true);
				if (i >= selects.Count)
				{
					buttons[i].gameObject.SetActive(false);
					continue;
				}
				buttons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(selects[i].Script.Replace("!","")); //������ ��ư�� ��ũ��Ʈ �ֱ�

				var index = i;
				buttons[index].onClick.RemoveAllListeners();
				buttons[index].onClick.AddListener(() => //������ ��ư�� Ŭ�� �̺�Ʈ �ֱ�
				{
					selectPanel.gameObject.SetActive(false);
					AddAffectionPoint(selects[index].Value); //������ ȣ���� ����

					if (selects[index].Branch != -1)
					{
						count = currentCommunicationList.FindIndex(x => x.ScriptID == selects[index].Branch); 
					}

					NextScript();
				});
			}
			return;
		}

		textPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(info.Script);

		if (info.Branch != -1)
		{
			count = currentCommunicationList.FindIndex(x => x.ScriptID == info.Branch);
			return;
		}

		if (info.Branch == 0) 
		{
			count = -1;
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
				modalWindow.Notice($"{currCharacter.Name}�� ������ ��ȭ�� �����ϴ�", "Ȯ��");
				return false;
			}
		}

		List<int> keys = new List<int>(commuinicationDict.idCommunicationList.Keys);
		int randomKey = keys[Random.Range(0, keys.Count)];

		currentCommunicationList = commuinicationDict.idCommunicationList[randomKey];

		if (currentCommunicationList == null)
		{
			modalWindow.gameObject.SetActive(true);
			modalWindow.Notice("��ȭ ����Ʈ�� ��� �ֽ��ϴ�", "Ȯ��");
			return false;
		}
		return true;
	}

	public void NoticeAffection(int point)
	{
		modalWindow.gameObject.SetActive(true);
		modalWindow.Notice($"ȣ������ {point} ������ϴ�!", "Ȯ��");
	}

	public void NoticeMaxAffection()
	{
		modalWindow.gameObject.SetActive(true);
		modalWindow.Notice($"�ִ� ȣ���� �����Դϴ�", "Ȯ��");
	}

	private void UpdateLayout()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(affectionText.rectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(characterName.rectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(affectionSlider.rectTransform);
	}
}
