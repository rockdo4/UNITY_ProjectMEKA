using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterArrangeTest : MonoBehaviour, IPointerDownHandler
{
    public GameObject characterPrefab;
    private GameObject characterGo;
    private string characterName;
    private bool created;
    private bool arranged;

    private void Awake()
    {
        characterName = characterPrefab.GetComponent<CharacterState>().occupation.ToString();
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
            // up이 됐을 때 배치가능 타일 위가 아니면 그대로 사라진다.
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
