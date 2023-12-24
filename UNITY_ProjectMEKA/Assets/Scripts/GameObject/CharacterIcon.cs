using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Defines;
using TMPro;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    public StageManager stageManager;
    public GameObject characterPrefab;
    public GameObject characterGo;
    private PlayerController playerController;

    public TextMeshProUGUI costText;
    public GameObject redFilter;
    public GameObject blackFilter;
    public Slider coolTimeSlider;
    public TextMeshProUGUI coolTimeText;

    public bool isDie = false;
    public bool isCollected = false;
    public bool arrangePossible = true;

    private float arrangeCoolTime;
    private int cost;
    private float timer;

    public bool created;
    private bool once;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
    }

    private void Start()
    {
        //Instantiate Character
        characterGo = Instantiate(characterPrefab);
		SetObjectPooling(characterGo);

		playerController = characterGo.GetComponent<PlayerController>();
        cost = playerController.state.arrangeCost;
        costText.text = cost.ToString();
        arrangeCoolTime = playerController.state.arrangeCoolTime;
        timer = arrangeCoolTime;
        characterGo.SetActive(false);
    }

    private void Update()
    {
        if(isDie || isCollected)
        {
            if(blackFilter.activeSelf)
            {
                blackFilter.SetActive(false);
            }
            CoolTimeUpdate();
        }
        else
        {
            CheckCostEnough();
        }
    }

	public void SetObjectPooling(GameObject characterGo)
	{
		if (characterGo == null)
		{
			Debug.Log("character is null");
			return;
		}

		var objects = characterGo.GetComponent<PlayerState>().objectInfos;

        if(objects == null)
        {
            Debug.Log("info is null");
			return;
        }

		foreach (var data in objects)
        {
            ObjectPoolManager.instance.AddObjectToPool(data.objectName, data.perfab, data.count);
        }
	}

	public void CreateCharacter()
    {
        characterGo.SetActive(true);
        created = true;
        playerController.joystick = stageManager.arrangeJoystick.transform.gameObject;
        playerController.icon = this;
        playerController.SetState(PlayerController.CharacterStates.Arrange);
        //stageManager.characterIconManager.currentCost -= cost;

        var dieEvent = characterGo.GetComponent<CanDie>();
        dieEvent.action.AddListener(() =>
        {
            playerController.currentTile.arrangePossible = true;
            playerController.icon.isDie = true;
            playerController.icon.arrangePossible = false;
            playerController.PlayerInit.Invoke();
        });
    }

    public void CoolTimeUpdate()
    {
        if (!redFilter.activeSelf)
        {
            redFilter.SetActive(true);
            coolTimeSlider.gameObject.SetActive(true);
        }

        timer -= Time.deltaTime;
        coolTimeText.text = timer.ToString("0.0");
        coolTimeSlider.value = 1 - (timer / arrangeCoolTime);

        if (timer <= 0f)
        {
            timer = arrangeCoolTime;
            isDie = false;
            isCollected = false;
            arrangePossible = true;
            redFilter.SetActive(false);
            coolTimeSlider.gameObject.SetActive(false);
            //stageManager.characterIconManager.currentCharacterCount++;
        }
    }

    public void CheckCostEnough()
    {
        var isCostEnough = stageManager.currentCost > cost;
        if(!isCostEnough && !blackFilter.activeSelf)
        {
            blackFilter.SetActive(true);
        }
        else if(isCostEnough && blackFilter.activeSelf)
        {
            blackFilter.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var isCurrentPlayerThis = stageManager.currentPlayer == playerController;

        if(stageManager.ingameStageUIManager == null)
        {
            Debug.Log("Ёнюс");
        }
        var isPossibleMode = (stageManager.ingameStageUIManager.windowMode == WindowMode.None) || (stageManager.ingameStageUIManager.windowMode == WindowMode.FirstArrange);
        var isCostEnough = stageManager.currentCost > cost;

        if (isPossibleMode || (isCurrentPlayerThis && isPossibleMode))
        {
            if (arrangePossible && isCostEnough)
            {
                CreateCharacter();
            }
            stageManager.currentPlayer = playerController;
            stageManager.currentPlayerIcon = playerController.icon;
            characterGo.transform.position = transform.position;
            once = false;
            stageManager.ingameStageUIManager.currentPlayerChanged = true;
        }
    }
}
