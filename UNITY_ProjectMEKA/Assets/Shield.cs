using UnityEngine;

public class Shield : MonoBehaviour
{
    public PlayerController player;
    

    // Update is called once per frame
    void Update()
    {
        if(player.state.shield >= 0f)
        {
            GetComponent<PoolAble>().ReleaseObject();
        }
    }
}
