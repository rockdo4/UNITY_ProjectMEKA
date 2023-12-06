using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Defines;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    public StageManager stageManager;
    public GameObject characterPrefab;
    private GameObject characterGo;
    private PlayerController playerController;

    public bool isDie = false;
    public bool arrangePossible = true;

    public int maxCost = 20;
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
        arrangeCoolTime = characterStat.arrangeCoolTime;
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

        if(isDie)
        {
            timer += Time.deltaTime;
            if(timer >= arrangeCoolTime)
            {
                timer = 0f;
                isDie = false;
                arrangePossible = true;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        var isCurrentPlayerThis = stageManager.currentPlayer == playerController;
        var isPossibleMode = (stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.None) || (stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.FirstArrange);

        if (isPossibleMode || (isCurrentPlayerThis && isPossibleMode))
        {
            if (arrangePossible)
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
