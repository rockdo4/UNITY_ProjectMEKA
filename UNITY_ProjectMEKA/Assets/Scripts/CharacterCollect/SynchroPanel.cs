using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynchroPanel : MonoBehaviour
{
	public static readonly int MAX_SYNCHRO_GRADE = 6;

	public TextMeshProUGUI beforeGrade;
	public TextMeshProUGUI afterGrade;

	public TextMeshProUGUI levelText;
	public ItemAutoQuantityCard[] synchroItemCard;
	public ItemAutoQuantityCard silverItemCard;
	public Button applyButton;

	public CharacterInfoText UpdateInfoPanel;
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
			ApplySynchro();
			UpdateInfoPanel.UpdateText();
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

		if(synchroInfoData == null)
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
			levelText.SetText($"<color=red>{currCharacter.CharacterLevel}</color>");
		}
		else
		{
			levelText.SetText($"{currCharacter.CharacterLevel}");
		}


		beforeGrade.SetText($"���� ��� : {grade}");
		afterGrade.SetText($"���� ��� : {grade + 1}");

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
	public void UpdateAfterSetCharacter()
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
