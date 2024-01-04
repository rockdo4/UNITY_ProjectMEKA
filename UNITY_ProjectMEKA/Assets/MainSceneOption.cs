using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneOption : MonoBehaviour
{
    public Button soundButton;
    public Button exitButton;
    public Sprite soundMuteImage;
    private Sprite saveSoundImage;

    private void Awake()
    {
        gameObject.SetActive(true);
        saveSoundImage = soundButton.image.sprite;

        soundButton.onClick.AddListener(() => 
        {
            if(SoundManager.instance.muteVolum)
            {
                soundButton.image.sprite = saveSoundImage;
            }
            else
            {
                soundButton.image.sprite = soundMuteImage;
            }
            SoundManager.instance.OnOffMasterVolume(SoundManager.instance.muteVolum);
            SoundManager.instance.muteVolum = !SoundManager.instance.muteVolum;
        });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
            Debug.Log("게임 종료");
        });
       
    }
}
