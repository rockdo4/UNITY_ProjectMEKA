using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PlayerController;

public class ArrangeJoystick : MonoBehaviour
{
    public Button cancelButton;
    public Button collectButton;
    public ArrangeJoystickHandler handler;

    private StageManager stageManager;
    private CharacterInfoUIManager characterInfoUIManager;
    private float yOffset = 1f;

    public UnityEvent ArrangeDone = new UnityEvent();

    private void Awake()
    {
        characterInfoUIManager = GameObject.FindGameObjectWithTag("CharacterInfoUIManager").GetComponent<CharacterInfoUIManager>();
        stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>();
        ArrangeDone = new UnityEvent();
        ArrangeDone.AddListener(ArrangeDoneEvent);
        cancelButton.onClick.AddListener(CancelEvent);
        collectButton.onClick.AddListener(CollectEvent);
    }

    private void OnEnable()
    {
        //if (settingMode)
        //{
        //    SettingModeInit();
        //}
        //else
        //{
        //    SecondArrangeInit();
        //}
    }

    private void Start()
    {
    }

    private void Update()
    {
        cancelButton.gameObject.SetActive(handler.cancelButtonOn);

        if (characterInfoUIManager.windowMode == Defines.CharacterInfoMode.Setting && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float enter;

            if (!Utils.IsUILayer() && plane.Raycast(ray, out enter))
            {
                Debug.Log("arrangeDone");
                ArrangeDone.Invoke();
            }
        }
    }

    //public void SecondArrangeInit()
    //{
    //    if (!handler.gameObject.activeSelf)
    //    {
    //        handler.gameObject.SetActive(true);
    //    }
    //    if (cancelButton.gameObject.activeSelf)
    //    {
    //        cancelButton.gameObject.SetActive(false);
    //    }
    //    if (collectButton.gameObject.activeSelf)
    //    {
    //        collectButton.gameObject.SetActive(false);
    //    }
    //}

    //public void SettingModeInit()
    //{
    //    if (handler.gameObject.activeSelf)
    //    {
    //        handler.gameObject.SetActive(false);
    //    }
    //    if (cancelButton.gameObject.activeSelf)
    //    {
    //        cancelButton.gameObject.SetActive(false);
    //    }
    //    if (!collectButton.gameObject.activeSelf)
    //    {
    //        collectButton.gameObject.SetActive(true);
    //    }
    //}

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

        if (arrangePossible)
        {
            stageManager.currentPlayer.currentTile.arrangePossible = false;
        }

        if (iconActive)
        {
            stageManager.currentPlayerIcon.gameObject.SetActive(false);

        }

        stageManager.currentPlayer.SetState(CharacterStates.Idle);
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
        stageManager.currentPlayer = null;
        stageManager.currentPlayerIcon = null;

        transform.gameObject.SetActive(false);
    }

    public void CollectEvent()
    {
        Debug.Log("collect event");
        if (collectButton.gameObject.activeSelf)
        {
            collectButton.gameObject.SetActive(false);
        }
        stageManager.currentPlayer.stateManager.firstArranged = false; stageManager.currentPlayer.stateManager.secondArranged = false;
        stageManager.currentPlayer.currentTile.arrangePossible = true;
        stageManager.currentPlayer.ReturnPool.Invoke();
        stageManager.currentPlayer = null;
        stageManager.currentPlayerIcon = null;

        transform.gameObject.SetActive(false);
    }

    public void SetPositionToCurrentPlayer(Transform playerTr)
    {
        var tempPos = playerTr.position;
        tempPos.y += yOffset;
        transform.position = tempPos;
        handler.transform.localPosition = Vector3.zero;
    }
}
