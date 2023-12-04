using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrangeJoystickHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    private List<GameObject> directions = new List<GameObject>();
    private Bounds backgroundBounds;
    private List<Bounds> bounds;
    private GameObject currentTile;
    private GameObject prevTile;
    private float half;
    //LinkedList<Tile> tempTiles = new LinkedList<Tile>();
    public float radius;

    public bool cancelButtonOn;
    
    private StageManager stageManager;
    private ArrangeJoystick joystick;
    private CharacterInfoUIManager characterInfoUIManager;

    private void Awake()
    {
        InitOnce();
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        cancelButtonOn = false;
    }

    public void Init()
    {
        transform.localPosition = Vector3.zero;
        currentTile = null;
    }

    public void InitOnce()
    {
        joystick = transform.parent.GetComponent<ArrangeJoystick>();
        characterInfoUIManager = GameObject.FindGameObjectWithTag("CharacterInfoUIManager").GetComponent<CharacterInfoUIManager>();
        stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();

        var boxCollider = GetComponent<BoxCollider>();
        half = boxCollider.bounds.size.x / 2f;
        backgroundBounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 0f));

        bounds = new List<Bounds>
        {
            new Bounds(new Vector3(0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, -0.5f, 0f), new Vector3(1f, 1f, 0f)),
            new Bounds(new Vector3(-0.5f, 0.5f, 0f), new Vector3(1f, 1f, 0f))
        };

        var parent = transform.parent;
        for (int i = 0; i < (int)Defines.RotationDirection.Count; ++i)
        {
            directions.Add(parent.GetChild(i).gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!stageManager.currentPlayer.stateManager.secondArranged)
        {
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!stageManager.currentPlayer.stateManager.secondArranged)
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
            for(int i = 0; i < (int)Defines.RotationDirection.Count; ++i)
            {
                if (bounds[i].Contains(transform.localPosition))
                {
                    currentTile = directions[i];
                    if(prevTile != currentTile)
                    {
                        Debug.Log($"{prevTile}에서 {currentTile}로 바뀜");
                    }
                    break;
                }
            }

            RotateHandler(currentTile.transform, false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        characterInfoUIManager.ClearTileMesh();

        if (!stageManager.currentPlayer.stateManager.secondArranged)
        {
            // 핸들러가 플레이어 좌표 기준 일정 반경 내에 있을 때
            // 배치 취소 버튼 활성화
            var playerCenterPos = stageManager.currentPlayer.transform.position;
            playerCenterPos.y = transform.position.y;

            if(Vector3.Distance(transform.position, playerCenterPos) < radius)
            {
                Debug.Log("플레이어 반경 내");
                cancelButtonOn = true;

                // 핸들러 로컬 포지션 0,0 고정
                transform.localPosition = Vector3.zero;
            }
            else
            {
                RotateHandler(currentTile.transform, true);
                joystick.ArrangeDone.Invoke();
                //player.stateManager.secondArranged = true;
                //ArrangeDone.Invoke();
            }
        }

        //else if(collectButton.IsActive())
        //{
        //    Debug.Log("collect button on");
        //    // 바깥에 눌렀을 때 취소되도록
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    Plane plane = new Plane(Vector3.up, transform.position);
        //    float enter;
        //    if (plane.Raycast(ray, out enter))
        //    {
        //        player.SetState(CharacterStates.Idle);
        //        transform.parent.gameObject.SetActive(false);
        //    }

        //}
    }

    public void RotateHandler(Transform currentTileParent, bool snap)
    {
        var pos = currentTileParent.position;
        var go = currentTileParent.gameObject;
        var playerState = stageManager.currentPlayer.stateManager.currentBase as PlayableArrangeState;
        if (go == directions[(int)Defines.RotationDirection.Up])
        {
            if (snap)
            {
                pos.z += -half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Up);
                playerState.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
                //if (player != null)
                //{
                //    ChangeTileMesh();
                //}
            }
        }
        else if (go == directions[(int)Defines.RotationDirection.Right])
        {
            if (snap)
            {
                pos.x += -half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Right);
                playerState.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
                //ChangeTileMesh();
            }
        }
        else if (go == directions[(int)Defines.RotationDirection.Down])
        {
            if (snap)
            {
                pos.z += half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Down);
                playerState.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
                //ChangeTileMesh();
            }
        }
        else
        {
            if (snap)
            {
                pos.x += half;
            }
            else
            {
                playerState.RotatePlayer(Defines.RotationDirection.Left);
                playerState.AttackableTileSet(stageManager.currentPlayer.state.occupation);
                characterInfoUIManager.ChangeAttackableTileMesh();
                //ChangeTileMesh();
            }
        }

        if (snap)
        {
            transform.position = pos;
        }
    }

    //public void ChangeTileMesh()
    //{
    //    //Debug.Log("ChangeTileMesh");
    //    ClearTileMesh(tempTiles);
    //    var state = player.stateManager.currentBase as PlayableArrangeState;
    //    state.AttackableTileSet();
    //    foreach (var tilePos in player.attakableTiles)
    //    {
    //        RaycastHit hit;

    //        //공중형이면 레이어 마스크 : 공중 + 지상
    //        //지상형이면 레이어 마스크 : 지상
    //        int layerMask = 0;
    //        int lowTileMask = 1 << LayerMask.NameToLayer("LowTile");
    //        int highTileMask = 1 << LayerMask.NameToLayer("HighTile");

    //        switch((int)player.transform.GetComponent<CharacterState>().occupation)
    //        {
    //            case (int)Defines.Occupation.Hunter:
    //            case (int)Defines.Occupation.Castor:
    //                layerMask = lowTileMask | highTileMask;
    //                break;
    //            default:
    //                layerMask = lowTileMask;
    //                break;
    //        }

    //        // 레이캐스트 실행
    //        var tempPos = new Vector3(tilePos.x, tilePos.y - 10f, tilePos.z);

    //        if (Physics.Raycast(tempPos, Vector3.up, out hit, Mathf.Infinity, layerMask))
    //        {
    //            var tileContoller = hit.transform.GetComponent<Tile>();
    //            tileContoller.SetTileMaterial(Tile.TileMaterial.Attack);
    //            tempTiles.AddLast(tileContoller);
    //        }
    //    }
    //}

    //public void ClearTileMesh(LinkedList<Tile> tempTiles)
    //{
    //    foreach(var tile in tempTiles)
    //    {
    //        tile.ClearTileMesh();
    //    }
    //    tempTiles.Clear();
    //}

    //public void SetFirstArranger(CharacterIcon icon)
    //{
    //    playerIcon = icon;
    //    stageManager = playerIcon.stageManager;
    //    player = stageManager.currentPlayer;
    //}

    //public void SetPositionToCurrentPlayer(Transform playerTr)
    //{
    //    var tempPos = playerTr.position;
    //    tempPos.y += yOffset;
    //    transform.parent.position = tempPos;
    //}
}
