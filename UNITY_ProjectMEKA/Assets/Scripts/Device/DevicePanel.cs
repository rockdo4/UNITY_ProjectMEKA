using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DevicePartType
{
	Core = 1,
	Engine = 2,
}

public class DevicePanel : MonoBehaviour
{
	private GachaSystem<int> coreOption;
	private GachaSystem<int> engineOption;
	private GachaSystem<int> subOption;

	private DeviceOptionTable deviceOptionTable;
	private DeviceValueTable deviceValueTable;

	private Dictionary<int, Device> deviceDict;

	public Transform characterInfoPanel;

	[Header("Scroll View")]
	public Transform scrollContent;
	public DeviceInfo devicePrefab;

	[Header("Character Info")]
	public TextMeshProUGUI characterName;


	[Header("Device Info")]
	public TextMeshProUGUI deviceName;
	public TextMeshProUGUI level;
	public TextMeshProUGUI type;
	public TextMeshProUGUI mainOption;
	public TextMeshProUGUI mainOptionValue;
	public TextMeshProUGUI subOption1;
	public TextMeshProUGUI subOptionValue1;
	public TextMeshProUGUI subOption2;
	public TextMeshProUGUI subOptionValue2;
	public TextMeshProUGUI subOption3;
	public TextMeshProUGUI subOptionValue3;

	public Button equipButton;
	public Button enhanceButton;

	private Device selectedDevice;

	private Character currCharacter;


	private void Awake()
	{
		coreOption = new GachaSystem<int>();
		engineOption = new GachaSystem<int>();
		subOption = new GachaSystem<int>();
	}

