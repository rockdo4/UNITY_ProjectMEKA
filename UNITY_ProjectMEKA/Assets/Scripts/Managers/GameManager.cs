using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// �̱���
public class GameManager : MonoBehaviour
{
    public GameObject player { get; private set; }

    public static GameManager instance = null;

    public bool IsGameover { get; private set; }

    private void Awake()
    {
        PlayDataManager.Init();
        Init();

        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
    }

    public void Update()
    {
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetPlayer(GameObject pl)
    {
        if (pl != null)
        {
            player = pl;
        }
        else
        {
            Debug.Log("player == null");
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        IsGameover = true;
        // ���� ���� UI Ȱ��ȭ
        Time.timeScale = 0f;
        UIManager.instance.SetActiveGameoverUI(true);
        SoundManager.instance.PlayUiWindowAudio();
    }

    public void Init()
    {
        IsGameover = false;
    }
}
