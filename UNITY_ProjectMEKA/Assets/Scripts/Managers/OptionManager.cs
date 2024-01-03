using UnityEngine;
using UnityEngine.UI;
using static Defines;

public enum SoundType
{
    SoundOn,
    SoundOff
}

public enum PlayType
{
    Play,
    Pause
}

public enum SpeedType
{
    x1,
    x2,
    x3,
}

public class OptionManager : MonoBehaviour
{
    protected GameObject optionPanel;
    protected Button optionButton;
    private IngameStageUIManager ingameUIManager;
    private StageManager stageManager;

    public Sprite[] soundButtonSprites = new Sprite[2];
    public Sprite[] pauseButtonSprites = new Sprite[2];
    public Sprite[] speedButtonSprites = new Sprite[3];

    public Button soundButton; // sprite : soundOn, soundMute
    public Button exitButton;
    public Button pauseButton; // sprite : pause, play
    public Button speedButton; // sprite : 1,2,3 ¹è¼Ó

    public SoundType currentSoundType;
    public PlayType currentPlayType;
    public SpeedType currentSpeedType;

    private string buttonLayerName;

    public StageUIManager stageUIManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
   
        ingameUIManager = GameObject.FindGameObjectWithTag(Tags.characterInfoUIManager).GetComponent<IngameStageUIManager>();

        buttonLayerName = LayerMask.LayerToName(gameObject.layer);
        optionButton = GetComponent<Button>();
        optionPanel = transform.GetChild(0).gameObject;
        optionButton.onClick.AddListener(OpenAndCloseOptionPanel);

        exitButton.onClick.AddListener(ingameUIManager.CloseScene);
        soundButton.onClick.AddListener(() => 
        { 
            ChangeButtonSpriteAndState(soundButton, soundButtonSprites); 
        });
        pauseButton.onClick.AddListener(() => 
        { 
            ChangeButtonSpriteAndState(pauseButton, pauseButtonSprites);
            PlayOrPause();
        });
        speedButton.onClick.AddListener(() => 
        { 
            ChangeButtonSpriteAndState(speedButton, speedButtonSprites);
            ChangePlaySpeed();
        });
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

    public void ChangeButtonSpriteAndState(Button button, Sprite[] spritePool)
    {
        var index = 0;
        for (int i = 0; i < spritePool.Length; ++i)
        {
            if (button.GetComponent<Image>().sprite == spritePool[i])
            {
                if(i >= spritePool.Length - 1)
                {
                    button.GetComponent<Image>().sprite = spritePool[0];
                    index = 0;
                }
                else
                {
                    button.GetComponent<Image>().sprite = spritePool[i + 1];
                    index = i + 1;
                }
                break;
            }
        }

        if(button == soundButton)
        {
            currentSoundType = (SoundType)index;
            SoundManager.instance.OnOffMasterVolume(SoundManager.instance.muteVolum);
            SoundManager.instance.muteVolum = !SoundManager.instance.muteVolum;
            
        }
        else if(button == pauseButton)
        {
            currentPlayType  = (PlayType)index;
        }
        else if(button == speedButton)
        {
            currentSpeedType = (SpeedType)index;
        }
    }

    public void PlayOrPause()
    {
        if(currentPlayType == PlayType.Play)
        {
            Time.timeScale = stageManager.CurrentSpeed;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void ChangePlaySpeed()
    {
        switch (currentSpeedType)
        {
            case SpeedType.x1:
                stageManager.CurrentSpeed = 1f;
                break;
            case SpeedType.x2:
                stageManager.CurrentSpeed = 2f;
                break;
            case SpeedType.x3:
                stageManager.CurrentSpeed = 3f;
                break;
        }
        Time.timeScale = stageManager.CurrentSpeed;
    }
}
