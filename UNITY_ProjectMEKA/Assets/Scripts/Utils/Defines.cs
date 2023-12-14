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
    }

    public static class StorySceneNames
    {
        public const string stage0_1 = "Bug_KimMinji";
        public const string stage0_2 = "a";
        public const string stage0_3 = "b";


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
    public enum GateType
    {
        LowGate1,
        LowGate2,
        LowGate3,
        LowGate4,
        HighGate1,
        HighGate2,
        HighGate3,
        HighGate4,
    }
    public enum MoveType
    {
        AutoTile,
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
        Win,
        Loose,
        Skill
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
    {//� ��ų���� ǥ��: �ڵ�, ���, �������� ����, �������� ���� 
        Auto,
        Instant,
        SnipingSingle,
        SnipingArea,
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

}
