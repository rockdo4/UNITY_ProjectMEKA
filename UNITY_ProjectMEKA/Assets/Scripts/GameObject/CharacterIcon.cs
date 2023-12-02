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
    public ArrangeJoystick arrangeJoystick;

    private GameObject characterGo;
    private PlayerController playerController;
    private UnityEvent SetJoystick;

    private bool created;
    private bool once;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
        var characterStat = characterPrefab.GetComponent<CharacterState>();
        var cost = characterStat.arrangeCost;

        SetJoystick = new UnityEvent();
    }

    private void Start()
    {
        SetJoystick.AddListener(() => 
        {
            arrangeJoystick.transform.parent.gameObject.SetActive(true);
            arrangeJoystick.SetPlayer(characterGo.transform);
            arrangeJoystick.SetFirstArranger(this);
            arrangeJoystick.SetPositionToCurrentPlayer(playerController.transform);
            once = true;
        });
    }

    private void Update()
    {
        if(playerController != null)
        {
            if(!playerController.stateManager.created)
            {
                created = false;
            }
        }

        if (created && !once && playerController.stateManager.firstArranged)
        {
            SetJoystick.Invoke();
        }
    }

    public void CreateCharacter()
    {
        var characterName = characterPrefab.GetComponent<CharacterState>().name;
        characterGo = ObjectPoolManager.instance.GetGo(characterName);
        playerController = characterGo.GetComponent<PlayerController>();
        created = true;
        playerController.stateManager.created = true;
        playerController.joystick = arrangeJoystick.transform.parent.gameObject;
        playerController.icon = this;
        stageManager.currentPlayer = playerController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!created && !arrangeJoystick.transform.gameObject.active)
        {
            CreateCharacter();
            characterGo.transform.position = transform.position;
            once = false;
        }
    }
}
