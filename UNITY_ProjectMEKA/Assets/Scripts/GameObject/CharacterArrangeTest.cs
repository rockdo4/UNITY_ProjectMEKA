using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterArrangeTest : MonoBehaviour, IPointerDownHandler
{
    public GameObject characterPrefab;
    private GameObject characterGo;
    private GameObject[] tiles;
    private string characterName;
    private bool created;
    private bool arranged;

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
        var tileParent = GameObject.FindGameObjectWithTag("LowTile");
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
            RaycastHit hit;

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
                characterGo.GetComponent<PlayerController>().ReleaseObject();
                created = false;
            }
        }
    }

    public void CreateCharacter()
    {
        characterGo = ObjectPoolManager.instance.GetGo(characterName);
        created = true;
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
