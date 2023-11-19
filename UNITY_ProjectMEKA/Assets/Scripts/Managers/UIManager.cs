using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �̱���
public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    private TextMeshProUGUI fps;
    public GameObject gameoverUI;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
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

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }
}
