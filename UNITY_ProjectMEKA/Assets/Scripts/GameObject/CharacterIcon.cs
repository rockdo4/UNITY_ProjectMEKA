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
    private GameObject characterGo;
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
    private float timer = 0f;

    private bool created;
    private bool once;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
    }

    private void Start()
    {
        var characterStat = characterPrefab.GetComponent<CharacterState>();
        cost = characterStat.arrangeCost;
        costText.text = cost.ToString();
        arrangeCoolTime = characterStat.arrangeCoolTime;
        timer = arrangeCoolTime;
    }

    private void Update()
    {
        if (playerController != null)
        {
            if (!playerController.stateManager.created)
            {
                created = false;
            }
        }

        if(isDie || isCollected)
        {
            CoolTimeUpdate();
            if (isCollected)
            {
                // 코스트 회복
            }
        }
    }

    public void CreateCharacter()
    {
        var characterName = characterPrefab.GetComponent<CharacterState>().name;
        characterGo = ObjectPoolManager.instance.GetGo(characterName);
        playerController = characterGo.GetComponent<PlayerController>();
        created = true;
        playerController.stateManager.created = true;
        playerController.joystick = stageManager.arrangeJoystick.transform.gameObject;
        playerController.icon = this;

        var dieEvent = characterGo.GetComponent<CanDie>();
        dieEvent.action.AddListener(() =>
        {
            playerController.currentTile.arrangePossible = true;
            playerController.icon.isDie = true;
            playerController.icon.arrangePossible = false;
            playerController.ReturnPool.Invoke();
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
