using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ModalWindow : MonoBehaviour
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

		GetComponent<Button>().onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
	}

	public void Show(string text, Action yesAction, string yesText = "", string noText = "")
	{
		var modalWindow = this;

		titleText.SetText(text);
		yesButton.GetComponentInChildren<TextMeshProUGUI>().SetText(yesText);
		noButton.gameObject.SetActive(true);
		noButton.GetComponentInChildren<TextMeshProUGUI>().SetText(noText);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener(() =>
		{
			yesAction();
			gameObject.SetActive(false);
			GameManager.Instance.SaveExecution();
		});

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});

		gameObject.SetActive(true);
	}

	public void Show(string text, string yesText = "")
	{
		var modalWindow = this;

		titleText.SetText(text);
		yesButton.GetComponentInChildren<TextMeshProUGUI>().SetText(yesText);
		noButton.gameObject.SetActive(false);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
			GameManager.Instance.SaveExecution();
		});

		gameObject.SetActive(true);
	}
}
