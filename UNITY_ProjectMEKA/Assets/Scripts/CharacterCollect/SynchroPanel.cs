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

	//버튼 누르면 실행됨
	public void SetCharacter(Character character)
	{
		if(character == null)
		{
            Debug.Log("캐릭터가 없습니다.");
            return;
        }

		currCharacter = character;

		var grade = currCharacter.CharacterGrade;
		var occupation = charTable.GetCharacterData(currCharacter.CharacterID).CharacterOccupation;

		synchroInfoData = synchroTable.GetSynchroData(grade, occupation);

		if(synchroInfoData == null)
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
			levelText.SetText($"<color=red>{currCharacter.CharacterLevel}</color>");
		}
		else
		{
			levelText.SetText($"{currCharacter.CharacterLevel}");
		}


		beforeGrade.SetText($"현재 등급 : {grade}");
		afterGrade.SetText($"다음 등급 : {grade + 1}");

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
			Debug.Log("합성 성공");
		}
		else
		{
			Debug.Log("실패");
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
			Debug.Log("재료가 모자랍니다.");
		}

		UpdateRequired();
		
		GameManager.Instance.SaveExecution();
	}

	public void OpenResultPanel()
	{
		Debug.Log("대충 패널 열렸다고 침");
		//resultPanel.gameObject.SetActive(true);

		//resultPanel 별 개수 세팅
	}
}
