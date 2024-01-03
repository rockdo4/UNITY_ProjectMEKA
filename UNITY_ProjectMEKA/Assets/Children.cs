using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Children : MonoBehaviour
{
	public Transform tr;

	private void Start()
	{
		tr = GetComponent<Transform>();
	}
}
