using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynchroPanel : MonoBehaviour
{
	public static readonly int MAX_SYNCHRO_GRADE = 6;

	public Image[] leftStar;
	public Image[] rightStar;

	public ItemAutoQuantityCard[] synchroItemCard;
	public TextMeshProUGUI beforeLevel;
	public TextMeshProUGUI afterLevel;

	[Header("etc")]
	public Button applyButton;
	public CharacterInfoText infoPanel;
	public RectTransform resultPanel;

	private SynchroData synchroInfoData;
	private Character currCharacter;

	private CharacterTable charTable;
	private SynchroTable synchroTable;

	private void Awake()
	{
		charTable = DataTableMgr.GetTable<CharacterTable>();
		synchroTable = DataTableMgr.GetTable<SynchroTable>();

		applyButton.onClick.AddListener(() =>
		{
			infoPanel.SetPopUpPanel(
				"��ũ���Ͻðڽ��ϱ�?",
				() =>
				{
					ApplySynchro();
					infoPanel.UpdateCharacter();
				}, 
				"��", "�ƴϿ�"
				);
		});

		//applyButton.onClick.AddListener(() =>
		//{
		//	//ApplySynchro();

		//	foreach (var card in synchroItemCard)
		//	{
		//		card.ConsumeItem();
		//		card.SetText();
		//	}

		//	//UpdateTargetLevel();
		//});
	}

	private void OnEnable()
	{
		
	}

	//��ư ������ �����
	public void SetCharacter(Character character)
	{
		if(character == null)
		{
            Debug.Log("ĳ���Ͱ� �����ϴ�.");
            return;
        }

		currCharacter = character;

		var grade = currCharacter.CharacterGrade;
		var occupation = charTable.GetCharacterData(currCharacter.CharacterID).CharacterOccupation;

		synchroInfoData = synchroTable.GetSynchroData(grade, occupation);

		
		var itemTable = DataTableMgr.GetTable<ItemInfoTable>();

		var tier1 = itemTable.GetItemData(synchroInfoData.Tier1ID);
		var tier2 = itemTable.GetItemData(synchroInfoData.Tier2ID);
		var tier3 = itemTable.GetItemData(synchroInfoData.Tier3ID);

		synchroItemCard[0].GetComponentInChildren<TextMeshProUGUI>().SetText($"{tier1.Name}");
		synchroItemCard[1].GetComponentInChildren<TextMeshProUGUI>().SetText($"{tier2.Name}");
		synchroItemCard[2].GetComponentInChildren<TextMeshProUGUI>().SetText($"{tier3.Name}");

		if (synchroInfoData == null)
		{
            Debug.Log("�ռ� ���� ����");
			synchroItemCard[0].SetItem(synchroInfoData.Tier1ID, synchroInfoData.RequireTier1);
			synchroItemCard[1].SetItem(synchroInfoData.Tier2ID, synchroInfoData.RequireTier2);
			synchroItemCard[2].SetItem(synchroInfoData.Tier3ID, synchroInfoData.RequireTier3);
			return;
        }
		else
		{
            synchroItemCard[0].SetItem(synchroInfoData.Tier1ID, synchroInfoData.RequireTier1);
            synchroItemCard[1].SetItem(synchroInfoData.Tier2ID, synchroInfoData.RequireTier2);
            synchroItemCard[2].SetItem(synchroInfoData.Tier3ID, synchroInfoData.RequireTier3);
        }

		if (currCharacter.CharacterLevel < synchroInfoData.Grade * 10)
		{
			beforeLevel.SetText($"<color=red>{synchroInfoData.Grade * 10}</color>");
			if(synchroInfoData.Grade == 6)
			{
				afterLevel.SetText($"--");
			}
			else
			{
				afterLevel.SetText($"{(synchroInfoData.Grade + 1) * 10}");
			}
		}
		else
		{
			beforeLevel.SetText($"{synchroInfoData.Grade * 10}");
			if (synchroInfoData.Grade == 6)
			{
				afterLevel.SetText($"--");
			}
			else
			{
				afterLevel.SetText($"{(synchroInfoData.Grade + 1) * 10}");
			}
		}

		for (int i = 0; i < leftStar.Length; i++)
		{
			if (i == grade - 3)
			{
				leftStar[i].gameObject.SetActive(true);
				rightStar[i].gameObject.SetActive(true);
			}
			else
			{
				leftStar[i].gameObject.SetActive(false);
				rightStar[i].gameObject.SetActive(false);
			}
		}

        UpdateAfterSetCharacter();
	}

	//��ư ������ SetCharacter �������� ���� 
	public void CheckGrade()
	{
		if (currCharacter == null)
			gameObject.SetActive(false);

		if (currCharacter.CharacterGrade == MAX_SYNCHRO_GRADE)
		{
			gameObject.SetActive(false);

			//�ִ� ����� ��� �˶� �˾�? ���
			//popup.SetActive(true);
		}
	}
	
	//������ ī�� ������Ʈ
	public void UpdateRequired()
	{
		foreach (var card in synchroItemCard)
		{
			card.SetText();			
		}
		SetCharacter(currCharacter);
	}
	private void UpdateAfterSetCharacter()
	{
        foreach (var card in synchroItemCard)
        {
            card.SetText();
        }
    }

	public void ApplySynchro()
	{
		bool isRequired = true;

		//������ üũ
		foreach (var card in synchroItemCard)
		{
			isRequired = card.IsEnoughRequire();
		}

		//���� üũ
		if(currCharacter.CharacterLevel < synchroInfoData.Grade * 10)
		{
			isRequired = false;
		}

		//��ȭ üũ
		//isRequired = silverItemCard.IsEnoughRequire();

		if(isRequired)
		{
			Debug.Log("�ռ� ����");
		}
		else
		{
			Debug.Log("����");
		}

		if(isRequired)
		{
			currCharacter.CharacterGrade++;

			foreach (var card in synchroItemCard)
			{
				card.ConsumeItem();
				card.SetText();
			}

			OpenResultPanel();
		}
		else
		{
			Debug.Log("��ᰡ ���ڶ��ϴ�.");
		}

		UpdateRequired();
		
		GameManager.Instance.SaveExecution();
	}

	public void OpenResultPanel()
	{
		Debug.Log("���� �г� ���ȴٰ� ħ");
		//resultPanel.gameObject.SetActive(true);

		//resultPanel �� ���� ����
	}
}
