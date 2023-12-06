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

    public int maxCost = 20;
    public int cost;

    private bool created;
    private bool once;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        var characterStat = characterPrefab.GetComponent<CharacterState>();
        var cost = characterStat.arrangeCost;
    }

    private void Start()
    {
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
            playerController.stateManager.firstArranged = false;
            playerController.stateManager.secondArranged = false;
            playerController.stateManager.created = false;
            playerController.currentTile.arrangePossible = true;
            playerController.icon.gameObject.SetActive(true);
        });

        stageManager.currentPlayer = playerController;
        stageManager.currentPlayerIcon = playerController.icon;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var isCurrentPlayerThis = stageManager.currentPlayer == playerController;
        var isPossibleMode = (stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.None) || (stageManager.characterInfoUIManager.windowMode == CharacterInfoMode.FirstArrange);

        if (isPossibleMode || (isCurrentPlayerThis && isPossibleMode))
        {
            CreateCharacter();
            characterGo.transform.position = transform.position;
            once = false;
            stageManager.characterInfoUIManager.currentPlayerChanged = true;
        }
    }
}
