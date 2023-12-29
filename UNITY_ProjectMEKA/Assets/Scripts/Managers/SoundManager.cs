using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// 싱글톤
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    public AudioMixer audioMixer;
    public List<AudioSource> bgmAudioSource;
    public List<AudioSource> SFXAudioSource;
    private string currentSceneName;
    private string saveSceneName;
    [System.Serializable]
    public class AudioClipList
    {
        public string name;
        public AudioClip clip;
    }
    public List<AudioClipList> audioClips;
    private Dictionary<string, AudioClip> dicAudioClips;
    private void Awake()
    {
        if (instance != null && instance != this)//한개만 존재하도록 수정
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
        saveSceneName = currentSceneName;
        PlayBGM("MainSceneBGM");

    }
    private void Update()
    {
        if(currentSceneName != SceneManager.GetActiveScene().name) 
        {
            currentSceneName = SceneManager.GetActiveScene().name;
            if(currentSceneName == saveSceneName)
            {
                PlayBGM("MainSceneBGM");
            }
            else
            {
                PlayBGM("InGameBGM");

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
            SetMasterVolume(PlayDataManager.data.MasterVolume);
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
        
        //bgmAudioSource.First().clip = dicAudioClips[bgm];
        //bgmAudioSource.First().loop = true;
        //bgmAudioSource.First().Play();
    }
}
