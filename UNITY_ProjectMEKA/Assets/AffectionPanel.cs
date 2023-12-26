using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
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

	[Header("Etc")]
	public Button closeButton;
	public ModalWindow modalWindow;

	private Character currCharacter;

	private void Awake()
	{
		closeButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
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

			Debug.Log("호감도 레벨업");
		}

		currCharacter.affection.LastTime = System.DateTime.Now;

		UpdateCharacter();
		NoticeAffection(point);
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
}
