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
		Button[] buttons = FindObjectsOfType<Button>(); // Scene���� ��� Button ��� ã��

		foreach (Button button in buttons)
		{
			Navigation navigation = button.navigation; // ���� ��ư�� Navigation ���� ��������
			navigation.mode = Navigation.Mode.None; // Navigation ��带 None���� ����
			button.navigation = navigation; // ����� Navigation ���� ����
		}

		Debug.Log("All Button Navigation Changed to None");
	}

	[MenuItem("Tools/Change All InputField Navigation")]
	public static void ChangeAllInPutFieldNavigation()
	{
		InputField[] inputFields = FindObjectsOfType<InputField>(); // Scene���� ��� InputField ��� ã��

		foreach (InputField inputField in inputFields)
		{
			Navigation navigation = inputField.navigation; // ���� ��ư�� Navigation ���� ��������
			navigation.mode = Navigation.Mode.None; // Navigation ��带 None���� ����
			inputField.navigation = navigation; // ����� Navigation ���� ����
		}

		Debug.Log("All InputField Navigation Changed to None");
	}

	[MenuItem("Tools/Change All DropDown Navigation")]
	public static void ChangeAllDropDownNavigation()
	{
		Dropdown[] dropdowns = FindObjectsOfType<Dropdown>(); // Scene���� ��� Dropdown ��� ã��

		foreach (Dropdown dropdown in dropdowns)
		{
			Navigation navigation = dropdown.navigation; // ���� ��ư�� Navigation ���� ��������
			navigation.mode = Navigation.Mode.None; // Navigation ��带 None���� ����
			dropdown.navigation = navigation; // ����� Navigation ���� ����
		}

		Debug.Log("All Dropdown Navigation Changed to None");
	}
}
