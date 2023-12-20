using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ModalWIndow : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Button yesButton;
    public Button noButton;

	private void Awake()
	{
		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
	}

	public static void Show(string titleText, Action yesAction, string yesText = null, string noText = null)
	{
		var modalWindow = FindObjectOfType<ModalWIndow>();
		modalWindow.titleText.SetText(titleText);
		modalWindow.yesButton.GetComponentInChildren<TextMeshProUGUI>().SetText(yesText);
		modalWindow.noButton.GetComponentInChildren<TextMeshProUGUI>().SetText(noText);

		modalWindow.yesButton.onClick.RemoveAllListeners();
		modalWindow.yesButton.onClick.AddListener(() =>
		{
			yesAction();
			modalWindow.gameObject.SetActive(false);
		});

		modalWindow.noButton.onClick.RemoveAllListeners();
		modalWindow.noButton.onClick.AddListener(() =>
		{
			modalWindow.gameObject.SetActive(false);
		});

		modalWindow.gameObject.SetActive(true);
	}
}
