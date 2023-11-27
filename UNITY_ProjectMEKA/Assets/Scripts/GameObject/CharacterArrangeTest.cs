using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static PlayerController;

public class CharacterArrangeTest : MonoBehaviour, IPointerDownHandler
{
    public GameObject characterPrefab;
    private GameObject characterGo;
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
                Debug.Log("��ġ����");
                hit.transform.GetComponentInChildren<Tile>().arrangePossible = false;
                firstArranged = true;
                characterGo.GetComponent<PlayerController>().SetState(CharacterStates.Arrange);
            }
            else
            {
                Debug.Log("��ġ�Ұ���");
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
        created = true;
        foreach(var tile in tiles)
        {
            tile.GetComponentInChildren<Tile>().SetPlacementPossible(created);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!created /*&& eventData.pointerEnter == gameObject*/)
        {
            CreateCharacter();
            characterGo.transform.position = transform.position;
        }
    }
}
