using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public void OnClickButtonExit()
    {
        Time.timeScale = 1.0f;
		UnityEngine.SceneManagement.SceneManager.LoadScene("GachaScene");
	}
}
