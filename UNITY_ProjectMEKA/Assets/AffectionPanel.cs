using System.Collections;
using System.Collections.Generic;
using TMPro;
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

	public void UpdateCharacter(Character character)
	{
		currCharacter = character;
		if (currCharacter == null)
		{
			return;
		}

		characterImage.sprite = Resources.Load<Sprite>("CharacterIcon/" + character.ImagePath);
		characterImage.preserveAspect = true;
		characterName.SetText(character.Name);
		affectionText.SetText($"Lv : {0}");
		affectionSlider.fillAmount = 0.5f;


		//var affection = currCharacter.Affection;

		//var affectionText = GetComponentsInChildren<Text>()[0];
		//var affectionSlider = GetComponentsInChildren<Slider>()[0];

		//affectionText.text = affection.ToString();
		//affectionSlider.value = affection;
	}
}
