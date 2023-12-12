using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
	private GachaSystem<int> coreOption;
	private GachaSystem<int> engineOption;
	private GachaSystem<int> subOption;

	private DeviceOptionTable deviceOptionTable;

	private void Awake()
	{
		coreOption = new GachaSystem<int>();
		engineOption = new GachaSystem<int>();
		subOption = new GachaSystem<int>();
	}

	private void Start()
	{
		deviceOptionTable = DataTableMgr.GetTable<DeviceOptionTable>();

		var coreOptions = deviceOptionTable.GetOrigianlCoreTable();
		var engineOptions = deviceOptionTable.GetOrigianlEngineTable();
		var subOptions = deviceOptionTable.GetOrigianlSubTable();

		foreach(var item in coreOptions)
		{
			coreOption.Add(item.Key, item.Value.Weight);
			//Debug.Log((item.Key, item.Value.Weight));
		}
		foreach(var item in engineOptions)
		{
			engineOption.Add(item.Key, item.Value.Weight);
			//Debug.Log((item.Key, item.Value.Weight));
		}
		foreach(var item in subOptions)
		{
			subOption.Add(item.Key, item.Value.Weight);
			//Debug.Log((item.Key, item.Value.Weight));
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			CreateDevice(1);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			CreateDevice(2);
		}
	}

	public void CreateDevice(int PartType)
	{
		var device = new Device();

        switch(PartType)
		{
			case 1:
				device.MainOptionID = coreOption.GetItem();
				break;
			case 2:
				device.MainOptionID = engineOption.GetItem();
				break;
			default:
				Debug.LogError("PartType is not valid");
				break;	
		}

		device.InstanceID = DeviceInventoryManager.Instance.Count;
		device.Name = "아이템";
		device.Description = "장비템임";
		device.CurrLevel = 1;
		device.MaxLevel = 10;
		device.PartType = PartType;

		while(true)
		{
			var subOption1 = subOption.GetItem();
			var isSame = CheckSameOption(device.MainOptionID, subOption1);

			if(!isSame)
			{
				device.SubOption1ID = subOption1;
				break;
			}
		}

		while(true)
		{
			var subOption2 = subOption.GetItem();
			var isSame = CheckSameOption(device.MainOptionID, subOption2);

			if(!isSame)
			{
				device.SubOption2ID = subOption2;
				break;
			}
		}

		device.SubOption3ID = 0;

		Debug.Log((device.InstanceID, device.Name, device.Description));

		var Option = deviceOptionTable.GetDeviceOptionData(device.MainOptionID).Name;
		var Option1 = deviceOptionTable.GetDeviceOptionData(device.SubOption1ID).Name;
		var Option2 = deviceOptionTable.GetDeviceOptionData(device.SubOption2ID).Name;

		Debug.Log((Option, Option1, Option2));


		//DeviceInventoryManager.Instance.AddDevice(device);
	}

	private bool CheckSameOption(int main, int sub)
	{
		var mainOption = deviceOptionTable.GetDeviceOptionData(main);
		var subOption = deviceOptionTable.GetDeviceOptionData(sub);

		if(mainOption == null || subOption == null)
		{
			throw new System.Exception("Option is null");
		}

		var mOption = mainOption.Name.Replace(" ", "");
		var sOption = subOption.Name.Replace(" ", "");

		if (mOption.Equals(sOption))
			return true;
		else
			return false;
	}
}