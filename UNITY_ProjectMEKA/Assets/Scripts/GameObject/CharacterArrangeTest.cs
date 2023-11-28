using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static PlayerController;

public class CharacterArrangeTest : MonoBehaviour, IPointerDownHandler
{
    public GameObject characterPrefab;
    public ArrangeJoystick arrangeJoystick;
    private GameObject characterGo;
    private PlayerController playerController;
    private List<GameObject> tiles;
    private string characterName;
    private bool created;
    private bool firstArranged;
    private RaycastHit hit;

    private void Awake()
    {
        characterName = characterPrefab.GetComponent<CharacterState>().occupation.ToString();
        var occupation = characterPrefab.GetComponent<CharacterState>().occupation;

        switch(occupation)
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

    public void TileSet(string tag)
    {
        var tileParent = GameObject.FindGameObjectWithTag(tag);
        var tileCount = tileParent.transform.childCount;
        tiles = new List<GameObject>();
        for (int i = 0; i < tileCount; ++i)
        {
            tiles.Add(tileParent.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && created && !firstArranged)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = 1 << LayerMask.NameToLayer("Background");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                var pos = hit.point;
                if(hit.transform.GetComponentInChildren<Tile>().arrangePossible)
                {
                    pos = hit.transform.position;
                    pos.y = hit.transform.GetComponentInChildren<Tile>().height;
                }
                characterGo.transform.position = pos;
            }
        }
        else if(Input.GetMouseButtonUp(0) && !firstArranged && created)
        {
            if (characterGo != null && hit.transform != null && hit.transform.GetComponent<Tile>().arrangePossible)
            {
                Debug.Log("배치가능");
                hit.transform.GetComponentInChildren<Tile>().arrangePossible = false;
                firstArranged = true;
                arrangeJoystick.SetPlayer(characterGo.transform);
                arrangeJoystick.transform.parent.gameObject.SetActive(firstArranged);
                var joystickPos = characterGo.transform.position;
                joystickPos.y += arrangeJoystick.yOffset;
                arrangeJoystick.transform.parent.position = joystickPos;
                
            }
            else
            {
                Debug.Log($"배치불가능: {hit}");
                characterGo.GetComponent<PlayerController>().ReleaseObject();
                created = false;
            }

            foreach (var tile in tiles)
            {
                tile.GetComponentInChildren<Tile>().SetPlacementPossible(false);
            }
        }
    }

    public void CreateCharacter()
    {
        Debug.Log("created");
        characterGo = ObjectPoolManager.instance.GetGo(characterName);
        playerController = characterGo.GetComponent<PlayerController>();
        //playerController.SetState(CharacterStates.Arrange);
        Time.timeScale = 0.2f;
        created = true;
        foreach(var tile in tiles)
        {
            tile.GetComponentInChildren<Tile>().SetPlacementPossible(created);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!created && !arrangeJoystick.gameObject.active)
        {
            CreateCharacter();
            arrangeJoystick.gameObject.SetActive(true);
            arrangeJoystick.SetFirstArranger(gameObject);
            characterGo.transform.position = transform.position;
        }
    }
}
