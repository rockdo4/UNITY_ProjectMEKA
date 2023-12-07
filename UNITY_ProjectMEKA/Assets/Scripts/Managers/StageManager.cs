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
    public int maxCost = 20;

    private void Awake()
    {
        currentCost = maxCost;
    }
}
