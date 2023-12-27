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

	public TextMeshProUGUI textLevel;
	public TextMeshProUGUI textMainOption;
	public TextMeshProUGUI textSubOption1;
	public TextMeshProUGUI textSubOption2;
	public TextMeshProUGUI textSubOption3;
	public TextMeshProUGUI textExp;

	public ItemQuantityDeviceCard[] itemCard;
	public Button applyButton;
	public DevicePanel devicePanel;

	private Device currDevice;
    private List<DeviceExpData> expData;
	private int currLevel;
	private StringTable stringTable;

	private void Awake()
	{
		applyButton.onClick.AddListener(() =>
		{
			ApplyUpgradeLevel();

			foreach(var card in itemCard)
			{
				card.ConsumeItem();
				card.SetText();
			}
			UpdateTargetLevel();
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

		var tier1Name = stringTable.GetString(tier1.NameStringID);
		var tier2Name = stringTable.GetString(tier2.NameStringID);
		var tier3Name = stringTable.GetString(tier3.NameStringID);

        itemCard[0].GetComponentInChildren<TextMeshProUGUI>().SetText(tier1Name);
		itemCard[1].GetComponentInChildren<TextMeshProUGUI>().SetText(tier2Name);
		itemCard[2].GetComponentInChildren<TextMeshProUGUI>().SetText(tier3Name);
	}

	private void OnEnable()
	{
		for (int i = 0; i < itemCard.Length; i++)
		{
			itemCard[i].SetItem(itemID[i]);
			itemCard[i].SetText();
		}
	}

	public void SetDeivce(Device device)
	{
		currDevice = device;
		currLevel = device.CurrLevel;

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

		textLevel.SetText($"레벨 : {currDevice.CurrLevel}	>>	{targetLevel}");

		if(main.Coefficient != 0)
		{
			textMainOption.SetText($"{main.Name} : " +
				$"{main.Coefficient + main.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{main.Coefficient + main.Increase * (targetLevel - 1)}");
		}
		else if(main.Value != 0)
		{
			textMainOption.SetText($"{main.Name} : " +
				$"{main.Value + main.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{main.Value + main.Increase * (targetLevel - 1)}");
		}

		if(sub1.Coefficient != 0)
		{
			textSubOption1.SetText($"{sub1.Name} : " +
				$"{sub1.Coefficient + sub1.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub1.Coefficient + sub1.Increase * (targetLevel - 1)}");
		}
		else if(sub1.Value != 0)
		{
			textSubOption1.SetText($"{sub1.Name} : " +
				$"{sub1.Value + sub1.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub1.Value + sub1.Increase * (targetLevel - 1)}");
		}

		if(sub2.Coefficient != 0)
		{
			textSubOption2.SetText($"{sub2.Name} : " +
				$"{sub2.Coefficient + sub2.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub2.Coefficient + sub2.Increase * (targetLevel - 1)}");
		}
		else if(sub2.Value != 0)
		{
			textSubOption2.SetText($"{sub2.Name} : " +
				$"{sub2.Value + sub2.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub2.Value + sub2.Increase * (targetLevel - 1)}");
		}

		if(sub3 != null)
		{
			if (sub3.Coefficient != 0)
			{
				textSubOption3.SetText($"{sub3.Name} : " +
					$"{sub3.Coefficient + sub3.Increase * (currDevice.CurrLevel - 1)}" +
					$"	>>	{sub3.Coefficient + sub3.Increase * (targetLevel - 1)}");
			}
			else if (sub3.Value != 0)
			{
				textSubOption3.SetText($"{sub3.Name} : " +
					$"{sub3.Value + sub3.Increase * (currDevice.CurrLevel - 1)}" +
					$"	>>	{sub3.Value + sub3.Increase * (targetLevel - 1)}");
			}
		}
		else
		{
			textSubOption3.SetText("미해금");
		}

		textExp.SetText($"경험치 : {currDevice.CurrExp} >> {remainExp}");

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

		textLevel.SetText($"레벨 : {currDevice.CurrLevel}	>>	{targetLevel}");

		if (main.Coefficient != 0)
		{
			textMainOption.SetText($"{main.Name} : " +
				$"{main.Coefficient + main.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{main.Coefficient + main.Increase * (targetLevel - 1)}");
		}
		else if (main.Value != 0)
		{
			textMainOption.SetText($"{main.Name} : " +
				$"{main.Value + main.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{main.Value + main.Increase * (targetLevel - 1)}");
		}

		if (sub1.Coefficient != 0)
		{
			textSubOption1.SetText($"{sub1.Name} : " +
				$"{sub1.Coefficient + sub1.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub1.Coefficient + sub1.Increase * (targetLevel - 1)}");
		}
		else if (sub1.Value != 0)
		{
			textSubOption1.SetText($"{sub1.Name} : " +
				$"{sub1.Value + sub1.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub1.Value + sub1.Increase * (targetLevel - 1)}");
		}

		if (sub2.Coefficient != 0)
		{
			textSubOption2.SetText($"{sub2.Name} : " +
				$"{sub2.Coefficient + sub2.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub2.Coefficient + sub2.Increase * (targetLevel - 1)}");
		}
		else if (sub2.Value != 0)
		{
			textSubOption2.SetText($"{sub2.Name} : " +
				$"{sub2.Value + sub2.Increase * (currDevice.CurrLevel - 1)}" +
				$"	>>	{sub2.Value + sub2.Increase * (targetLevel - 1)}");
		}

		if (sub3 != null)
		{
			if (sub3.Coefficient != 0)
			{
				textSubOption3.SetText($"{sub3.Name} : " +
					$"{sub3.Coefficient + sub3.Increase * (currDevice.CurrLevel - 1)}" +
					$"	>>	{sub3.Coefficient + sub3.Increase * (targetLevel - 1)}");
			}
			else if (sub3.Value != 0)
			{
				textSubOption3.SetText($"{sub3.Name} : " +
					$"{sub3.Value + sub3.Increase * (currDevice.CurrLevel - 1)}" +
					$"	>>	{sub3.Value + sub3.Increase * (targetLevel - 1)}");
			}
		}
		else
		{
			textSubOption3.SetText("미해금");
		}

		textExp.SetText($"경험치 : {currDevice.CurrExp} >> {remainExp}");
	}

}
