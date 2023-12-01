using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public void OnClickButtonExit()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene("GachaScene");
	}
}
