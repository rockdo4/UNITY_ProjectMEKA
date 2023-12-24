using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public int stageID;
    private Button stageButton;

    private void Awake()
    {
        stageButton = GetComponent<Button>();
        stageButton.onClick.AddListener(() =>
        {
            var stageData = StageDataManager.Instance.selectedStageDatas[stageID];
            StageDataManager.Instance.selectedStageData = stageData;

            Debug.Log("������ �������� ID : " + StageDataManager.Instance.selectedStageData.stageID);
        });
    }
}
