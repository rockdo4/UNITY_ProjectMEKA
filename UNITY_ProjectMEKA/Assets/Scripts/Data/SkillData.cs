using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
	public int SkillID { get; set; }
	public int SkillLevelID { get; set; }
	public int SkillLevel { get; set; }
	public int SkillMaxLevel { get; set; }
}

public class SkillUpgradeData
{
	public int Level { get; set; }
	public int Tier1ID { get; set; }
	public int RequireTier1 { get; set; }
	public int Tier2ID { get; set; }
	public int RequireTier2 { get; set; }
	public int Tier3ID { get; set; }
	public int RequireTier3 { get; set; }
}