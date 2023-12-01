using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
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
    private CharacterIcon playerIcon;
    public float radius;

    public Button cancelButton;
    public Button collectButton;

    public UnityEvent ArrangeDone;

    private void OnEnable()
    {
        currentTile = null;
        ArrangeDone = new UnityEvent();
        ArrangeDone.AddListener(() =>
        {
            Debug.Log("arrange done");
            secondArranged = true;
            player.currentTile.arrangePossible = false;
            player.SetState(CharacterStates.Idle);
            ClearTileMesh(tempTiles);
            playerIcon.gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
            transform.parent.gameObject.SetActive(false);
        });


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
            secondArranged = false;
            ClearTileMesh(tempTiles);
            transform.localPosition = Vector3.zero;
            transform.parent.gameObject.SetActive(false);
            player.currentTile.arrangePossible = true;
            player.ReturnPool.Invoke();
			Time.timeScale = 1f;
		});

        collectButton.onClick.AddListener(() =>
        {
            Debug.Log("collect init");
            if (collectButton.gameObject.activeSelf)
            {
                collectButton.gameObject.SetActive(false);
            }
            secondArranged = false;
            ClearTileMesh(tempTiles);
            transform.localPosition = Vector3.zero;
            transform.gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            player.currentTile.arrangePossible = true;
            player.ReturnPool.Invoke();
            playerIcon.gameObject.SetActive(true);
            Time.timeScale = 1f;
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
                        //Debug.Log($"{prevTile}에서 {currentTile}로 바뀜");
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
            // 핸들러가 플레이어 좌표 기준 일정 반경 내에 있을 때
            // 배치 취소 버튼 활성화
            var playerCenterPos = player.transform.position;
            playerCenterPos.y = transform.position.y;

            if(Vector3.Distance(transform.position, playerCenterPos) < radius)
            {
                Debug.Log("플레이어 반경 내");
                // 배치 취소 버튼 활성화
                cancelButton.gameObject.SetActive(true);

                // 핸들러 로컬 포지션 0,0 고정
                transform.localPosition = Vector3.zero;
            }
            else
            {
                RotatePlayer(currentTile.transform, true);
                secondArranged = true;
                ArrangeDone.Invoke();
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
        Debug.Log("ChangeTileMesh");
        ClearTileMesh(tempTiles);
        var state = player.stateManager.currentBase as PlayableArrangeState;
        state.UpdateAttackPositions();
        foreach (var tilePos in player.attakableTilePositions)
        {
            RaycastHit hit;

            //공중형이면 레이어 마스크 : 공중 + 지상
            //지상형이면 레이어 마스크 : 지상
            int layerMask = 0;
            int lowTileMask = 1 << LayerMask.NameToLayer("LowTile");
            int highTileMask = 1 << LayerMask.NameToLayer("HighTile");

            switch((int)player.transform.GetComponent<CharacterState>().occupation)
            {
                case (int)Defines.Occupation.Hunter:
                case (int)Defines.Occupation.Castor:
                    layerMask = lowTileMask | highTileMask;
                    break;
                default:
                    layerMask = lowTileMask;
                    break;
            }
            
            // 레이캐스트 실행
            var tempPos = new Vector3(tilePos.x, tilePos.y - 10f, tilePos.z);

            if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
            {
                // 레이가 오브젝트에 부딪혔을 때의 처리
                //Debug.Log("Hit " + hit.collider.gameObject.name);
                var tileContoller = hit.transform.GetComponent<Tile>();
                tileContoller.SetTileMaterial(Tile.TileMaterial.Attack);
                tempTiles.AddLast(tileContoller);
            }
        }
    }

    public void ClearTileMesh(LinkedList<Tile> tempTiles)
    {
        foreach(var tile in tempTiles)
        {
            tile.ClearTileMesh();
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
            // 레이캐스트 실행
            var tempPos = new Vector3(tilePos.x, tilePos.y - 10f, tilePos.z);

            if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
            {
                // 레이가 오브젝트에 부딪혔을 때의 처리
                //Debug.Log("Hit " + hit.collider.gameObject.name);
                //hit.transform.GetComponent<Tile>().SetTileMaterial(true, Tile.TileMaterial.Attack);
            }
            else
            {
                // 레이가 아무것도 부딪히지 않았을 때의 처리
                //Debug.Log("No hit");
            }
            Gizmos.DrawLine(tilePos, tilePos + Vector3.down * 1000); // 10은 레이의 길이
        }

        var playerCenterPos = player.transform.position;
        playerCenterPos.y = transform.position.y;

        //Gizmos.DrawSphere(playerCenterPos, radius);
    }

    public void SetPlayer(Transform player)
    {
        this.player = player.GetComponent<PlayerController>();
    }

    public void SetFirstArranger(CharacterIcon icon)
    {
        playerIcon = icon;
    }

    public void SetPositionToCurrentPlayer(Transform playerTr)
    {
        var tempPos = playerTr.position;
        tempPos.y += yOffset;
        transform.parent.position = tempPos;
    }
}
