using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[HideInInspector]
    public CharacterInfoUIManager characterInfoUIManager;
    public CharacterIconManager characterIconManager;
    public ArrangeJoystick arrangeJoystick;

    public PlayerController currentPlayer;
    public CharacterIcon currentPlayerIcon;

    public float currentCost;
    public int maxCost;
    public int allMonsterCount;
    public int leftMonsterCount;
    public int leftWaveCount;
    public int currentHouseLife;
    public int maxHouseLife;

    private void Awake()
    {
        currentCost = maxCost;
        currentHouseLife = maxHouseLife;
    }

    private void Update()
    {
        if(currentHouseLife <= 0)
        {
            Debug.Log("게임오버");
        }
    }
}
