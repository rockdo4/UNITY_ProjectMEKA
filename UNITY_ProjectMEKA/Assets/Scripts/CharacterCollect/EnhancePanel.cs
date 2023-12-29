using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhancePanel : MonoBehaviour
{
	[Header("소모 아이템 ID")]
	public int[] itemID;

	public TextMeshProUGUI levelText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI afterDamageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI afterArmorText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI afterHpText;
    public TextMeshProUGUI expText;
	public Image expBar;
	public TextMeshProUGUI expPercent;

    public ItemQuantityCard[] reportItemCard;
	[Header("이미지")]
	public Image characterImage;

	[Header("버튼")]
	public Button applyButton;
	public Button exitButton;
	public Button synchroButton;

	[Header("패널")]
	public SynchroPanel synchroPanel;
	public CharacterInfoText infoPanel;

	private Character currCharacter;
	private bool isFull = false;
	private List<ExpData> table;

	private void Awake()
	{
		applyButton.onClick.AddListener(() => 
		{
			infoPanel.SetPopUpPanel("강화하시겠습니까?", () => 
			{ 
				ExecuteUpgrade();
			}
			,"예", "아니오");
		});

		exitButton.onClick.AddListener(() =>
		{
			infoPanel.UpdateCharacter();
            gameObject.SetActive(false);
        });

		synchroButton.onClick.AddListener(() =>
		{
            synchroPanel.gameObject.SetActive(true);
            synchroPanel.SetCharacter(currCharacter);
			gameObject.SetActive(false);
        });
	}

	public void OnEnable()
	{
		for (int i = 0; i < reportItemCard.Length; i++)
		{
			reportItemCard[i].SetItem(itemID[i]);
			reportItemCard[i].SetText();
		}
		currCharacter = infoPanel.character;
		isFull = false;
	}

    private void Start()
    {
        table = DataTableMgr.GetTable<ExpTable>().GetOriginalTable();
    }

    public void ExecuteUpgrade()
	{
        ApplyUpgradeLevel();

        foreach (var card in reportItemCard)
        {
            card.ConsumeItem();
            card.SetText();
        }

        UpdateTargetLevel();
    }

	public void SetCharacter(Character character)
	{
		currCharacter = character;

		int characterID = currCharacter.CharacterID;
		int level = currCharacter.CharacterLevel;

		int result = CombineID(characterID, level);

		Debug.Log(("ID",result));
		var levelData = DataTableMgr.GetTable<CharacterLevelTable>().GetLevelData(result);

		characterImage.sprite = Resources.Load<Sprite>(currCharacter.CharacterStanding);

		UpdateTargetLevel();
	}

	public LevelData CalculateData(int totalExp, out int remain)
	{
        if (table == null)
            table = DataTableMgr.GetTable<ExpTable>().GetOriginalTable();

        int currentLevel = currCharacter.CharacterLevel;
		int targetLevel = currentLevel;
		int maxLevel = currCharacter.CharacterGrade * 10;


		while (totalExp > 0)
		{
			if (totalExp >= table[targetLevel - 1].RequireExp)
			{
				targetLevel++;
				if (targetLevel > maxLevel)
				{
					targetLevel--;
					break;
				}
				totalExp -= table[targetLevel - 1].RequireExp;
			}
			else
			{
				break;
			}
		}
		int characterID = currCharacter.CharacterID;
		int result = CombineID(characterID, targetLevel);

		remain = totalExp;
		return DataTableMgr.GetTable<CharacterLevelTable>().GetLevelData(result);
	}

	public void UpdateTargetLevel()
	{
		int totalExp = 0;
		int remainExp = 0;

		totalExp += currCharacter.CurrentExp;

		foreach (var card in reportItemCard)
		{
			int value = 0;
			if(card.item != null)
			{
				value = card.item.Value;
			}
			totalExp += (card.selectedQuantity * value);
		}

		var data = CalculateData(totalExp, out remainExp);
        int result = CombineID(currCharacter.CharacterID, currCharacter.CharacterLevel);
		var levelData = DataTableMgr.GetTable<CharacterLevelTable>().GetLevelData(result);
		if(table == null)
            table = DataTableMgr.GetTable<ExpTable>().GetOriginalTable();

		levelText.SetText($"{data.CharacterLevel}");
		damageText.SetText($"{levelData.CharacterDamage}");
		afterDamageText.SetText($"{data.CharacterDamage}");
		armorText.SetText($"{levelData.CharacterArmor}");
		afterArmorText.SetText($"{data.CharacterArmor}");
		hpText.SetText($"{levelData.CharacterHP}");
		afterHpText.SetText($"{data.CharacterHP}");

		//expText.SetText($"경험치 : {currCharacter.CurrentExp} >> {remainExp}");
		var ratio = (float)remainExp / table[data.CharacterLevel].RequireExp;
        if (ratio > 1) ratio = 1;
        expBar.fillAmount = ratio;
		expPercent.SetText($"{(int)(ratio * 100)}%");

        int maxLevel = currCharacter.CharacterGrade * 10;

        if(data.CharacterLevel >= maxLevel)
		{
            isFull = true;
        }
    }

	public void ApplyUpgradeLevel()
	{
		int totalExp = 0;
		int remainExp = 0;
		totalExp += currCharacter.CurrentExp;

		foreach (var card in reportItemCard)
		{
			int value = 0;
			if (card.item != null)
			{
				value = card.item.Value;
			}
			totalExp += (card.selectedQuantity * value);
		}

		var data = CalculateData(totalExp, out remainExp);

		currCharacter.CharacterLevel = data.CharacterLevel;
		currCharacter.CurrentExp = remainExp;

		int result = CombineID(currCharacter.CharacterID, currCharacter.CharacterLevel);
		var levelData = DataTableMgr.GetTable<CharacterLevelTable>().GetLevelData(result);
        if (table == null)
            table = DataTableMgr.GetTable<ExpTable>().GetOriginalTable();

        levelText.SetText($"{data.CharacterLevel}");
        damageText.SetText($"{levelData.CharacterDamage}");
        afterDamageText.SetText($"{data.CharacterDamage}");
        armorText.SetText($"{levelData.CharacterArmor}");
        afterArmorText.SetText($"{data.CharacterArmor}");
        hpText.SetText($"{levelData.CharacterHP}");
        afterHpText.SetText($"{data.CharacterHP}");

        //expText.SetText($"경험치 : {currCharacter.CurrentExp} >> {remainExp}");
        var ratio = (float)remainExp / table[data.CharacterLevel].RequireExp;
		if(ratio > 1) ratio = 1;
        expBar.fillAmount = ratio;
        expPercent.SetText($"{(int)(ratio * 100)}%");

        int maxLevel = currCharacter.CharacterGrade * 10;

        if (data.CharacterLevel >= maxLevel)
        {
            isFull = true;
        }

        GameManager.Instance.SaveExecution();
	}

	public int CombineID(int characterID, int level)
	{
		StringBuilder stringBuilder = new StringBuilder();
		//stringBuilder.Append(character.CharacterID);
		//임시로 고정 숫자 넣음 고쳐야 됨
		stringBuilder.Append(characterID);

		if (level < 10)
		{
			stringBuilder.Append(0);
			stringBuilder.Append(level);
		}
		else
			stringBuilder.Append(level);


		int.TryParse(stringBuilder.ToString(), out int result);

		return result;
	}

	public bool CheckFull()
	{
		if (isFull)
			return true;
		return false;
	}
}
