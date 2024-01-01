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

        //public Image characterImage;
        //public Image mechaImage;
        //public Image attackAreaImage;
        //public Image skillAreaImage;
        //public Image skillIconImage;

        var characterImagePath = characterData.CharacterStanding;
        characterImage.sprite = Resources.Load<Sprite>(characterImagePath);

        // mecha 이미지 : 캐릭터아이디_mecha로 리소스 폴더에 찾아서 연결
        // attackArea, skillArea, skillIcon도 마찬가지
    }
}
