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

public class SkillInfo
{
	public int SkillID { get; set; }
	public int SkillLevelID { get; set; }
	public int SkillLevel { get; set; }
	public int SkillMaxLevel { get; set; }
	public string ImagePath { get; set; }
	public int MaxSigma { get; set; }
	public int UseSigma { get; set; }
	public int CoolTime { get; set; }
	public int Duration { get; set; }
	public float SkillDamageCoefficient { get; set; }
	public float SkillValueIncrease { get; set; }
	public float ArrangeCostSec { get; set; }
	public float ArrangeCostRecovery { get; set; }
}