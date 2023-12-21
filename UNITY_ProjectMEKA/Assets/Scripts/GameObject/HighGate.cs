using UnityEngine;
using static Defines;

public class HighGate : GateController
{
    protected override void Awake()
    {
        base.Awake();
        spawnInitPos = new Vector3(transform.position.x, 2f, transform.position.z);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void SetEnemy(GameObject enemyGo, EnemySpawnInfo spawnInfo, WaveInfo waveInfo)
    {
        base.SetEnemy(enemyGo, spawnInfo, waveInfo);
        enemyGo.GetComponent<Rigidbody>().useGravity = false;
    }
}
