using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static Defines;

// 싱글톤
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    public AudioMixer audioMixer;
    public List<AudioSource> bgmAudioSource;
    public List<AudioSource> SFXAudioSource;
    private string currentSceneName;
    private string saveMainScene;
    private StageManager stageManager;
    [System.Serializable]
    public class AudioClipList
    {
        public string name;
        public AudioClip clip;
    }
    public List<AudioClipList> audioClips;
    private Dictionary<string, AudioClip> dicAudioClips;
    public bool muteVolum;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        dicAudioClips = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
        {
            if (!dicAudioClips.ContainsKey(clip.name))
            {
                dicAudioClips.Add(clip.name, clip.clip);
            }
            else
            {
                Debug.Log("사운드 딕셔너리 이름 겹침");
            }

        }
    }

    private void Start()
    {
        //Init();
        currentSceneName = SceneManager.GetActiveScene().name;
        saveMainScene = currentSceneName;
        PlayBGM("MainSceneBGM");

    }
    private void Update()
    {
        if(currentSceneName != SceneManager.GetActiveScene().name) 
        {
            currentSceneName = SceneManager.GetActiveScene().name;
            GameObject stageManagerObject = GameObject.FindGameObjectWithTag(Tags.stageManager);
            if (stageManagerObject != null)
            {
                stageManager = GameObject.FindGameObjectWithTag(Tags.stageManager).GetComponent<StageManager>();
                switch (stageManager.stageID)
                    {
                        case 91101://0-1
                            PlayBGM("Stage1~2");
                            stageManager = null;
                            break;
                        case 91102://0-2
                            PlayBGM("Stage1~2");
                            stageManager = null;
                            break;
                        case 91103://0-3
                            PlayBGM("Stage3");
                            stageManager = null;
                            break;
                        case 91104://0-4
                            PlayBGM("Stage4~6");
                            stageManager = null;
                            break;
                        case 91105://0-5
                            PlayBGM("Stage4~6");
                            stageManager = null;
                            break;
                        case 91106://0-6
                            PlayBGM("Stage4~6");
                            stageManager = null;
                            break;
                        case 91107://0-7
                            PlayBGM("Stage7~9");
                            stageManager = null;
                            break;
                        case 91108://0-8
                            PlayBGM("Stage7~9");
                            stageManager = null;
                            break;
                        case 91109://0-9
                            PlayBGM("Stage7~9");
                            stageManager = null;
                            break;
                        case 91110://0-10
                            PlayBGM("Stage10");
                            stageManager = null;
                            break;
                        case 91111://1-1
                            PlayBGM("Stage11~12");
                            stageManager = null;
                            break;
                        case 91112://1-2
                            PlayBGM("Stage11~12");
                            stageManager = null;
                            break;
                        case 91113://1-3
                            PlayBGM("Stage13");
                            stageManager = null;
                            break;
                        case 91114://1-4
                            PlayBGM("Stage14");
                            stageManager = null;
                            break;
                        case 931221://CN-1
                            PlayBGM("ChallengeStage");
                            stageManager = null;
                            break;
                        case 931222://CN-2
                            PlayBGM("ChallengeStage");
                            stageManager = null;
                            break;
                        case 921211://PR-1
                            PlayBGM("PracticeStage1~2");
                            stageManager = null;
                            break;
                        case 921212://PR-2
                            PlayBGM("PracticeStage1~2");
                            stageManager = null;
                            break;
                        case 921213://PR-3
                            PlayBGM("PracticeStage3~4");
                            stageManager = null;
                            break;
                        case 921214://PR-4
                            PlayBGM("PracticeStage3~4");
                            stageManager = null;
                            break;
                        case 921215://PR-5
                            PlayBGM("PracticeStage5~6");
                            stageManager = null;
                            break;
                        case 921216://PR-6
                            PlayBGM("PracticeStage5~6");
                            stageManager = null;
                            break;
                        case 941201://TR-1
                            stageManager = null;
                            PlayBGM("ChallengeStage");
                            break;
                        case 941202://TR-2
                            PlayBGM("ChallengeStage");
                            stageManager = null;
                            break;
                    }
                
            }
           
            else if (currentSceneName == saveMainScene)
            {
                PlayBGM("MainSceneBGM");
            }
            
        }
        
    }
    public void Init()
    {
        PlayDataManager.Init();

        SetMasterVolume(PlayDataManager.data.MasterVolume);
        SetBGMVolume(PlayDataManager.data.BGMVolume);
        SetSEVolume(PlayDataManager.data.SEVolume);
        OnOffMasterVolume(PlayDataManager.data.IsMasterVolumMute);
        OnOffBGMVolume(PlayDataManager.data.IsBGMVolumMute);
        OnOffSEVolume(PlayDataManager.data.IsSEVolumMute);
    }

    public void OnOffMasterVolume(bool value)
    {
        if(value)
        {
            SetMasterVolume(PlayDataManager.data.MasterVolume * 2f);
        }
        else
        {
            SetMasterVolume(0.001f);
        }
        PlayDataManager.data.IsMasterVolumMute = value;
    }

    public void OnOffBGMVolume(bool value)
    {
        if (value)
        {
            SetBGMVolume(PlayDataManager.data.BGMVolume);
        }
        else
        {
            SetMasterVolume(0.001f);
        }
        PlayDataManager.data.IsBGMVolumMute = value;
    }

    public void OnOffSEVolume(bool value)
    {
        if (value)
        {
            SetSEVolume(PlayDataManager.data.SEVolume);
        }
        else
        {
            SetSEVolume(0.001f);
        }
        PlayDataManager.data.IsSEVolumMute = value;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("master", Mathf.Log10(volume) * 40);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("bgm", Mathf.Log10(volume) * 40);
    }

    public void SetSEVolume(float volume)
    {
        audioMixer.SetFloat("se", Mathf.Log10(volume) * 40);
    }

    public void SaveVolumes() // 창 닫는 버튼 누를 때 호출해주기
    {
        float value;

        audioMixer.GetFloat("master",out value);
        PlayDataManager.data.MasterVolume = value;

        audioMixer.GetFloat("bgm", out value);
        PlayDataManager.data.BGMVolume = value;

        audioMixer.GetFloat("se", out value);
        PlayDataManager.data.SEVolume = value;

        PlayDataManager.Save();
    }

    public void PlayerSFXAudio(string sound)
    {
        if(dicAudioClips.ContainsKey(sound))
        {
            SFXAudioSource.First().PlayOneShot(dicAudioClips[sound]);
        }
    }
    public void PlayBGM(string bgm)
    {
        foreach(var b in bgmAudioSource)//bgm여러개일 경우의 기반
        {
            b.clip = dicAudioClips[bgm];
            b.loop = true;
            b.Play();
            return;
        }
        
    }
}
