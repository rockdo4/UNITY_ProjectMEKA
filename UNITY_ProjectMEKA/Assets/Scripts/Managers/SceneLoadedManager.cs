using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadedManager : MonoBehaviour
{
    public Button button;
	public Canvas canvas;

	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		button.onClick.AddListener(() => 
		{
			Debug.Log("Button Clicked");
		});
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded(scene, mode);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		StartCoroutine(BlockClick());
	}

	IEnumerator BlockClick()
	{
		canvas.sortingOrder = 1;
		button.gameObject.SetActive(true);

		yield return new WaitForSeconds(1.5f);

		button.gameObject.SetActive(false);
		canvas.sortingOrder = -1;
	}
}
