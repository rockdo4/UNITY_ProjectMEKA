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
        characterGo = Instantiate(characterPrefab);
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
            CoolTimeUpdate();
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
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var isCurrentPlayerThis = stageManager.currentPlayer == playerController;
        var isPossibleMode = (stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.None) || (stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.FirstArrange);
        var isCostEnough = stageManager.characterIconManager.currentCost > cost;

        if (isPossibleMode || (isCurrentPlayerThis && isPossibleMode))
        {
            if (arrangePossible && isCostEnough)
            {
                Debug.Log("플레이어 생성");
                CreateCharacter();
            }
            stageManager.currentPlayer = playerController;
            stageManager.currentPlayerIcon = playerController.icon;
            characterGo.transform.position = transform.position;
            once = false;
            stageManager.characterInfoUIManager.currentPlayerChanged = true;
        }
    }
}
