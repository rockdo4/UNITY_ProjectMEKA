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
    public Image attackAreaImage;
    public Image skillAreaImage;
    public Image skillIconImage;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI occupationText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI skillTypeText;
    public TextMeshProUGUI characterInfoText;

    private string characterImagePathID;

    private void OnEnable()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
        stringTable = StageDataManager.Instance.stringTable;
        skillInfoTable = DataTableMgr.GetTable<SkillInfoTable>();
        characterImagePathID = "Standing";        
    }

    private void Start()
    {
    }

    public void SetCharacterInfo()
    {
        var characterId = stageManager.currentPlayer.state.id;
        var characterState = stageManager.currentPlayer.state;
        var characterSkillType = stageManager.currentPlayer.skillState.skillType;
        var characterData = StageDataManager.Instance.characterTable.GetCharacterData(characterId);

        // �����ϱ�
        levelText.SetText(characterState.level.ToString());

        var occupationStringID = characterData.OccupationStringID;
        var occupation = stringTable.GetString(occupationStringID);
        occupationText.SetText(occupation);

        var nameStringID = characterData.CharacterNameStringID;
        var name = stringTable.GetString(nameStringID);
        nameText.SetText(name);

        var skillType = stringTable.GetString(characterSkillType.ToString());
        skillTypeText.SetText(skillType);

        var characterInfoStringID = characterData.OccupationInfoStringID;
        var characterInfo = stringTable.GetString(characterInfoStringID);
        characterInfoText.SetText(characterInfo);

        //public Image mechaImage;
        //public Image attackAreaImage;
        //public Image skillAreaImage;
        //public Image skillIconImage;

        var characterImagePath = characterData.CharacterStanding;
        characterImage.sprite = Resources.Load<Sprite>(characterImagePath);

        var skillID = characterData.SkillID;
        var skillDatas = skillInfoTable.GetSkillDatas(skillID);
        var skillImagePath = skillDatas[0].ImagePath;
        skillIconImage.sprite = Resources.Load<Sprite>(skillImagePath);

        // mecha �̹��� : ĳ���;��̵�_mecha�� ���ҽ� ������ ã�Ƽ� ����
        // attackArea, skillArea, skillIcon�� ��������
    }
}
