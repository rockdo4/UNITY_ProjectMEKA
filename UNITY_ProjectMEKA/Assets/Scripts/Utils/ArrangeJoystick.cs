using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PlayerController;

public class ArrangeJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Count
    }

    private List<GameObject> directions = new List<GameObject>();
    private Bounds backgroundBounds;
    private List<Bounds> bounds;
    private GameObject currentTile;
    private GameObject prevTile;
    LinkedList<Tile> tempTiles = new LinkedList<Tile>();
    private BoxCollider boxCollider;
    private float half;
    [HideInInspector]
    public float yOffset = 1f;

    public bool secondArranged;
    private PlayerController player;
    private CharacterArrangement playerIcon;
    public float radius;

    public Button cancelButton;

    private void OnEnable()
    {
        secondArranged = false;
        boxCollider = GetComponent<BoxCollider>();
        half = boxCollider.bounds.size.x / 2f;
        backgroundBounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 0f));

        var parent = transform.parent;
        for (int i = 0; i < (int)Direction.Count; ++i)
        {
            directions.Add(parent.GetChild(i).gameObject);
        }

        bounds = new List<Bounds>
        {
            new Bounds(new Vector3(0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f))
        };
    }

    private void Start()
    {
        cancelButton.onClick.AddListener(() =>
        {
            Debug.Log("cancel init");
            if (cancelButton.gameObject.activeSelf)
            {
                cancelButton.gameObject.SetActive(false);
            }
            secondArranged = true;
            ClearTileMesh(tempTiles);
            transform.localPosition = Vector3.zero;
            transform.parent.gameObject.SetActive(false);
            player.ReturnPool.Invoke();
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!secondArranged)
        {
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!secondArranged)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float enter;
            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint;
                transform.localPosition = backgroundBounds.ClosestPoint(transform.localPosition);
            }

            prevTile = currentTile;
            for(int i = 0; i < (int)Direction.Count; ++i)
            {
                if (bounds[i].Contains(transform.localPosition))
                {
                    currentTile = directions[i];
                    if(prevTile != currentTile)
                    {
                        Debug.Log($"{prevTile}���� {currentTile}�� �ٲ�");
                        RotatePlayer(currentTile.transform, false);
                    }
                    break;
                }
            }

            if (cancelButton.gameObject.activeSelf)
            {
                cancelButton.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!secondArranged)
        {
            // �ڵ鷯�� �÷��̾� ��ǥ ���� ���� �ݰ� ���� ���� ��
            // ��ġ ��� ��ư Ȱ��ȭ
            var playerCenterPos = player.transform.position;
            playerCenterPos.y = transform.position.y;

            if(Vector3.Distance(transform.position, playerCenterPos) < radius)
            {
                Debug.Log("�÷��̾� �ݰ� ��");
                // ��ġ ��� ��ư Ȱ��ȭ
                cancelButton.gameObject.SetActive(true);

                // �ڵ鷯 ���� ������ 0,0 ����
                transform.localPosition = Vector3.zero;
            }
            else
            {
                RotatePlayer(currentTile.transform, true);
                secondArranged = true;
                player.SetState(CharacterStates.Idle);
                ClearTileMesh(tempTiles);
                playerIcon.gameObject.SetActive(false);
                transform.localPosition = Vector3.zero;
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void RotatePlayer(Transform currentTileParent, bool mouseUp)
    {
        var pos = currentTileParent.position;
        var go = currentTileParent.gameObject;
        if (go == directions[(int)Direction.Up])
        {
            if (mouseUp)
            {
                pos.z += -half;
            }
            else
            {
                player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                if(player != null)
                {
                    ChangeTileMesh();
                }
            }
        }
        else if (go == directions[(int)Direction.Right])
        {
            if (mouseUp)
            {
                pos.x += -half;
            }
            else
            {
                player.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                ChangeTileMesh();
            }
        }
        else if (go == directions[(int)Direction.Down])
        {
            if (mouseUp)
            {
                pos.z += half;
            }
            else
            {
                player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                ChangeTileMesh();
            }
        }
        else
        {
            if (mouseUp)
            {
                pos.x += half;
            }
            else
            {
                player.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                ChangeTileMesh();
            }
        }

        if(mouseUp)
        {
            transform.position = pos;
        }
    }

    public void ChangeTileMesh()
    {
        ClearTileMesh(tempTiles);
        var state = player.stateManager.currentBase as PlayableArrangeState;
        state.UpdateAttackPositions();
        foreach (var tilePos in player.attakableTilePositions)
        {
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer(LayerMask.LayerToName(player.stateManager.tiles[0].layer));
            // ����ĳ��Ʈ ����
            var tempPos = new Vector3(tilePos.x, tilePos.y - 10f, tilePos.z);

            if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
            {
                // ���̰� ������Ʈ�� �ε����� ���� ó��
                //Debug.Log("Hit " + hit.collider.gameObject.name);
                var tileContoller = hit.transform.GetComponent<Tile>();
                tileContoller.SetTileMaterial(true, Tile.TileMaterial.Attack);
                tempTiles.AddLast(tileContoller);
            }
            else
            {
                // ���̰� �ƹ��͵� �ε����� �ʾ��� ���� ó��
                //Debug.Log("No hit");
            }
        }
    }

    public void ClearTileMesh(LinkedList<Tile> tempTiles)
    {
        foreach(var tile in tempTiles)
        {
            tile.SetTileMaterial(false, Tile.TileMaterial.None);
        }
        tempTiles.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var tilePos in player.attakableTilePositions)
        {
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer(LayerMask.LayerToName(player.stateManager.tiles[0].layer));
            //Debug.Log(LayerMask.LayerToName(firstArranger.tiles[0].layer));
            // ����ĳ��Ʈ ����
            var tempPos = new Vector3(tilePos.x, tilePos.y - 10f, tilePos.z);

            if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
            {
                // ���̰� ������Ʈ�� �ε����� ���� ó��
                //Debug.Log("Hit " + hit.collider.gameObject.name);
                //hit.transform.GetComponent<Tile>().SetTileMaterial(true, Tile.TileMaterial.Attack);
            }
            else
            {
                // ���̰� �ƹ��͵� �ε����� �ʾ��� ���� ó��
                //Debug.Log("No hit");
            }
            Gizmos.DrawLine(tilePos, tilePos + Vector3.down * 1000); // 10�� ������ ����
        }

        var playerCenterPos = player.transform.position;
        playerCenterPos.y = transform.position.y;

        //Gizmos.DrawSphere(playerCenterPos, radius);
    }

    public void SetPlayer(Transform player)
    {
        this.player = player.GetComponent<PlayerController>();
    }

    public void SetFirstArranger(CharacterArrangement icon)
    {
        playerIcon = icon;
    }
}
