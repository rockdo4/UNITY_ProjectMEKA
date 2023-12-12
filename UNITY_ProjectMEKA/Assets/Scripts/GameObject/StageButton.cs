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
            //StageDataManager.Instance.selectedStageDatas.
            //StageDataManager.Instance.selectedStageData = 
        });
    }
}
