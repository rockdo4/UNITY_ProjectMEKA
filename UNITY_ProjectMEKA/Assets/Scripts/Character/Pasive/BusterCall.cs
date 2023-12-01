using UnityEngine;

public class BusterCall : MonoBehaviour
{
    private float timer = 3f;
    EnemyController enemy;
    GameObject[] Enemys;
    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 3f;
            Enemys = GameObject.FindGameObjectsWithTag("Enemy");

            Vector3Int playerGridPos = enemy.CurrentGridPos;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        Vector3Int checkPos = new Vector3Int(playerGridPos.x + x, playerGridPos.y + y, playerGridPos.z + z);

                        foreach (var player in Enemys)
                        {
                            EnemyController pc = player.GetComponent<EnemyController>();
                            if (pc != null && pc.CurrentGridPos == checkPos)
                            {
                                enemy.healingTarget = player;
                                enemy.Healing(0.3f);

                            }
                        }
                    }
                }
            }
        }
    }
}
