using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    protected GameObject optionPanel;
    protected Button optionButton;

    public Sprite[] soundButtonSprites = new Sprite[2];
    public Sprite[] pauseButtonSprites = new Sprite[2];
    public Sprite[] speedButtonSprites = new Sprite[4];

    public Button soundButton; // sprite : soundOn, soundMute
    public Button exitButton;
    public Button pauseButton; // sprite : pause, play
    public Button speedButton; // sprite : 1,2,4,8 ¹è¼Ó

    private string buttonLayerName;

    private void Awake()
    {
        buttonLayerName = LayerMask.LayerToName(gameObject.layer);
        optionButton = GetComponent<Button>();
        optionPanel = transform.GetChild(0).gameObject;
        optionButton.onClick.AddListener(OpenAndCloseOptionPanel);
        soundButton.onClick.AddListener(() => { ChangeButtonSprite(soundButton, soundButtonSprites); });
        pauseButton.onClick.AddListener(() => { ChangeButtonSprite(pauseButton, pauseButtonSprites); });
        speedButton.onClick.AddListener(() => { ChangeButtonSprite(speedButton, speedButtonSprites); });
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

    public void ChangeButtonSprite(Button button, Sprite[] spritePool)
    {
        //var currentSprite = button.GetComponent<Image>().sprite;
        for(int i = 0; i < spritePool.Length; ++i)
        {
            if (button.GetComponent<Image>().sprite == spritePool[i])
            {
                if(i >= soundButtonSprites.Length - 1)
                {
                    button.GetComponent<Image>().sprite = spritePool[0];
                }
                else
                {
                    button.GetComponent<Image>().sprite = spritePool[i + 1];
                }
                break;
            }
        }
    }

    //public void OnClickPauseButton()
    //{
    //    var pauseButtonSprite = pauseButton.GetComponent<Image>().sprite;
    //    for (int i = 0; i < pauseButtonSprites.Length; ++i)
    //    {
    //        if (pauseButtonSprite == pauseButtonSprites[i])
    //        {
    //            if (i >= soundButtonSprites.Length - 1)
    //            {
    //                pauseButtonSprite = pauseButtonSprites[0];
    //            }
    //            else
    //            {
    //                pauseButtonSprite = pauseButtonSprites[i + 1];
    //            }
    //            break;
    //        }
    //    }
    //}
}
