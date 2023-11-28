using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines
{
    public enum Property
    {
        None,
        Prime,
        Edila,
        Grieve,
    }
    public enum Occupation
    {
        None,
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

    public enum bgmType
    {
    }

    public enum seType
    {
    }

}
