using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNavigation : EditorWindow
{
	[MenuItem("Tools/Change All Button Navigation")]
	public static void ChangeAllButtonNavigation()
	{
		Button[] buttons = FindObjectsOfType<Button>(); // Scene에서 모든 Button 요소 찾기

		foreach (Button button in buttons)
		{
			Navigation navigation = button.navigation; // 현재 버튼의 Navigation 정보 가져오기
			navigation.mode = Navigation.Mode.None; // Navigation 모드를 None으로 변경
			button.navigation = navigation; // 변경된 Navigation 정보 적용
		}

		Debug.Log("All Button Navigation Changed to None");
	}

	[MenuItem("Tools/Change All InputField Navigation")]
	public static void ChangeAllInPutFieldNavigation()
	{
		InputField[] inputFields = FindObjectsOfType<InputField>(); // Scene에서 모든 InputField 요소 찾기

		foreach (InputField inputField in inputFields)
		{
			Navigation navigation = inputField.navigation; // 현재 버튼의 Navigation 정보 가져오기
			navigation.mode = Navigation.Mode.None; // Navigation 모드를 None으로 변경
			inputField.navigation = navigation; // 변경된 Navigation 정보 적용
		}

		Debug.Log("All InputField Navigation Changed to None");
	}

	[MenuItem("Tools/Change All DropDown Navigation")]
	public static void ChangeAllDropDownNavigation()
	{
		Dropdown[] dropdowns = FindObjectsOfType<Dropdown>(); // Scene에서 모든 Dropdown 요소 찾기

		foreach (Dropdown dropdown in dropdowns)
		{
			Navigation navigation = dropdown.navigation; // 현재 버튼의 Navigation 정보 가져오기
			navigation.mode = Navigation.Mode.None; // Navigation 모드를 None으로 변경
			dropdown.navigation = navigation; // 변경된 Navigation 정보 적용
		}

		Debug.Log("All Dropdown Navigation Changed to None");
	}
}
