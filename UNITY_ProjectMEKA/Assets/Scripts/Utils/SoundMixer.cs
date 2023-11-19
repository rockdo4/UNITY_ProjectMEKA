using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class SoundMixer : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] public Slider m_MusicBGMSlider;
    [SerializeField] public Slider m_MusicSFXSlider;

    private void Start()
    {
        //m_MusicSFXSlider.value = 0.5f;
        //m_MusicBGMSlider.value = 0.5f;
        PlayDataManager.Init();
        var originData = PlayDataManager.data;
        if(m_MusicBGMSlider != null && m_MusicSFXSlider != null)
        {
            m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
            m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
            //m_MusicSFXSlider.value = originData.bgmVolume;
            //m_MusicBGMSlider.value = originData.effectVolume;
            SetMusicVolume(m_MusicBGMSlider.value);
            SetSFXVolume(m_MusicSFXSlider.value);
        }
        else
        {
            //SetMusicVolume(originData.bgmVolume);
            //SetSFXVolume(originData.effectVolume);
        }
    }

    public static SoundMixer instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundMixer>();
            }
            return m_instance;
        }
    }
    private static SoundMixer m_instance;
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //if(bgm != null && clip != null)
        //{
        //    bgm.PlayOneShot(clip);
        //}
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("bgm", Mathf.Log10(volume) * 40);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("effect", Mathf.Log10(volume) * 40);
    }

    public void SaveVolume()
    {
        var originData = PlayDataManager.data;
        //originData.bgmVolume = m_MusicBGMSlider.value;
        //originData.effectVolume = m_MusicSFXSlider.value;
        PlayDataManager.Save();
        //if(originData.bgmVolume != 0.5f)
    //    {
    //        Debug.Log("저장완료");
    //    }
    }
}