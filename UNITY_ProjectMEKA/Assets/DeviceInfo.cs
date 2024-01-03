using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeviceInfo : MonoBehaviour
{
	private Device device;
	private TextMeshProUGUI text;
	private Image image;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		image = GetComponent<Image>();
	}

	public void SetDevice(Device device)
	{
		this.device = device;

		var itemInfoTable = DataTableMgr.GetTable<ItemInfoTable>();

		if(device.PartType == 1)
		{
			image.sprite = Resources.Load<Sprite>(itemInfoTable.GetItemData(88).ImagePath);
		}
		else if(device.PartType == 2)
		{
			image.sprite = Resources.Load<Sprite>(itemInfoTable.GetItemData(99).ImagePath);
		}

	}

	public void SetText()
	{
		text.SetText(device.Name);
	}

	public Device GetDevice()
	{
		return device;
	}
}
