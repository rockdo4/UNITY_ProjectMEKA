using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUsePanel: MonoBehaviour
{
    public Button image;
    public TextMeshProUGUI selectCount;
    public TextMeshProUGUI quantityCount;
    public Button useButton;

    public ItemCardManager itemPanel;

    private Item item;
    private int count;

    private GachaSystem<int> coreOption;
    private GachaSystem<int> engineOption;
    public GachaSystem<int> subOption;

    private DeviceOptionTable deviceOptionTable;
    private DeviceValueTable deviceValueTable;
    private Dictionary<int, Device> deviceDict;

    private void OnEnable()
    {
        count = 0;
    }
    private void Start()
    {
        SetDevice();

        image.onClick.AddListener(() =>
        {
            count++;
            if (count > item.Count)
                count--;

            selectCount.SetText($"{count}");
        });

        useButton.onClick.AddListener(() =>
        {

            for (int i = 0; i < count; i++)
            {
                CreateDevice(Random.Range(1, 3));
                GameManager.Instance.SaveExecution();
            }
            item.Count -= count;
            count = 0;
            
            itemPanel.UpdateItemCard();
            gameObject.SetActive(false);
        });
    }

    public void SetDevice()
    {
        coreOption = new GachaSystem<int>();
        engineOption = new GachaSystem<int>();
        subOption = new GachaSystem<int>();

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
    }

    public void CreateDevice(int PartType)
    {
        if (PartType < 1) PartType = 1;
        if (PartType > 2) PartType = 2;

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

        if (PartType == 1)
        {
            device.Name = "디바이스 코어";
            device.Description = "디바이스 메카의 코어에 사용하는 부품이다.";
        }
        else if (PartType == 2)
        {
            device.Name = "디바이스 엔진";
            device.Description = "디바이스 메카의 엔진에 사용하는 부품이다.";
        }

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

    public void SetItem(Item item)
    {
        this.item = item;
        selectCount.SetText($"{count}");
        quantityCount.SetText($"{item.Count}");

        var info = DataTableMgr.GetTable<ItemInfoTable>().GetItemData(item.ID);
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>(info.ImagePath);
    }
}
