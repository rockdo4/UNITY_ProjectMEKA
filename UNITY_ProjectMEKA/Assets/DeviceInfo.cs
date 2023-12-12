using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeviceInfo : MonoBehaviour
{
	private Device device;
	private TextMeshProUGUI text;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void SetDevice(Device device)
	{
		this.device = device;
	}

	public void SetText()
	{
		text.SetText(device.Name);
	}
}
