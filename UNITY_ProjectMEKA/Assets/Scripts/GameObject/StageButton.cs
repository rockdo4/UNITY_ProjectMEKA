using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public int stageID;
    public GameObject hardBg;
    public GameObject hardTag;
    public Image monsterImage;
    public TextMeshProUGUI stageIndexText;
    public TextMeshProUGUI rewardGuideText;

    private Button stageButton;

    private void Awake()
    {
        stageButton = GetComponent<Button>();
        stageButton.onClick.AddListener(() =>
        {
            var stageData = StageDataManager.Instance.selectedStageDatas[stageID];
            StageDataManager.Instance.selectedStageData = stageData;

            Debug.Log("선택한 스테이지 ID : " + StageDataManager.Instance.selectedStageData.stageID);
        });
    }
}
