using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Defines;

public class CharacterInfo : MonoBehaviour
{
    public StageManager stageManager;
    private StringTable stringTable;
    private SkillInfoTable skillInfoTable;

    public Image characterImage;
    public Image mechaImage;
    public Image attackRangeImage;
    public Image skillRangeImage;
    public Image skillIconImage;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI occupationText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillTypeText;
    public TextMeshProUGUI skillInfoText;

    private string skillNameStringIDKey;
    private string skillInfoStringIDKey;

    private void OnEnable()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        stringTable = StageDataManager.Instance.stringTable;
        skillInfoTable = DataTableMgr.GetTable<SkillInfoTable>();
        skillNameStringIDKey = "_skillName";
        skillInfoStringIDKey = "_skillInfo";
    }

    private void Start()
    {
    }

    public void SetCharacterInfo()
    {
        var characterId = stageManager.currentPlayer.state.id;
        var characterState = stageManager.currentPlayer.state;
        var characterSkillState = stageManager.currentPlayer.skillState;
        var characterData = StageDataManager.Instance.characterTable.GetCharacterData(characterId);

        // 수정하기
        levelText.SetText(characterState.level.ToString());

        var occupationStringID = characterData.OccupationStringID;
        var occupation = stringTable.GetString(occupationStringID);
        occupationText.SetText(occupation);

        var nameStringID = characterData.CharacterNameStringID;
        var name = stringTable.GetString(nameStringID);
        nameText.SetText(name);

        var skillType = stringTable.GetString(characterSkillState.skillType.ToString());
        skillTypeText.SetText(skillType);

        var characterInfoStringID = characterData.OccupationInfoStringID;
        var characterInfo = stringTable.GetString(characterInfoStringID);
        skillInfoText.SetText(characterInfo);

        var characterImagePath = characterData.CharacterStanding;
        characterImage.sprite = Resources.Load<Sprite>(characterImagePath);

        var mechaImagePath = characterData.MechaImagePath;
        mechaImage.sprite = Resources.Load<Sprite>(mechaImagePath);

        var attackRangeImagePath = characterData.AttackRangeImagePath;
        attackRangeImage.sprite = Resources.Load<Sprite>(attackRangeImagePath);

        var skillID = characterData.SkillID;
        //var skillLevelID = characterSkillState.
        var skillDatas = skillInfoTable.GetSkillDatas(skillID);
        var skillImagePath = skillDatas[0].ImagePath;
        skillIconImage.sprite = Resources.Load<Sprite>(skillImagePath);

        var skillRangeImagePath = characterData.SkillRangeImagePath;
        skillRangeImage.sprite = Resources.Load<Sprite>(skillRangeImagePath);

        var skillNameStringID = new string($"{skillID}{skillNameStringIDKey}");
        var skillName = stringTable.GetString(skillNameStringID);
        skillNameText.SetText(skillName);

        //var skillInfoStringID = new string($"{}");
    }
}
