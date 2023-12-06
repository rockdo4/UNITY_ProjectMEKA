using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[HideInInspector]
    public CharacterInfoUIManager characterInfoUIManager;
    public CharacterIconManager characterIconManager;
    public ArrangeJoystick arrangeJoystick;

    public PlayerController currentPlayer;
    public CharacterIcon currentPlayerIcon;
}