	private void Start()
	{
		deviceDict = DeviceInventoryManager.Instance.m_DeviceStorage;
		deviceOptionTable = DataTableMgr.GetTable<DeviceOptionTable>();
		deviceValueTable = DataTableMgr.GetTable<DeviceValueTable>();

		var coreOptions = deviceOptionTable.GetOrigianlCoreTable();
		var engineOptions = deviceOptionTable.GetOrigianlEngineTable();
		var subOptions = deviceOptionTable.GetOrigianlSubTable();

		foreach (var item in coreOptions)
		{
			coreOption.Add(item.Key, item.Value.Weight);
			//Debug.Log((item.Key, item.Value.Weight));
		}
		foreach (var item in engineOptions)
		{
			engineOption.Add(item.Key, item.Value.Weight);
			//Debug.Log((item.Key, item.Value.Weight));
		}
		foreach (var item in subOptions)
		{
			subOption.Add(item.Key, item.Value.Weight);
			//Debug.Log((item.Key, item.Value.Weight));
		}

		CheckPlayData();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CreateDevice(1);
			UpdateDeviceCard();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			CreateDevice(2);
			UpdateDeviceCard();
		}
	}

	public void SetCharacter(Character character)
	{
		if(character == null)
		{
			Debug.Log("캐릭터가 없습니다.");
			return;
		}

		currCharacter = character;
		characterName.SetText(currCharacter.Name);

		UpdateDeviceCard();
	}

	public void CreateDevice(int PartType)
	{
		var device = new Device();

		switch (PartType)
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

		while (true)
		{
			var subOption1 = subOption.GetItem();
			var isSame = CheckSameOption(device.MainOptionID, subOption1);

			if (!isSame)
			{
				device.SubOption1ID = subOption1;
				break;
			}
		}

		while (true)
		{
			var subOption2 = subOption.GetItem();
			var isSame = CheckSameOption(device.MainOptionID, subOption2);

			if (!isSame)
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


		DeviceInventoryManager.Instance.AddDevice(device);
	}

	private bool CheckSameOption(int main, int sub)
	{
		var mainOption = deviceOptionTable.GetDeviceOptionData(main);
		var subOption = deviceOptionTable.GetDeviceOptionData(sub);

		if (mainOption == null || subOption == null)
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

	public void UpdateDeviceCard(Dictionary<int, Device> devices = null)
	{
		var items = scrollContent.GetComponentsInChildren<DeviceInfo>();

		if(devices == null)
		{
			devices = DeviceInventoryManager.Instance.m_DeviceStorage;
		}

		foreach(var item in items)
		{
			item.GetComponent<Button>().onClick.RemoveAllListeners();
			item.GetComponent<PoolAble>().ReleaseObject();
		}

		int count = 0;
		foreach(var device in devices)
		{
			var item = ObjectPoolManager.instance.GetGo("Device");
			var text = item.GetComponentInChildren<TextMeshProUGUI>();
			var str = device.Value.Name;

			text.SetText(device.Value.PartType.ToString());
			item.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log(("이름: " + str, "인스턴스아이디: " + device.Value.InstanceID, "레벨: " + device.Value.CurrLevel, "설명: " + device.Value.Description));
				SetDeviceInfoText(device.Value);
				selectedDevice = device.Value;
			});
			item.name = device.Value.Name;
			item.transform.SetParent(scrollContent, false);
			item.gameObject.name = count++.ToString();
			item.transform.SetAsLastSibling();
		}
	}

	public void SetDeviceInfoText(Device device)
	{
		deviceName.SetText(device.Name);
		level.SetText(device.CurrLevel.ToString());
		type.SetText(device.PartType.ToString());

		mainOption.SetText(deviceOptionTable.GetDeviceOptionData(device.MainOptionID).Name);

		float mainValue = deviceValueTable.GetDeviceValueData(device.MainOptionID).Coefficient;
		if(mainValue == 0)
		{
			mainValue = deviceValueTable.GetDeviceValueData(device.MainOptionID).Value;
		}
		mainValue += deviceValueTable.GetDeviceValueData(device.MainOptionID).Increase * (device.CurrLevel - 1);
		mainOptionValue.SetText(mainValue.ToString());

		//

		subOption1.SetText(deviceOptionTable.GetDeviceOptionData(device.SubOption1ID).Name);

		float subValue1 = deviceValueTable.GetDeviceValueData(device.SubOption1ID).Coefficient;
		if (subValue1 == 0)
		{
			subValue1 = deviceValueTable.GetDeviceValueData(device.SubOption1ID).Value;
		}
		subValue1 += deviceValueTable.GetDeviceValueData(device.SubOption1ID).Increase * (device.CurrLevel - 1);
		subOptionValue1.SetText(subValue1.ToString());

		//

		subOption2.SetText(deviceOptionTable.GetDeviceOptionData(device.SubOption2ID).Name);

		float subValue2 = deviceValueTable.GetDeviceValueData(device.SubOption2ID).Coefficient;
		if (subValue2 == 0)
		{
			subValue2 = deviceValueTable.GetDeviceValueData(device.SubOption2ID).Value;
		}
		subValue2 += deviceValueTable.GetDeviceValueData(device.SubOption2ID).Increase * (device.CurrLevel - 1);
		subOptionValue2.SetText(subValue2.ToString());

		//

		if (device.SubOption3ID == 0)
		{
			subOption3.SetText("10레벨에 해금");
			subOptionValue3.SetText("--");
		}
		else
		{
			subOption3.SetText(deviceOptionTable.GetDeviceOptionData(device.SubOption3ID).Name);

			float subValue3 = deviceValueTable.GetDeviceValueData(device.SubOption3ID).Coefficient;
			if (subValue3 == 0)
			{
				subValue3 = deviceValueTable.GetDeviceValueData(device.SubOption3ID).Value;
			}
			subValue3 += deviceValueTable.GetDeviceValueData(device.SubOption3ID).Increase * (device.CurrLevel - 1);
			subOptionValue3.SetText(subValue3.ToString());
		}
	}

	public void OnClickEquip()
	{
		if(selectedDevice.IsEquipped)
		{
			Debug.Log("이미 장착된 장비입니다.");
			return;
		}
		else
		{
			selectedDevice.IsEquipped = true;
			
			if(selectedDevice.PartType == (int)DevicePartType.Core)
			{
				currCharacter.DeviceCoreID = selectedDevice.InstanceID;
			}
			else if(selectedDevice.PartType == (int)DevicePartType.Engine)
			{
				currCharacter.DeviceEngineID = selectedDevice.InstanceID;
			}
		}
	}

	public void CheckPlayData()
	{
		DeviceInventoryManager.Instance.m_DeviceStorage = PlayDataManager.data.deviceStorage;
	}
}