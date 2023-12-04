using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static PlayerController;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    public StageManager stageManager;
    public GameObject characterPrefab;
    private GameObject characterGo;
    private PlayerController playerController;

    public ArrangeJoystick arrangeJoystick;
    private UnityEvent SetJoystick;
    private CharacterInfoUIManager characterInfoUIManager;

    private bool created;
    private bool once;

    private void Awake()
    {
        characterInfoUIManager = GameObject.FindGameObjectWithTag("CharacterInfoUIManager").GetComponent<CharacterInfoUIManager>();
        stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
        var characterStat = characterPrefab.GetComponent<CharacterState>();
        var cost = characterStat.arrangeCost;

        //SetJoystick = new UnityEvent();
        //SetJoystick.AddListener(() =>
        //{
        //    arrangeJoystick.settingMode = false;
        //    arrangeJoystick.transform.gameObject.SetActive(true);
        //    arrangeJoystick.SetPositionToCurrentPlayer(playerController.transform);
        //    once = true;
        //});
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (playerController != null)
        {
            if (!playerController.stateManager.created)
            {
                created = false;
            }
        }

        if (created && !once && playerController.stateManager.firstArranged)
        {
            //SetJoystick.Invoke();
        }
    }

    public void CreateCharacter()
    {
        var characterName = characterPrefab.GetComponent<CharacterState>().name;
        characterGo = ObjectPoolManager.instance.GetGo(characterName);
        playerController = characterGo.GetComponent<PlayerController>();
        created = true;
        playerController.stateManager.created = true;
        playerController.joystick = arrangeJoystick.transform.gameObject;
        playerController.icon = this;

        var dieEvent = characterGo.GetComponent<CanDie>();
        dieEvent.action.AddListener(() =>
        {
            playerController.currentTile.arrangePossible = true;
            playerController.icon.gameObject.SetActive(true);
        });

        stageManager.currentPlayer = playerController;
        stageManager.currentPlayerIcon = playerController.icon;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //var isCurrentPlayerNull = stageManager.currentPlayer == null;
        var isCurrentPlayerThis = stageManager.currentPlayer == playerController;
        var isPossibleMode = (characterInfoUIManager.windowMode == Defines.CharacterInfoMode.None) || (characterInfoUIManager.windowMode == Defines.CharacterInfoMode.FirstArrange);

        if (isPossibleMode || (isCurrentPlayerThis && isPossibleMode))
        {
            CreateCharacter();
            characterGo.transform.position = transform.position;
            once = false;
            characterInfoUIManager.currentPlayerChanged = true;
        }
    }
}
