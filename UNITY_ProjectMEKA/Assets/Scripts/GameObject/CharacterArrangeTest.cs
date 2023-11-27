using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CharacterArrangeTest : MonoBehaviour, IPointerDownHandler
{
    public GameObject characterPrefab;
    private GameObject characterGo;
    private List<GameObject> tiles;
    private string characterName;
    private bool created;
    private bool arranged;
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
        if (Input.GetMouseButton(0) && created)
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
        else if(Input.GetMouseButtonUp(0) && !arranged)
        {
            if (characterGo != null && hit.transform != null && hit.transform.GetComponent<Tile>().arrangePossible)
            {
                Debug.Log("배치가능");
                hit.transform.GetComponentInChildren<Tile>().arrangePossible = false;
                arranged = true;
            }
            else
            {
                Debug.Log("배치불가능");
                characterGo.GetComponent<PlayerController>().ReleaseObject();
                created = false;
                foreach (var tile in tiles)
                {
                    tile.GetComponentInChildren<Tile>().SetPlacementPossible(created);
                }
            }
        }
    }

    public void CreateCharacter()
    {
        characterGo = ObjectPoolManager.instance.GetGo(characterName);
        created = true;
        foreach(var tile in tiles)
        {
            tile.GetComponentInChildren<Tile>().SetPlacementPossible(created);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!created)
        {
            CreateCharacter();
            characterGo.transform.position = transform.position;
        }
    }
}
