using System.Collections;
using System.Collections.Generic;
using System.Text;
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

	public DeviceEnhance deviceEnhancePanel;

	[Header("Scroll View")]
	public Transform scrollContent;
	public DeviceInfo devicePrefab;

	[Header("Character Info")]
	public TextMeshProUGUI characterName;
	public Button coreItem;
	public Button engineItem;


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

		equipButton.onClick.AddListener(OnClickEquip_UnEquip);
		enhanceButton.onClick.AddListener(() =>
		{
			deviceEnhancePanel.gameObject.SetActive(true);
			deviceEnhancePanel.SetDeivce(selectedDevice);
		});
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
	
	public void CreateCore()
	{
		CreateDevice(1);
		UpdateDeviceCard();

		GameManager.Instance.SaveExecution();
	}

	public void CreateEngine()
	{
		CreateDevice(2);
		UpdateDeviceCard();

		GameManager.Instance.SaveExecution();
	}

	public void SetCharacter(Character character)
	{
		if(character == null)
		{
			Debug.Log("캐릭터가 없습니다.");
			return;
		}

		deviceDict = DeviceInventoryManager.Instance.m_DeviceStorage;
		currCharacter = character;
		characterName.SetText(currCharacter.Name);

		UpdateEquipedDeivce();
		UpdateDeviceCard();
	}

	public void UpdateEquipedDeivce()
	{
		if(currCharacter.DeviceCoreID != 0)
		{
			coreItem.GetComponentInChildren<TextMeshProUGUI>().SetText(deviceDict[currCharacter.DeviceCoreID].Name);
			coreItem.onClick.RemoveAllListeners();
			coreItem.onClick.AddListener(() =>
			{
				SetDeviceInfoText(deviceDict[currCharacter.DeviceCoreID]);
				selectedDevice = deviceDict[currCharacter.DeviceCoreID];
			});
		}
		else
		{
			coreItem.GetComponentInChildren<TextMeshProUGUI>().SetText("비어있음");
			coreItem.onClick.RemoveAllListeners();
			coreItem.onClick.AddListener(() =>
			{
				SetDeviceInfoText(null);
				selectedDevice = null;
			});
		}

		if(currCharacter.DeviceEngineID != 0)
		{
			engineItem.GetComponentInChildren<TextMeshProUGUI>().SetText(deviceDict[currCharacter.DeviceEngineID].Name);
			engineItem.onClick.RemoveAllListeners();
			engineItem.onClick.AddListener(() =>
			{
				SetDeviceInfoText(deviceDict[currCharacter.DeviceEngineID]);
				selectedDevice = deviceDict[currCharacter.DeviceEngineID];
			});
		}
		else
		{
			engineItem.GetComponentInChildren<TextMeshProUGUI>().SetText("비어있음");
			engineItem.onClick.RemoveAllListeners();
			engineItem.onClick.AddListener(() =>
			{
				SetDeviceInfoText(null);
				selectedDevice = null;
			});
		}
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

		StringBuilder sb = new StringBuilder();
		sb.Append(71);
		sb.Append(DeviceInventoryManager.Instance.Count.ToString("0000"));

		int.TryParse(sb.ToString(), out int id);

		device.InstanceID = id;
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

			if(device.Value.TargetCharacterID != 0)
			{
				item.SetActive(false);
			}
			else
			{
				item.SetActive(true);
			}
		}
	}

	public void SetDeviceInfoText(Device device)
	{
		if(device == null)
		{
			deviceName.SetText("장비를 선택해주세요.");
			level.SetText("--");
			type.SetText("--");

			mainOption.SetText("--");
			mainOptionValue.SetText("--");

			subOption1.SetText("--");
			subOptionValue1.SetText("--");

			subOption2.SetText("--");
			subOptionValue2.SetText("--");

			subOption3.SetText("--");
			subOptionValue3.SetText("--");

			return;
		}


		deviceName.SetText(device.Name);
		level.SetText(device.CurrLevel.ToString());
		type.SetText(device.PartType.ToString());

		mainOption.SetText(deviceOptionTable.GetDeviceOptionData(device.MainOptionID).Name);

		float mainValue = deviceValueTable.GetDeviceValueData(device.MainOptionID).Coefficient;
		if(mainValue != 0)
		{
			mainValue += deviceValueTable.GetDeviceValueData(device.MainOptionID).Increase * (device.CurrLevel - 1);
			mainOptionValue.SetText(mainValue.ToString() + "%");
		}
		else
		{
			mainValue = deviceValueTable.GetDeviceValueData(device.MainOptionID).Value;
			mainValue += deviceValueTable.GetDeviceValueData(device.MainOptionID).Increase * (device.CurrLevel - 1);
			mainOptionValue.SetText(mainValue.ToString());
		}

		//

		subOption1.SetText(deviceOptionTable.GetDeviceOptionData(device.SubOption1ID).Name);

		float subValue1 = deviceValueTable.GetDeviceValueData(device.SubOption1ID).Coefficient;
		if (subValue1 != 0)
		{
			subValue1 += deviceValueTable.GetDeviceValueData(device.SubOption1ID).Increase * (device.CurrLevel - 1);
			subOptionValue1.SetText(subValue1.ToString() + "%");
		}
		else
		{
			subValue1 = deviceValueTable.GetDeviceValueData(device.SubOption1ID).Value;
			subValue1 += deviceValueTable.GetDeviceValueData(device.SubOption1ID).Increase * (device.CurrLevel - 1);
			subOptionValue1.SetText(subValue1.ToString());
		}

		//

		subOption2.SetText(deviceOptionTable.GetDeviceOptionData(device.SubOption2ID).Name);

		float subValue2 = deviceValueTable.GetDeviceValueData(device.SubOption2ID).Coefficient;
		if (subValue2 != 0)
		{
			subValue2 += deviceValueTable.GetDeviceValueData(device.SubOption2ID).Increase * (device.CurrLevel - 1);
			subOptionValue2.SetText(subValue2.ToString() + "%");
		}
		else
		{
			subValue2 = deviceValueTable.GetDeviceValueData(device.SubOption2ID).Value;
			subValue2 += deviceValueTable.GetDeviceValueData(device.SubOption2ID).Increase * (device.CurrLevel - 1);
			subOptionValue2.SetText(subValue2.ToString());
		}

		//

		if (device.SubOption3ID == 0)
		{
			subOption3.SetText("10레벨에 해금");
			subOptionValue3.SetText("--");
		}
		else
		{
			float subValue3 = deviceValueTable.GetDeviceValueData(device.SubOption3ID).Coefficient;
			if (subValue3 != 0)
			{
				subValue3 += deviceValueTable.GetDeviceValueData(device.SubOption3ID).Increase * (device.CurrLevel - 1);
				subOptionValue3.SetText(subValue3.ToString() + "%");
			}
			else
			{
				subValue3 = deviceValueTable.GetDeviceValueData(device.SubOption3ID).Value;
				subValue3 += deviceValueTable.GetDeviceValueData(device.SubOption3ID).Increase * (device.CurrLevel - 1);
				subOptionValue3.SetText(subValue3.ToString());
			}
		}
	}

	public void UpdateDeviceAfterUpgrade()
	{
		if(selectedDevice != null)
		{
			SetDeviceInfoText(selectedDevice);
		}
	}

	public void OnClickEquip_UnEquip()
	{
		if(selectedDevice == null)
		{
			Debug.Log("장비를 선택해주세요.");
			return;
		}

		//이미 어딘가 장착되었을 때
		//원래 장착된 곳에서 해제하고 현재 캐릭터에 장착
		//아이템의 타겟 캐릭터가 현재 캐릭터가 다르면 장착
		if(selectedDevice.IsEquipped && selectedDevice.TargetCharacterID != currCharacter.CharacterID)
		{
			var target = selectedDevice.TargetCharacterID;
			var character = CharacterManager.Instance.m_CharacterStorage[target];

			if(character == null)
			{
				Debug.Log("장착된 캐릭터가 없습니다.");
				return;
			}

			if(selectedDevice.PartType == (int)DevicePartType.Engine)
			{
				DeviceInventoryManager.Instance.m_DeviceStorage[currCharacter.DeviceEngineID].IsEquipped = false;
				character.DeviceEngineID = 0;
				currCharacter.DeviceEngineID = selectedDevice.InstanceID;
				selectedDevice.TargetCharacterID = currCharacter.CharacterID;
			}
			else if(selectedDevice.PartType == (int)DevicePartType.Core)
			{
				DeviceInventoryManager.Instance.m_DeviceStorage[currCharacter.DeviceCoreID].IsEquipped = false;
				character.DeviceCoreID = 0;
				currCharacter.DeviceCoreID = selectedDevice.InstanceID;
				selectedDevice.TargetCharacterID = currCharacter.CharacterID;
			}

			Debug.Log("기존 캐릭터에서 해제하고 장착 완료");
		}
		//아이템이 장착되어있고, 타겟 캐릭터가 현재 캐릭터면 장착 해제
		else if(selectedDevice.IsEquipped && selectedDevice.TargetCharacterID == currCharacter.CharacterID)
		{
			if(selectedDevice.PartType == (int)DevicePartType.Engine)
			{
				currCharacter.DeviceEngineID = 0;
				selectedDevice.TargetCharacterID = 0;
			}
			
			if(selectedDevice.PartType == (int)DevicePartType.Core)
			{
				currCharacter.DeviceCoreID = 0;
				selectedDevice.TargetCharacterID = 0;
			}

			selectedDevice.IsEquipped = false;
			Debug.Log("장착 해제");
		}
		//장착 안되어있으면,
		else if(!selectedDevice.IsEquipped)
		{

			if(selectedDevice.PartType == (int)DevicePartType.Engine)
			{
				if(currCharacter.DeviceEngineID != 0)
				{
					DeviceInventoryManager.Instance.m_DeviceStorage[currCharacter.DeviceEngineID].TargetCharacterID = 0;
					DeviceInventoryManager.Instance.m_DeviceStorage[currCharacter.DeviceEngineID].IsEquipped = false;
				}

				currCharacter.DeviceEngineID = selectedDevice.InstanceID;
				selectedDevice.TargetCharacterID = currCharacter.CharacterID;
				selectedDevice.IsEquipped = true;
			}
			else if(selectedDevice.PartType == (int)DevicePartType.Core)
			{
				if (currCharacter.DeviceCoreID != 0)
				{
					DeviceInventoryManager.Instance.m_DeviceStorage[currCharacter.DeviceCoreID].TargetCharacterID = 0;
					DeviceInventoryManager.Instance.m_DeviceStorage[currCharacter.DeviceCoreID].IsEquipped = false;
				}

				currCharacter.DeviceCoreID = selectedDevice.InstanceID;
				selectedDevice.TargetCharacterID = currCharacter.CharacterID;
				selectedDevice.IsEquipped = true;
			}

			Debug.Log("장착");
		}

		UpdateEquipedDeivce();
		UpdateDeviceCard();
		GameManager.Instance.SaveExecution();
	}

	public void CheckPlayData()
	{
		DeviceInventoryManager.Instance.m_DeviceStorage = PlayDataManager.data.deviceStorage;
	}
}