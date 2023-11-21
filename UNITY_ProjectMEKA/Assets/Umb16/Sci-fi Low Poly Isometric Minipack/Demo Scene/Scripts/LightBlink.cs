using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlink : MonoBehaviour
{
    public float value;
    public Light[] lights;
    public Renderer[] renderers;
    public AudioClip[] audioClips;
    float[] lightsIntensity;
    Color[] emissionColors;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        lightsIntensity = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
                lightsIntensity[i] = lights[i].intensity;
        }
        emissionColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                emissionColors[i] = renderers[i].material.GetColor("_EmissionColor");
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
                lights[i].intensity = lightsIntensity[i] * value;
        }
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                renderers[i].material.SetColor("_EmissionColor", emissionColors[i] * value);
        }
    }

    public void PlaySound(int index)
    {
        if (audioSource != null && audioClips.Length > index && index >= 0)
            audioSource.PlayOneShot(audioClips[index]);

    }
}
