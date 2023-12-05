using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[HideInInspector]
    private CharacterInfoUIManager characterInfoUIManager;
    public PlayerController currentPlayer;
    public CharacterIcon currentPlayerIcon;

    private void Awake()
    {
        characterInfoUIManager = GameObject.FindGameObjectWithTag("CharacterInfoUIManager").GetComponent<CharacterInfoUIManager>();
    }
}
