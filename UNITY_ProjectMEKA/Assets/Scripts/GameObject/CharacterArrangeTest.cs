using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterArrangeTest : MonoBehaviour, IPointerDownHandler
{
    public GameObject characterPrefab;
    private GameObject characterGo;
    private GameObject[] tiles;
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
        tiles = new GameObject[tileCount];
        for (int i = 0; i < tileCount; ++i)
        {
            tiles[i] = tileParent.transform.GetChild(i).gameObject;
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
                characterGo.transform.position = hit.point;
            }
        }
        else if(Input.GetMouseButtonUp(0) && !arranged)
        {
            if(characterGo != null)
            {
                Debug.Log(hit.transform.gameObject);
                if(!hit.transform.gameObject.GetComponentInChildren<Tile>().arrangePossible)
                {
                    characterGo.GetComponent<PlayerController>().ReleaseObject();
                    created = false;
                    foreach (var tile in tiles)
                    {
                        tile.GetComponentInChildren<Tile>().SetPlacementPossible(created);
                    }
                }
                else
                {
                    // 캐릭터를 해당 타일의 중앙점에 고정!
                    var pos = hit.transform.position;
                    pos.y = hit.point.y;
                    characterGo.transform.position = pos;
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
