using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public Button backButton;

	public void LoadMainScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("GachaScene");
	}
}
