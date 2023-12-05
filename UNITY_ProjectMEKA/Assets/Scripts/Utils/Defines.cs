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
    }

    public enum Property
    {
        None,
        Prime,
        Edila,
        Grieve,
    }
    public enum Occupation
    {
        Guardian,
        Striker,
        Castor,
        Hunter,
        Supporters,
    }
    public enum EnemyType
    {
        None,
        Monster,//기본형으로 쓸생각
        LongDistance,//원거리형
        Offensive,//공격형
        OhYaBung,//오야붕||보스
        YangSehyung,//양세형
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
    public enum CharacterInfoMode
    {
        None,
        FirstArrange,
        SecondArrange,
        Setting
    }

    public enum bgmType
    {
    }

    public enum seType
    {
    }

}
