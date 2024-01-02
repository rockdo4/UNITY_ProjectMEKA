using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//세이브 파일에 저장할 클래스
//Class to save files
public class Character
{
	public int CharacterID;
	public int CharacterLevel;
	public int CurrentExp;
	public int CharacterGrade;
	public int SkillLevel;
	public bool IsUnlock;

	public int DeviceCoreID;
	public int DeviceEngineID;

	public CharacterAffection affection;

	public float Damage
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<CharacterLevelTable>();

			var id = CharacterID * 100 + CharacterLevel;
			var levelData = levelTable.GetLevelData(id);

			if(levelData == null )
			{
                //levelData = levelTable.GetLevelData(1340103);
                //levelData = levelTable.GetLevelData(1210103);
                //levelData = levelTable.GetLevelData(1130103);
                levelData = levelTable.GetLevelData(1350103);

            }

            var valueTable = DataTableMgr.GetTable<DeviceValueTable>();

			float value = 0;
			float coefficient = 0;

			if(DeviceCoreID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceCoreID];
				if (device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
						if(option == null)
                            continue;
                        
						if (option.ID == 81102 || option.ID == 82102 || option.ID == 81203)
						{
							coefficient += option.Coefficient + option.Increase * (device.CurrLevel - 1);
						}
						else if (option.ID == 81204)
						{
							value += option.Value + option.Increase * (device.CurrLevel - 1);
						}
					}
				}
			}

			if(DeviceEngineID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceEngineID];
				if(device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
                        if (option == null)
                            continue;

                        if (option.ID == 81102 || option.ID == 82102 || option.ID == 81203)
						{
							coefficient += option.Coefficient;
						}
						else if (option.ID == 81204)
						{
							value += option.Value;
						}
					}
				}
			}

			return (levelData.CharacterDamage + value) * (1 + (coefficient / 100));
		}
	}
	public float Armor
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<CharacterLevelTable>();

			var id = CharacterID * 100 + CharacterLevel;
			var levelData = levelTable.GetLevelData(id);

            if (levelData == null)
            {
                //levelData = levelTable.GetLevelData(1340103);
                //levelData = levelTable.GetLevelData(1210103);
                //levelData = levelTable.GetLevelData(1130103);
                levelData = levelTable.GetLevelData(1350103);
            }

            var valueTable = DataTableMgr.GetTable<DeviceValueTable>();

			float value = 0;
			float coefficient = 0;

			if (DeviceCoreID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceCoreID];
				if(device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
                        if (option == null)
                            continue;

                        if (option.ID == 81103 || option.ID == 82103 || option.ID == 81205)
						{
							coefficient += option.Coefficient;
						}
						else if (option.ID == 81206)
						{
							value += option.Value;
						}
					}
				}
			}

			if (DeviceEngineID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceEngineID];
				if(device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
                        if (option == null)
                            continue;

                        if (option.ID == 81103 || option.ID == 82103 || option.ID == 81205)
						{
							coefficient += option.Coefficient;
						}
						else if (option.ID == 81206)
						{
							value += option.Value;
						}
					}
				}
			}
			return (levelData.CharacterArmor + value) * (1 + (coefficient / 100));
		}
	}
	public float HP
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<CharacterLevelTable>();

			var id = CharacterID * 100 + CharacterLevel;
			var levelData = levelTable.GetLevelData(id);

            if (levelData == null)
            {
                //levelData = levelTable.GetLevelData(1340103);
                //levelData = levelTable.GetLevelData(1210103);
                //levelData = levelTable.GetLevelData(1130103);
                levelData = levelTable.GetLevelData(1350103);
            }

            var valueTable = DataTableMgr.GetTable<DeviceValueTable>();

			float value = 0;
			float coefficient = 0;

			if (DeviceCoreID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceCoreID];
				if(device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
                        if (option == null)
                            continue;

                        if (option.ID == 81101 || option.ID == 82101 || option.ID == 81201)
						{
							coefficient += option.Coefficient;
						}
						else if (option.ID == 81202)
						{
							value += option.Value;
						}
					}
				}
			}

			if (DeviceEngineID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceEngineID];
				if(device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
                        if (option == null)
                            continue;

                        if (option.ID == 81101 || option.ID == 82101 || option.ID == 81201)
						{
							coefficient += option.Coefficient;
						}
						else if (option.ID == 81202)
						{
							value += option.Value;
						}
					}
				}
			}

			return (levelData.CharacterHP + value) * (1 + (coefficient / 100));
		}
	}

	public float ArrangementCost
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();

			var charData = charTable.GetCharacterData(CharacterID);

			var valueTable = DataTableMgr.GetTable<DeviceValueTable>();

			float value = 0;
			float coefficient = 0;

			if (DeviceCoreID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceCoreID];
				if(device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
                        if (option == null)
                            continue;

                        if (option.ID == 82107)
						{
							value += option.Value;
						}
					}
				}
			}

			if (DeviceEngineID != 0)
			{
				var device = DeviceInventoryManager.Instance.m_DeviceStorage[DeviceEngineID];
				if(device != null)
				{
					int[] ids =
					{
						device.MainOptionID,
						device.SubOption1ID,
						device.SubOption2ID,
						device.SubOption3ID
					};

					for (int i = 0; i < ids.Length; i++)
					{
						var option = valueTable.GetDeviceValueData(ids[i]);
                        if (option == null)
                            continue;

                        if (option.ID == 82107)
						{
							value += option.Value;
						}
					}
				}
			}

			return charData.ArrangementCost + value;
		}
	}



	public float WithdrawCost
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();
			var charData = charTable.GetCharacterData(CharacterID);
			return charData.WithdrawCost;
		}
	}

	public float ReArrangementCoolDown
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();
			var charData = charTable.GetCharacterData(CharacterID);
			return charData.ReArrangementCoolDown;
		}
	}

	public string Name
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();
			var charData = charTable.GetCharacterData(CharacterID);
			var stringTable = StageDataManager.Instance.stringTable;
			return stringTable.GetString(charData.CharacterNameStringID);
		}
	}

	public string ImagePath
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();
			var charData = charTable.GetCharacterData(CharacterID);
			return charData.ImagePath;
		}
	}

	public string CharacterStanding
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();
			var charData = charTable.GetCharacterData(CharacterID);
			return charData.CharacterStanding;
		}
	}

	public string CharacterHead
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();
			var charData = charTable.GetCharacterData(CharacterID);
			return charData.CharacterHead;
		}
	}

	public int SkillID
	{
		get
		{
			var charTable = DataTableMgr.GetTable<CharacterTable>();
			var charData = charTable.GetCharacterData(CharacterID);
			return charData.SkillID;
		}
	}

	public string ATKSpd
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<CharacterLevelTable>();

			var id = CharacterID * 100 + CharacterLevel;
			var levelData = levelTable.GetLevelData(id);
			return levelData.CharacterATKSpd;
		}
	}

	public int CriChance
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<CharacterLevelTable>();

			var id = CharacterID * 100 + CharacterLevel;
			var levelData = levelTable.GetLevelData(id);
			return levelData.CharacterCriChance;
		}
	}

	public int CriDMG
	{
		get
		{
			var levelTable = DataTableMgr.GetTable<CharacterLevelTable>();

			var id = CharacterID * 100 + CharacterLevel;
			var levelData = levelTable.GetLevelData(id);
			return levelData.CharacterCriDMG;
		}
	}
}

