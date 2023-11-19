using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    SoundEffectSingle,
    SoundEffectLoop,
    BGM,
    UI
}

// ΩÃ±€≈Ê
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioClip uiButton;
    public AudioClip uiUpgrade;
    public AudioClip uiWindow;
    public AudioClip play;
    public bool isUIWindow;
    private static AudioSource[] audioSource;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        audioSource = GetComponents<AudioSource>();
    }

    private void Start()
    {
    }

    public void PlaySingleAudio(AudioClip clip)
    {
        audioSource[(int)AudioType.SoundEffectSingle].PlayOneShot(clip);
    }

    public void PlayLoopAudio(AudioClip clip)
    {
        audioSource[(int)AudioType.SoundEffectLoop].clip = clip;
        audioSource[(int)AudioType.SoundEffectLoop].loop = true;
        audioSource[(int)AudioType.SoundEffectLoop].Play();
    }

    public void StopLoopAudio()
    {
        audioSource[(int)AudioType.SoundEffectLoop].Stop();
    }

    public void PlayBgm()
    {
        audioSource[(int)AudioType.BGM].clip = play;
        audioSource[(int)AudioType.BGM].loop = true;
        audioSource[(int)AudioType.BGM].Play();
    }

    public void PlayUiWindowAudio()
    {
        audioSource[(int)AudioType.SoundEffectSingle].Pause();
        audioSource[(int)AudioType.SoundEffectLoop].Pause();
        audioSource[(int)AudioType.BGM].Pause();

        audioSource[(int)AudioType.UI].clip = uiWindow;
        audioSource[(int)AudioType.UI].loop = true;
        audioSource[(int)AudioType.UI].Play();
    }

    public void CloseUiWindowAudio()
    {
        audioSource[(int)AudioType.SoundEffectSingle].UnPause();
        audioSource[(int)AudioType.SoundEffectLoop].UnPause();
        audioSource[(int)AudioType.BGM].UnPause();

        audioSource[(int)AudioType.UI].Stop();
    }

    public void PlayUIButtonAudio()
    {
        audioSource[(int)AudioType.SoundEffectSingle].PlayOneShot(uiButton);
    }

    public void PlayUIUpgradeAudio()
    {
        audioSource[(int)AudioType.SoundEffectSingle].PlayOneShot(uiUpgrade);
    }

    public void StopSoundEffect()
    {
        audioSource[(int)AudioType.SoundEffectSingle].Stop();
    }
}
