using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    public Image pressStart;

    private void Start()
    {
		StartCoroutine(StartBlink());
	}

    IEnumerator StartBlink()
    {
		while (true)
        {
			pressStart.enabled = true;
			yield return new WaitForSeconds(0.5f);
			pressStart.enabled = false;
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			StopAllCoroutines();
			pressStart.enabled = true;
			SceneManager.LoadScene("MainScene");
		}
	}
}