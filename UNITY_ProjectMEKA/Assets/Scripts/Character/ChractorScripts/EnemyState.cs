using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class EnemyState : CharacterState
{
    
    [SerializeField, Header("적 패시브 설정")]//e
    public List<Passive> passive;

    [SerializeField, Header("적 타입 설정")]//e
    public Defines.EnemyType enemyType;

   
    // 11.22, 김민지, 적 이동시 필요
    [SerializeField, Header("이동속력 설정,적 이동시 필요")]//e
    public float speed;

    [SerializeField, Header("적 공중형, 지상형 선택(언체크 지상형)")]//e
    public bool isFly;

    [SerializeField, Header("무빙샷?")]
    public bool isMovingShot;
    
    public bool isBlock = true;

}
