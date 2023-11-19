using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public Material material { get; set; }

    public float fadeTime = 0.5f;
    private float accumTime = 0f;
    private Coroutine fadeCor;

    public void StartFadeIn()
    {
        if(fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        accumTime = 0f;
        Color newColor = material.color;
        while(accumTime < fadeTime)
        {
            newColor.a = Mathf.Lerp(0f, 0.35f, accumTime / fadeTime);
            material.color = newColor;
            yield return null;
            accumTime += Time.deltaTime;
        }
        newColor.a = 0.35f;
        material.color = newColor;
    }

    private IEnumerator FadeOut()
    {
        accumTime = 0f;
        Color newColor = material.color;
        while ( accumTime < fadeTime)
        {
            newColor.a = Mathf.Lerp(0.35f, 0f, accumTime / fadeTime);
            material.color = newColor;
            yield return null;
            accumTime += Time.deltaTime;
        }
        newColor.a = 0f;
        material.color = newColor;
    }
}
