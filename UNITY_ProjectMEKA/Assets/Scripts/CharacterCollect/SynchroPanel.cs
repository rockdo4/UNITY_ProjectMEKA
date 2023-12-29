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
	public Image characterImage;

	public ItemAutoQuantityCard[] synchroItemCard;
	public TextMeshProUGUI beforeLevel;
	public TextMeshProUGUI afterLevel;

	[Header("etc")]
	public Button applyButton;
	public CharacterInfoText infoPanel;

	private SynchroData synchroInfoData;
	private Character currCharacter;

	private CharacterTable charTable;
	private SynchroTable synchroTable;

	private StringTable stringTable;

	private void Awake()
	{
        stringTable = StageDataManager.Instance.stringTable;
        charTable = DataTableMgr.GetTable<CharacterTable>();
		synchroTable = DataTableMgr.GetTable<SynchroTable>();

		applyButton.onClick.AddListener(() =>
		{
			infoPanel.SetPopUpPanel(
				"싱크로하시겠습니까?",
				() =>
				{
					ApplySynchro();
					infoPanel.UpdateCharacter();
				}, 
				"예", "아니오"
				);
		});
	}

	//버튼 누르면 실행됨
	public void SetCharacter(Character character)
	{
		if(character == null)
		{
            Debug.Log("캐릭터가 없습니다.");
            return;
        }

		currCharacter = character;
        characterImage.sprite = Resources.Load<Sprite>(currCharacter.CharacterStanding);

        var grade = currCharacter.CharacterGrade;
		var occupation = charTable.GetCharacterData(currCharacter.CharacterID).CharacterOccupation;

		synchroInfoData = synchroTable.GetSynchroData(grade, occupation);

		
		var itemTable = DataTableMgr.GetTable<ItemInfoTable>();

		var tier1 = itemTable.GetItemData(synchroInfoData.Tier1ID);
		var tier2 = itemTable.GetItemData(synchroInfoData.Tier2ID);
		var tier3 = itemTable.GetItemData(synchroInfoData.Tier3ID);

        var tier1Name = stringTable.GetString(tier1.NameStringID);
        var tier2Name = stringTable.GetString(tier2.NameStringID);
        var tier3Name = stringTable.GetString(tier3.NameStringID);

        synchroItemCard[0].GetComponentInChildren<TextMeshProUGUI>().SetText(tier1Name);
		synchroItemCard[1].GetComponentInChildren<TextMeshProUGUI>().SetText(tier2Name);
		synchroItemCard[2].GetComponentInChildren<TextMeshProUGUI>().SetText(tier3Name);

		if (synchroInfoData == null)
		{
            Debug.Log("합성 정보 없음");
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

	//버튼 누르면 SetCharacter 다음으로 실행 
	public void CheckGrade()
	{
		if (currCharacter == null)
			gameObject.SetActive(false);

		if (currCharacter.CharacterGrade == MAX_SYNCHRO_GRADE)
		{
			gameObject.SetActive(false);

			//최대 등급인 경우 알람 팝업? 띄움
			//popup.SetActive(true);
		}
	}
	
	//아이템 카드 업데이트
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

		//아이템 체크
		foreach (var card in synchroItemCard)
		{
			isRequired = card.IsEnoughRequire();
		}

		//레벨 체크
		if(currCharacter.CharacterLevel < synchroInfoData.Grade * 10)
		{
			isRequired = false;
		}

		//은화 체크
		//isRequired = silverItemCard.IsEnoughRequire();

		if(isRequired)
		{
			currCharacter.CharacterGrade++;

			foreach (var card in synchroItemCard)
			{
				card.ConsumeItem();
				card.SetText();
			}

            infoPanel.SetNoticePanel("싱크로 성공", "확인");
        }
		else
		{
            infoPanel.SetNoticePanel("싱크로 실패", "확인");
        }

		UpdateRequired();
		
		GameManager.Instance.SaveExecution();
	}
}
