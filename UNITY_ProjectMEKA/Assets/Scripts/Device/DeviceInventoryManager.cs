using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInventoryManager
{
	private static DeviceInventoryManager instance;
	public SortedDictionary<int, Device> m_DeviceStorage;

	private DeviceInventoryManager()
	{
		//세이브 파일 로드
		//m_DeviceStorage = PlayDataManager.data.deviceStorage;
	}
	public static DeviceInventoryManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DeviceInventoryManager();
			}
			return instance;
		}
	}
	public int Count
	{
		get
		{
			return m_DeviceStorage.Count;
		}
	}

	public void AddDevice(Device item)
	{
		if(item == null)
		{
			Debug.LogError("Device is null");
		}
		else if(m_DeviceStorage.ContainsKey(item.InstanceID))
		{
			Debug.LogError("Device is already exist");
		}

		m_DeviceStorage.Add(item.InstanceID, item);
	}
}
