using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PlayerController;

public class ArrangeJoystick : MonoBehaviour
{
    public Button cancelButton;
    public Button collectButton;
    public ArrangeJoystickHandler handler;

    private StageManager stageManager;
    private float yOffset = 1f;

    public UnityEvent ArrangeDone = new UnityEvent();

    public bool settingMode;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
        ArrangeDone = new UnityEvent();
        ArrangeDone.AddListener(ArrangeDoneEvent);
        cancelButton.onClick.AddListener(CancelEvent);
        collectButton.onClick.AddListener(CollectEvent);
    }

    private void OnEnable()
    {
        if(settingMode)
        {
            SettingModeInit();
        }
        else
        {
            SecondArrangeInit();
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if(handler.cancelButtonOn)
        {
            if(!cancelButton.gameObject.activeSelf)
            {
                cancelButton.gameObject.SetActive(true);
            }
        }

        if(settingMode && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float enter;

            if (plane.Raycast(ray, out enter))
            {
                ArrangeDone.Invoke();
                Vector3 hitPoint = ray.GetPoint(enter);
                Debug.Log($"{hitPoint}, {collectButton.transform.position}");
            }
        }
    }

    public void SecondArrangeInit()
    {
        if(!handler.gameObject.activeSelf)
        {
            handler.gameObject.SetActive(true);
        }
        if (cancelButton.gameObject.activeSelf)
        {
            cancelButton.gameObject.SetActive(false);
        }
        if (collectButton.gameObject.activeSelf)
        {
            collectButton.gameObject.SetActive(false);
        }
    }

    public void SettingModeInit()
    {
        if (handler.gameObject.activeSelf)
        {
            handler.gameObject.SetActive(false);
        }
        if (cancelButton.gameObject.activeSelf)
        {
            cancelButton.gameObject.SetActive(false);
        }
        if (!collectButton.gameObject.activeSelf)
        {
            collectButton.gameObject.SetActive(true);
        }
    }

    public void ArrangeDoneEvent()
    {
        Debug.Log("arrange done");

        var secondArranged = stageManager.currentPlayer.stateManager.secondArranged;
        var arrangePossible = stageManager.currentPlayer.currentTile.arrangePossible;
        var iconActive = stageManager.currentPlayerIcon.gameObject.activeSelf;

        if (!secondArranged)
        {
            stageManager.currentPlayer.stateManager.secondArranged = true;
        }
        
        if(arrangePossible)
        {
            stageManager.currentPlayer.currentTile.arrangePossible = false;
        }

        if(iconActive)
        {
            stageManager.currentPlayerIcon.gameObject.SetActive(false);

        }

        stageManager.currentPlayer.SetState(CharacterStates.Idle);
        //ClearTileMesh(tempTiles);

        stageManager.currentPlayer = null;
        stageManager.currentPlayerIcon = null;

        transform.gameObject.SetActive(false);
    }

    public void CancelEvent()
    {
        if (cancelButton.gameObject.activeSelf)
        {
            cancelButton.gameObject.SetActive(false);
        }
        stageManager.currentPlayer.stateManager.firstArranged = false;
        stageManager.currentPlayer.stateManager.secondArranged = false;
        //ClearTileMesh(tempTiles);
        stageManager.currentPlayer.currentTile.arrangePossible = true;
        //stageManager.currentPlayer.SetState(CharacterStates.Idle);
        stageManager.currentPlayer.ReturnPool.Invoke();
        transform.gameObject.SetActive(false);
    }

    public void CollectEvent()
    {
        Debug.Log("collect event");
        if (collectButton.gameObject.activeSelf)
        {
            collectButton.gameObject.SetActive(false);
        }
        stageManager.currentPlayer.stateManager.secondArranged = false;
        stageManager.currentPlayer.currentTile.arrangePossible = true;
        //ClearTileMesh(tempTiles);
        stageManager.currentPlayer.ReturnPool.Invoke();
        transform.gameObject.SetActive(false);
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
    //            // 레이가 오브젝트에 부딪혔을 때의 처리
    //            //Debug.Log("Hit " + hit.collider.gameObject.name);
    //            var tileContoller = hit.transform.GetComponent<Tile>();
    //            tileContoller.SetTileMaterial(Tile.TileMaterial.Attack);
    //            tempTiles.AddLast(tileContoller);
    //        }
    //    }
    //}

    public void ClearTileMesh(LinkedList<Tile> tempTiles)
    {
        foreach(var tile in tempTiles)
        {
            tile.ClearTileMesh();
        }
        tempTiles.Clear();
    }

    public void SetPositionToCurrentPlayer(Transform playerTr)
    {
        var tempPos = playerTr.position;
        tempPos.y += yOffset;
        transform.position = tempPos;
    }
}
