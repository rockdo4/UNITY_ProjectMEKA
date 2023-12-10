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

    private void Awake()
    {
        currentCost = maxCost;
    }
}