//캐릭터 정보
//Character information
public class CharacterData
{
	public int CharacterID { get; set; }
	public string CharacterNameStringID { get; set; }	
	public int InitialGrade { get; set; }
	public int CharacterProperty { get; set; }
	public int CharacterOccupation { get; set; }
	public int ArrangementCost { get; set; }
	public int WithdrawCost { get; set; }
	public int ReArrangementCoolDown { get; set; }
	public string ImagePath { get; set; }
	public string CharacterStanding { get; set; }
	public string CharacterHead { get; set; }
	public string PortraitPath { get; set; }
	public string MechaImagePath { get; set; }
	public string AttackRangeImagePath { get; set; }
	public string SkillRangeImagePath { get; set; }

    public int SkillID { get; set; }
	public int MaxSigma { get; set; }
	public string OccupationInfoStringID { get; set; }
	public string OccupationStringID { get; set; }
	public int IsBasic { get; set; }
}

//캐릭터ID + 레벨을 ID로 사용해서 레벨에 따른 스탯 불러옴
//Use CharacterID + Level as ID to load stats according to level
public class LevelData
{
	public int CharacterLevelID { get; set; }
	public int CharacterGrade { get; set; }
	public int CharacterLevel { get; set; }
	public float CharacterDamage { get; set; }
	public float CharacterArmor { get; set; }
	public float CharacterHP { get; set; }
	public string CharacterATKSpd { get; set; }
	public int CharacterCriChance { get; set; }
	public int CharacterCriDMG { get; set; }
}

//레벨업하기 위한 경험치 정보
//Experience information for level up
public class ExpData
{
	public int CharacterLevel { get; set; }
	public int CharacterGrade { get; set; }
	public int RequireExp { get; set; }
	public int AccumulateExp { get; set; }
}

public class CharacterAffection
{
	public int AffectionLevel { get; set; }
	public int AffectionPoint { get; set; }
	public DateTime LastTime { get; set; }
}

public class AffectionData
{
	public int Level { get; set; }
	public int AffectionPoint { get; set; }
}

public class CommunicationData
{
	public int CharacterID { get; set; }
	public int ID { get; set; }
	public int ScriptID { get; set; }
	public string Script { get; set; }
	public int Value { get; set; }
	public int Branch { get; set; }
}