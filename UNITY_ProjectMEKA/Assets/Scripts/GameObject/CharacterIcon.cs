using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Defines;
using TMPro;
using static PlayerController;

public class CharacterIcon : MonoBehaviour, IPointerDownHandler
{
    public StageManager stageManager;
    public GameObject characterPrefab;
    public GameObject characterGo;
    private PlayerController playerController;
    private CharacterState characterState;

    public Image characterImage;
    public TextMeshProUGUI costText;
    public Image propertyImage;
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
        SetSkill(characterGo);

		playerController = characterGo.GetComponent<PlayerController>();
        characterState = characterGo.GetComponent<CharacterState>();

        Init();

        characterGo.SetActive(false);
    }

    public void Init()
    {
        cost = playerController.state.arrangeCost;
        costText.text = cost.ToString();

        var id = characterState.id;
        var characterImagePath = StageDataManager.Instance.characterTable.GetCharacterData(id).ImagePath;
        characterImage.sprite = Resources.Load<Sprite>(characterImagePath);
        propertyImage.sprite = characterState.property switch
        {
            Property.Prime => Resources.Load<Sprite>("CharacterIcon/PrimeIcon"),
            Property.Grieve => Resources.Load<Sprite>("CharacterIcon/GrieveIcon"),
            Property.Edila => Resources.Load<Sprite>("CharacterIcon/EdilaIcon"),
            Property.None => Resources.Load<Sprite>("CharacterIcon/NoneIcon"),
        }; 

        arrangeCoolTime = playerController.state.arrangeCoolTime;
        timer = arrangeCoolTime;
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

    public void SetSkill(GameObject characterGo)
    {
        var state = characterGo.GetComponent<PlayerState>();
        var character = CharacterManager.Instance.m_CharacterStorage[state.id];

        var table = DataTableMgr.GetTable<SkillInfoTable>();
        var datas = table.GetSkillDatas(character.SkillID);
        var data = datas[character.SkillLevel - 1];

		if(characterGo.GetComponent<BuffSkilType>() != null)
        {
            var skill = characterGo.GetComponent<BuffSkilType>();
            skill.skillCost = data.UseSigma;
            skill.skillCoolTime = data.CoolTime;
            skill.skillDuration = data.Duration;
            skill.figure = (data.SkillDamageCoefficient != 0) ? data.SkillDamageCoefficient : data.SkillValueIncrease;
            skill.addCost = (data.ArrangeCostSec != 0) ? data.ArrangeCostSec : data.ArrangeCostRecovery;
		}
        else if(characterGo.GetComponent<SnipingSkillType>() != null)
        {
			var skill = characterGo.GetComponent<SnipingSkillType>();
			skill.skillCost = data.UseSigma;
			skill.skillCoolTime = data.CoolTime;
			skill.duration = data.Duration;
			skill.figure = (data.SkillDamageCoefficient != 0) ? data.SkillDamageCoefficient : data.SkillValueIncrease;
		}
        else if(characterGo.GetComponent<HitSkillType>() != null)
        {
			var skill = characterGo.GetComponent<HitSkillType>();
			skill.skillCost = data.UseSigma;
			skill.skillCoolTime = data.CoolTime;
			skill.skillDuration = data.Duration;
			skill.figure = (data.SkillDamageCoefficient != 0) ? data.SkillDamageCoefficient : data.SkillValueIncrease;
		}
        else if(characterGo.GetComponent<ProjectileTypeSkill>() != null)
        {
			var skill = characterGo.GetComponent<ProjectileTypeSkill>();
			skill.skillCost = data.UseSigma;
			skill.skillCoolTime = data.CoolTime;
		}
        else if(characterGo.GetComponent<ChageAttackStileSkill>() != null)
        {
			var skill = characterGo.GetComponent<ChageAttackStileSkill>();
			skill.skillCost = data.UseSigma;
			skill.skillCoolTime = data.CoolTime;
			skill.skillDuration = data.Duration;
		}
        else if(characterGo.GetComponent<PassiveTypeSkill>() != null)
        {
			var skill = characterGo.GetComponent<PassiveTypeSkill>();
			skill.skillCost = data.UseSigma;
			skill.skillCoolTime = data.CoolTime;
			skill.figure = (data.SkillDamageCoefficient != 0) ? data.SkillDamageCoefficient : data.SkillValueIncrease;
		}
        else if(characterGo.GetComponent<NukeSkill>() != null)
        {
			var skill = characterGo.GetComponent<NukeSkill>();
			skill.skillCost = data.UseSigma;
			skill.skillCoolTime = data.CoolTime;
		}
        else if(characterGo.GetComponent<LaserSkillType>() != null)
        {
			var skill = characterGo.GetComponent<LaserSkillType>();
			skill.skillCost = data.UseSigma;
			skill.skillCoolTime = data.CoolTime;
			skill.skillDuration = data.Duration;
			skill.figure = (data.SkillDamageCoefficient != 0) ? data.SkillDamageCoefficient : data.SkillValueIncrease;
		}
	}

	public void ActiveCharacter()
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
            Debug.Log("����");
        }
        var isPossibleMode = (stageManager.ingameStageUIManager.windowMode == WindowMode.None) || (stageManager.ingameStageUIManager.windowMode == WindowMode.FirstArrange);
        var isCostEnough = stageManager.currentCost > cost;

        if (isPossibleMode || (isCurrentPlayerThis && isPossibleMode))
        {
            if (arrangePossible && isCostEnough)
            {
                ActiveCharacter();
            }
            stageManager.currentPlayer = playerController;
            stageManager.currentPlayerIcon = playerController.icon;
            characterGo.transform.position = transform.position;
            once = false;
            stageManager.ingameStageUIManager.currentPlayerChanged = true;
        }
    }
}
