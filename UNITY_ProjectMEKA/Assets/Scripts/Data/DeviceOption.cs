public class Device
{
	public int InstanceID;
	public string Name;
	public string Description;
	public int CurrLevel;
	public int CurrExp;
	public int MaxLevel;
	public int PartType;
	public int MainOptionID;
	public int SubOption1ID;
	public int SubOption2ID;
	public int SubOption3ID = 0;
	public bool IsEquipped = false;
	public int TargetCharacterID = 0;
}

public class DeviceOption
{
	public int ID { get; set; }
	public int StatType { get; set; }
	public int PartType { get; set; }
	public string Name { get; set; }
	public int Weight { get; set; }
}

public class DeviceValue
{
	public int ID { get; set; }
	public string Name { get; set; }
	public float Coefficient { get; set; }
	public float Value { get; set; }
	public float Increase { get; set; }
	public float MaxLevel { get; set; }
}

public class DeviceExpData
{
	public int DeviceLevel { get; set; }
	public int RequireExp { get; set; }
	public int AccumulateExp { get; set; }
}