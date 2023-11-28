using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public GameObject popup;
	public TextMeshProUGUI popupText;

	private void Awake()
	{
		popup.SetActive(false);
	}

	public void OnClickDeleteButton()
	{
		popup.SetActive(true);

		popupText.SetText("Are you sure you want \n" +
			"to delete this preset?");
	}
}
