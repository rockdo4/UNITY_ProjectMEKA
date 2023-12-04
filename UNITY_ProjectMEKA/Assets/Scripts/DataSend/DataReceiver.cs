using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataReceiver : MonoBehaviour
{
	public List<int[]> data = new List<int[]>();

	public void OnEnable()
	{
		int[] receiveArr = DataSender.arr;

		foreach (int i in receiveArr)
		{
			Debug.Log(i);
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			DisplayLog();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			SceneManager.LoadScene("A", LoadSceneMode.Single);
		}
	}

	public void DisplayLog()
	{
		int[] receiveArr = DataSender.arr;

		foreach (int i in receiveArr)
		{
			Debug.Log(i);
		}
	}
}
