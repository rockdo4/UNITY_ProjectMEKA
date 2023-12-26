using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    protected GameObject optionPanel;
    protected Button optionButton;
    private string buttonLayerName;

    private void Awake()
    {
        buttonLayerName = LayerMask.LayerToName(gameObject.layer);
        optionButton = GetComponent<Button>();
        optionPanel = transform.GetChild(0).gameObject;
        optionButton.onClick.AddListener(OpenAndCloseOptionPanel);
    }

    private void Update()
    {
        var isOptionPanelOpend = optionPanel.activeSelf;
        var isMouseOnOptionButton = Utils.IsButtonLayer(buttonLayerName);

        if (Input.GetMouseButtonDown(0) && isOptionPanelOpend && !isMouseOnOptionButton)
        {
            CloseOptionPanel();
        }
    }

    public void OpenAndCloseOptionPanel()
    {
        if(optionPanel.activeSelf)
        {
            CloseOptionPanel();
        }
        else
        {
            OpenOptionPanel();
        }
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
}
