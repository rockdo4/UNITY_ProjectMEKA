using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSChecker : MonoBehaviour
{
	[Range(1, 100)]
	public int fFont_Size;
	[Range(0, 1)]
	public float Red, Green, Blue;

	float deltaTime = 0.0f;
	private bool showFPS = true;


	private void Start()
	{
		fFont_Size = fFont_Size == 0 ? 50 : fFont_Size;
	}

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

		if(Input.GetKeyDown(KeyCode.F1))
		{
			showFPS = !showFPS;
		}

		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			
		}
	}

	void OnGUI()
	{
		if (!showFPS) return;

		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(Screen.safeArea.x, Screen.safeArea.y, w, h * 0.02f);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / fFont_Size;
		style.normal.textColor = new Color(Red, Green, Blue, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
	}
}
