using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PlayerController;

// 이거다

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
            //var tempPos = transform.position;
            //tempPos.y -= 0.01f;
            Plane plane = new Plane(Vector3.up, transform.position);
            float enter;

            if (!EventSystem.current.IsPointerOverGameObject() && plane.Raycast(ray, out enter))
            {
                ArrangeDone.Invoke();
                Vector3 hitPoint = ray.GetPoint(enter);
                Debug.Log($"{hitPoint}, {collectButton.transform.position}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmo 색상 설정 (예: 녹색)
        Gizmos.color = Color.green;

        // Plane의 중심 위치
        Vector3 center = transform.position;

        // Plane의 반넓이 계산
        float halfWidth = 10f / 2f;
        float halfLength = 10f / 2f;

        // Plane의 네 꼭지점 계산
        Vector3 topLeft = center + new Vector3(-halfWidth, 0f, halfLength);
        Vector3 topRight = center + new Vector3(halfWidth, 0f, halfLength);
        Vector3 bottomLeft = center + new Vector3(-halfWidth, 0f, -halfLength);
        Vector3 bottomRight = center + new Vector3(halfWidth, 0f, -halfLength);

        // Gizmo를 사용하여 Plane의 선을 그리기
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
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
        stageManager.currentPlayer.ReturnPool.Invoke();
        transform.gameObject.SetActive(false);
    }

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
