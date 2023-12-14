﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PlayerController;
using static Defines;

public class ArrangeJoystick : MonoBehaviour
{
    public Button cancelButton;
    public Button collectButton;
    public Button skillButton;
    public ArrangeJoystickHandler handler;

    private StageManager stageManager;
    private IngameStageUIManager characterInfoUIManager;
    private float yOffset = 1f;

    public UnityEvent ArrangeDone = new UnityEvent();

    private void Awake()
    {
        characterInfoUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<IngameStageUIManager>();
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        ArrangeDone = new UnityEvent();
        ArrangeDone.AddListener(ArrangeDoneEvent);
        cancelButton.onClick.AddListener(CancelEvent);
        collectButton.onClick.AddListener(CollectEvent);
        skillButton.onClick.AddListener(SkillEvent);
    }

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        cancelButton.gameObject.SetActive(handler.cancelButtonOn);
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

        if (arrangePossible)
        {
            stageManager.currentPlayer.currentTile.arrangePossible = false;
        }

        if (iconActive)
        {
            stageManager.currentPlayerIcon.gameObject.SetActive(false);

        }
        //stageManager.characterIconManager.currentCharacterCount--;
        var cost = stageManager.currentPlayer.state.arrangeCost;
        stageManager.currentCost -= cost;
        stageManager.useCost += cost;
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
        //stageManager.currentPlayer.stateManager.firstArranged = false;
        //stageManager.currentPlayer.stateManager.secondArranged = false;
        //ClearTileMesh(tempTiles);
        stageManager.currentPlayer.currentTile.arrangePossible = true;
        //stageManager.currentPlayer.SetState(CharacterStates.Idle);
        stageManager.currentPlayerIcon.created = false;
        stageManager.currentPlayer.PlayerInit.Invoke();
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
        stageManager.currentPlayer.currentTile.arrangePossible = true;
        stageManager.currentPlayerIcon.created = false;
        stageManager.currentPlayerIcon.isCollected = true;
        stageManager.currentPlayerIcon.arrangePossible = false;

        // for tilemap test
        //var id = stageManager.currentPlayer.state.id;
        //var characterData = stageManager.characterIconManager.characterTable.GetCharacterData(id);
        //var withdrawCost = characterData.WithdrawCost;
        //stageManager.currentCost += withdrawCost;

        stageManager.currentPlayer.PlayerInit.Invoke();
        stageManager.currentPlayer = null;
        stageManager.currentPlayerIcon = null;

        gameObject.SetActive(false);
    }

    public void SkillEvent()
    {
        if (skillButton.gameObject.activeSelf)
        {
            skillButton.gameObject.SetActive(false);
        }
        stageManager.currentPlayer.gameObject.GetComponent<SkillBase>().UseSkill();
        stageManager.currentPlayer.SetState(CharacterStates.Idle);
        stageManager.currentPlayer = null;
        stageManager.currentPlayerIcon = null;

        gameObject.SetActive(false);
    }

    public void SetPositionToCurrentPlayer(Transform playerTr)
    {
        var tempPos = playerTr.position;
        tempPos.y += yOffset;
        transform.position = tempPos;
        handler.transform.localPosition = Vector3.zero;
    }
}
