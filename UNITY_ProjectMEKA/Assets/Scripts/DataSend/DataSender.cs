using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSender : MonoBehaviour
{
	public static int[] arr;

	private void Start()
	{
		arr = new int[5] { 1, 2, 3, 4, 5 };
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			LoadSceneB();
		}
	}

	public void LoadSceneB()
	{
		SceneManager.LoadScene("B", LoadSceneMode.Single);
	}
}
