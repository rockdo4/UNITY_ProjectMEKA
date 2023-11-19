using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ΩÃ±€≈Ê
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private TextMeshProUGUI fps;
    public GameObject gameoverUI;

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
    }

    public void Update()
    {
        UpdateFps();
    }

    public void UpdateFps()
    {
        if (fps == null)
            return;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("FPS : ");
        stringBuilder.Append(1f / Time.deltaTime);
        fps.text = stringBuilder.ToString();
    }

    // ∞‘¿” ø¿πˆ UI »∞º∫»≠
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }
}
