using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeviceEnhance : MonoBehaviour
{
	[Header("소모 아이템 ID")]
	public int[] itemID;

	public Image deviceImage;
	public TextMeshProUGUI textDeviceName;

	public TextMeshProUGUI textLevel;
	public TextMeshProUGUI textExp;
	public Image imageExp;

	[Header("OptionName")]
	public TextMeshProUGUI textMainOptionName;
	public TextMeshProUGUI textSubOption1Name;
	public TextMeshProUGUI textSubOption2Name;
	public TextMeshProUGUI textSubOption3Name;

	[Header("before")]
	public TextMeshProUGUI textMainOption;
	public TextMeshProUGUI textSubOption1;
	public TextMeshProUGUI textSubOption2;
	public TextMeshProUGUI textSubOption3;

	[Header("after")]
	public TextMeshProUGUI afterMainOption;
	public TextMeshProUGUI afterSubOption1;
	public TextMeshProUGUI afterSubOption2;
	public TextMeshProUGUI afterSubOption3;

	public ItemQuantityCardDevice[] itemCard;
	public Button applyButton;
	public DevicePanel devicePanel;

	private Device currDevice;
    private List<DeviceExpData> expData;
	private int currLevel;
	private StringTable stringTable;
	private bool isFull = false;

	private const int engineID = 99;
	private const int coreID = 88;

	private void Awake()
	{
		applyButton.onClick.AddListener(() =>
		{
			devicePanel.characterInfoPanel.GetComponent<CharacterInfoText>().SetPopUpPanel("강화하시겠습니까?", () =>
			{
				ExecuteUpgradeDevice();
			}
			, "예", "아니오");
		});

		expData = DataTableMgr.GetTable<DeviceExpTable>().GetOriginalTable();
		stringTable = StageDataManager.Instance.stringTable;

    }

	private void Start()
	{
		var itemTable = DataTableMgr.GetTable<ItemInfoTable>();

		var tier1 = itemTable.GetItemData(itemID[0]);
		var tier2 = itemTable.GetItemData(itemID[1]);
		var tier3 = itemTable.GetItemData(itemID[2]);
	}

	private void OnEnable()
	{
		for (int i = 0; i < itemCard.Length; i++)
		{
			itemCard[i].SetItem(itemID[i]);
			itemCard[i].SetText();
		}
		isFull = false;
	}

	public void ExecuteUpgradeDevice()
	{
		ApplyUpgradeLevel();

		foreach (var card in itemCard)
		{
			card.ConsumeItem();
			card.SetText();
		}
		UpdateTargetLevel();
	}

	public void SetDeivce(Device device)
	{
		currDevice = device;
		currLevel = device.CurrLevel;

		var table = DataTableMgr.GetTable<ItemInfoTable>();
		var stringTable = StageDataManager.Instance.stringTable;

		if(currDevice.PartType == 1)
		{
			deviceImage.sprite = Resources.Load<Sprite>(table.GetItemData(99).ImagePath);
			textDeviceName.SetText(stringTable.GetString(table.GetItemData(99).NameStringID));
		}
		else if(currDevice.PartType == 2)
		{
			deviceImage.sprite = Resources.Load<Sprite>(table.GetItemData(88).ImagePath);
			textDeviceName.SetText(stringTable.GetString(table.GetItemData(88).NameStringID));
		}
		

		UpdateTargetLevel();
	}

	public int CalculateExp(int totalExp, out int remain)
	{
		int currentLevel = currDevice.CurrLevel;
		int targetLevel = currentLevel;
		int maxLevel = expData.Count;

		while (totalExp > 0)
		{
			if (totalExp >= expData[targetLevel - 1].RequireExp)
			{
				totalExp -= expData[targetLevel - 1].RequireExp;
				targetLevel++;
			}
			else
			{
				break;
			}

			if (targetLevel > maxLevel)
			{
				targetLevel--;
				break;
			}
		}

		remain = totalExp;
		return targetLevel;
	}

	public void ApplyUpgradeLevel()
	{
		int totalExp = 0;
		int remainExp = 0;
		totalExp += currDevice.CurrExp;

		foreach(var card in itemCard)
		{
			int value = 0;
			if(card.item != null)
			{
				value = card.item.Value;
			}
			totalExp += (card.selectedQuantity * value);
		}

		var targetLevel = CalculateExp(totalExp, out remainExp);

		currDevice.CurrLevel = targetLevel;
		currDevice.CurrExp = remainExp;

		var table = DataTableMgr.GetTable<DeviceValueTable>();

		var main = table.GetDeviceValueData(currDevice.MainOptionID);
		var sub1 = table.GetDeviceValueData(currDevice.SubOption1ID);
		var sub2 = table.GetDeviceValueData(currDevice.SubOption2ID);
		var sub3 = table.GetDeviceValueData(currDevice.SubOption3ID);

		textLevel.SetText($"{currDevice.CurrLevel}");
		var ratio = (float)remainExp / expData[targetLevel - 1].RequireExp;
		if (ratio > 1)
			ratio = 1;

		textExp.SetText($"{(int)(ratio * 100)}%");
		imageExp.fillAmount = ratio;

		if(targetLevel == 10 && currDevice.SubOption3ID == 0)
		{
			var option = devicePanel.subOption.GetItem();
			currDevice.SubOption3ID = option;
		}


		if (main.Coefficient != 0)
		{
			textMainOptionName.SetText(main.Name);
			textMainOption.SetText($"{main.Coefficient + main.Increase * (currDevice.CurrLevel - 1)}");
			afterMainOption.SetText($"{main.Coefficient + main.Increase * (targetLevel - 1)}");
		}
		else if(main.Value != 0)
		{
			textMainOptionName.SetText(main.Name);
			textMainOption.SetText($"{main.Value + main.Increase * (currDevice.CurrLevel - 1)}");
			afterMainOption.SetText($"{main.Value + main.Increase * (targetLevel - 1)}");
		}

		if(sub1.Coefficient != 0)
		{
			textSubOption1Name.SetText(sub1.Name);
			textSubOption1.SetText($"{sub1.Coefficient + sub1.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption1.SetText($"{sub1.Coefficient + sub1.Increase * (targetLevel - 1)}");
		}
		else if(sub1.Value != 0)
		{
			textSubOption1Name.SetText(sub1.Name);
			textSubOption1.SetText($"{sub1.Value + sub1.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption1.SetText($"{sub1.Value + sub1.Increase * (targetLevel - 1)}");
		}

		if(sub2.Coefficient != 0)
		{
			textSubOption2Name.SetText(sub2.Name);
			textSubOption2.SetText($"{sub2.Coefficient + sub2.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption2.SetText($"{sub2.Coefficient + sub2.Increase * (targetLevel - 1)}");
		}
		else if(sub2.Value != 0)
		{
			textSubOption2Name.SetText(sub2.Name);
			textSubOption2.SetText($"{sub2.Value + sub2.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption2.SetText($"{sub2.Value + sub2.Increase * (targetLevel - 1)}");
		}

		if(sub3 != null)
		{
			if (sub3.Coefficient != 0)
			{
				textSubOption3Name.SetText(sub3.Name);
				textSubOption3.SetText($"{sub3.Coefficient + sub3.Increase * (currDevice.CurrLevel - 1)}");
				afterSubOption3.SetText($"{sub3.Coefficient + sub3.Increase * (targetLevel - 1)}");
			}
			else if (sub3.Value != 0)
			{
				textSubOption3Name.SetText(sub3.Name);
				textSubOption3.SetText($"{sub3.Value + sub3.Increase * (currDevice.CurrLevel - 1)}");
				afterSubOption3.SetText($"{sub3.Value + sub3.Increase * (targetLevel - 1)}");
			}
		}
		else
		{
			textSubOption3Name.SetText("미해금");
			textSubOption3.SetText("미해금");
			afterSubOption3.SetText("미해금");
		}

		if (targetLevel >= 10)
		{
			isFull = true;
		}

		devicePanel.UpdateDeviceAfterUpgrade();

		GameManager.Instance.SaveExecution();
	}

	public void UpdateTargetLevel()
	{
		int totalExp = 0;
		int remainExp = 0;
		totalExp += currDevice.CurrExp;

		foreach (var card in itemCard)
		{
			int value = 0;
			if (card.item != null)
			{
				value = card.item.Value;
			}
			totalExp += (card.selectedQuantity * value);
		}

		var targetLevel = CalculateExp(totalExp, out remainExp);

		var table = DataTableMgr.GetTable<DeviceValueTable>();

		var main = table.GetDeviceValueData(currDevice.MainOptionID);
		var sub1 = table.GetDeviceValueData(currDevice.SubOption1ID);
		var sub2 = table.GetDeviceValueData(currDevice.SubOption2ID);
		var sub3 = table.GetDeviceValueData(currDevice.SubOption3ID);

		textLevel.SetText($"{targetLevel}");

		var ratio = (float)remainExp / expData[targetLevel - 1].RequireExp;
		if(ratio > 1)
			ratio = 1;

		textExp.SetText($"{(int)(ratio * 100)}%");
		imageExp.fillAmount = ratio;

		if (main.Coefficient != 0)
		{
			textMainOptionName.SetText(main.Name);
			textMainOption.SetText($"{main.Coefficient + main.Increase * (currDevice.CurrLevel - 1)}");
			afterMainOption.SetText($"{main.Coefficient + main.Increase * (targetLevel - 1)}");
		}
		else if (main.Value != 0)
		{
			textMainOptionName.SetText(main.Name);
			textMainOption.SetText($"{main.Value + main.Increase * (currDevice.CurrLevel - 1)}");
			afterMainOption.SetText($"{main.Value + main.Increase * (targetLevel - 1)}");
		}

		if (sub1.Coefficient != 0)
		{
			textSubOption1Name.SetText(sub1.Name);
			textSubOption1.SetText($"{sub1.Coefficient + sub1.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption1.SetText($"{sub1.Coefficient + sub1.Increase * (targetLevel - 1)}");
		}
		else if (sub1.Value != 0)
		{
			textSubOption1Name.SetText(sub1.Name);
			textSubOption1.SetText($"{sub1.Value + sub1.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption1.SetText($"{sub1.Value + sub1.Increase * (targetLevel - 1)}");
		}

		if (sub2.Coefficient != 0)
		{
			textSubOption2Name.SetText(sub2.Name);
			textSubOption2.SetText($"{sub2.Coefficient + sub2.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption2.SetText($"{sub2.Coefficient + sub2.Increase * (targetLevel - 1)}");
		}
		else if (sub2.Value != 0)
		{
			textSubOption2Name.SetText(sub2.Name);
			textSubOption2.SetText($"{sub2.Value + sub2.Increase * (currDevice.CurrLevel - 1)}");
			afterSubOption2.SetText($"{sub2.Value + sub2.Increase * (targetLevel - 1)}");
		}

		if (sub3 != null)
		{
			if (sub3.Coefficient != 0)
			{
				textSubOption3Name.SetText(sub3.Name);
				textSubOption3.SetText($"{sub3.Coefficient + sub3.Increase * (currDevice.CurrLevel - 1)}");
				afterSubOption3.SetText($"{sub3.Coefficient + sub3.Increase * (targetLevel - 1)}");
			}
			else if (sub3.Value != 0)
			{
				textSubOption3Name.SetText(sub3.Name);
				textSubOption3.SetText($"{sub3.Value + sub3.Increase * (currDevice.CurrLevel - 1)}");
				afterSubOption3.SetText($"{sub3.Value + sub3.Increase * (targetLevel - 1)}");
			}
		}
		else
		{
			textSubOption3Name.SetText("미해금");
			textSubOption3.SetText("미해금");
			afterSubOption3.SetText("미해금");
		}

		if(targetLevel >= 10)
		{
			isFull = true;
		}
	}

	public bool CheckFull()
	{
		return isFull;
	}
}
