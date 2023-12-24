using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Defines
{
    public static class Tags
    {
        public const string enemy = "Enemy";
        public const string lowDoor = "LowDoor";
        public const string highTile = "HighTile";
        public const string lowTile = "LowTile";
        public const string playerCollider = "PlayerCollider";
        public const string enemyCollider = "EnemyCollider";
        public const string joystick = "Joystick";
        public const string handler = "Handler";
        public const string stageManager = "StageManager";
        public const string characterInfoUIManager = "CharacterInfoUIManager";
        public const string cancel = "Cancel";
        public const string collect = "Collect";
        public const string player = "Player";
        public const string characterIconManager = "CharacterIconManager";
        public const string house = "House";
        public const string stageUIManager = "StageUIManager";
    }

    public static class Layers
    {
        public const string player = "Player";
        public const string enemy = "Enemy";
        public const string background = "Background";
        public const string lowTile = "LowTile";
        public const string highTile = "HighTile";
        public const string arrangeTile = "ArrangeTile";
        public const string enemyCollider = "EnemyCollider";
        public const string playerCollider = "PlayerCollider";
        public const string handler = "Handler";
        public const string slider = "Slider";
        public const string house = "House";
        public const string gate = "Gate";
        public const string onTile = "OnTile";
    }

    public static class StorySceneNames
    {
        public const string stage0_1 = "Stage_0-1";
        public const string stage0_2 = "Stage_TR-1";
        public const string stage0_3 = "Stage_0-2";
        public const string stage0_4 = "Stage_0-3";
        public const string stage0_5 = "Stage_0-4";
        public const string stage0_6 = "Stage_TR-2";
        public const string stage0_7 = "Stage_0-5";
        public const string stage0_8 = "Stage_0-6";
        public const string stage0_9 = "Stage_0-7";
        public const string stage0_10 = "Stage_0-8";
        public const string stage0_11 = "Stage_0-9";
        public const string stage0_12 = "Stage_0-10";
        public const string stage0_13 = "Stage_1-1";
        public const string stage0_14 = "Stage_1-2";
        public const string stage0_15 = "Stage_1-3";
        public const string stage0_16 = "Stage_1-4";
    }
    public static class AssignmentSceneNames
    {
        public const string stage0_1 = "Bug_KimMinji";
        public const string stage0_2 = "a";
        public const string stage0_3 = "b";
    }
    public static class ChallengeSceneNames
    {
        public const string stage0_1 = "Bug_KimMinji";
        public const string stage0_2 = "a";
        public const string stage0_3 = "b";
    }

    public enum Property
    {
        None,
        Prime,
        Grieve, //���� ����
        Edila,
    }
    public enum Occupation
    {
        None, // ������� ���� �� �ż� �߰���
        Guardian,
        Striker,
        Castor,
        Hunter,
        Supporters,
    }
    public enum EnemyType
    {
        None,
        Monster,//�⺻������ ������
        LongDistance,//���Ÿ���
        Offensive,//������
        OhYaBung,//���ߺ�||����
        YangSehyung,//�缼��
    }
    public enum HouseType
    {
        House1,
        House2,
        House3,
        House4
    }
    public enum MoveType
    {
        Waypoint,
        WaypointRepeat,
        Straight,
    }
    public enum RotationDirection
    {
        Up,
        Right,
        Down,
        Left,
        Count
    }
    public enum WindowMode
    {
        None,
        FirstArrange,
        SecondArrange,
        Setting,
        Skill,
        Loose,
        Win,
    }

    public enum bgmType
    {
    }

    public enum seType
    {
    }

    public enum ProjectileType
    {
        None,
        Bullet,
        Aoe,
        PiercingShot,
        ChainAttack,
        HitScan,
        Instantaneous,
    }

    public enum Passive
    {
        None,//����
        Unstoppable,//���� �Ұ�
        Explosion,//����
        BusterCall,//���� ����
        SpeedUp,//�̼� ����
        Counterattack,//����
        Spite,//����
        Outlander,//�ƿ�����
        Tenacity,//������ ����
        Revenge,//����
        Mechanic,//�����

    }

    public enum Skills
    {
        None,
        Snapshot,
        StunningBlow,
        AmberSkill,
        IYRASkill,
        KALEASkill,
        MERIASkill,
        PALASkill,
        RYUSIENSkill,
        ISABELLASkill,
    }
    public enum SkillType
    {
        Auto, // skill button delete
        Instant, // 즉발
        SnipingSingle, // 한칸
        SnipingArea, // 범위 : +, 사각형, 1칸
    }

    public enum StageClass
    {
        None,
        Story,
        Assignment,
        Challenge
    }
    public enum StageMode
    {
        None,
        Deffense,
        Annihilation,
        Survival
    }

    public enum GameState
    {
        Playing,
        Win,
        Die,
        Pause
    }

    public enum IncrementalForm
    {
        Percentage,
        Magnification,
    }

	public enum MissionType
	{
		None,
		MonsterKillCount,
		SurviveTime,
		ClearTime,
		CostLimit,
		HouseLifeLimit,
		PlayerWin
	}

	public enum MissionClear
    {
		Clear,
		Fail
	}

    public enum TileType
    {
        None,
        Obstacle
    }

    public enum Language
    {
        Kor,
        Eng
    }
}
