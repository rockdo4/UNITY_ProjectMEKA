using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static PlayerController;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    public GameObject characterPrefab;
    public ArrangeJoystick arrangeJoystick;

    private GameObject characterGo;
    private PlayerController playerController;
    private UnityEvent SetJoystick;

    private bool created;
    private bool once;

    private void Awake()
    {
        var characterStat = characterPrefab.GetComponent<CharacterState>();
        var occupation = characterStat.occupation;
        var cost = characterStat.arrangeCost;

        SetJoystick = new UnityEvent();

        switch (occupation)
        {
            case Defines.Occupation.Guardian:
            case Defines.Occupation.Striker:
                TileSet("LowTile");
                break;
            default:
                TileSet("HighTile");
                break;
        }
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

    public List<GameObject> TileSet(string tag)
    {
        var tileParent = GameObject.FindGameObjectWithTag(tag);
        var tileCount = tileParent.transform.childCount;
        var tiles = new List<GameObject>();
        for (int i = 0; i < tileCount; ++i)
        {
            if (tileParent.transform.GetChild(i).GetComponentInChildren<Tile>().arrangePossible)
            {
                tiles.Add(tileParent.transform.GetChild(i).gameObject);
            }
        }
        return tiles;
    }

    private void Update()
    {
        if(playerController != null)
        {
            if(!playerController.stateManager.created)
            {
                created = false;
            }
            //Debug.Log($"created: {created} / once: {once} / firstArranged: {playerController.stateManager.firstArranged}");
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

        var occupation = characterGo.GetComponent<CharacterState>().occupation;
        switch (occupation)
        {
            case Defines.Occupation.Guardian:
            case Defines.Occupation.Striker:
                playerController.stateManager.tiles = TileSet("LowTile");
                break;
            default:
                playerController.stateManager.tiles = TileSet("HighTile");
                break;
        }
        created = true;
        playerController.stateManager.created = true;
        playerController.joystick = arrangeJoystick.transform.parent.gameObject;
        playerController.icon = this;
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
