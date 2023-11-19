using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// 싱글톤
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

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        IsGameover = true;
        // 게임 오버 UI 활성화
        Time.timeScale = 0f;
        UIManager.instance.SetActiveGameoverUI(true);
        SoundManager.instance.PlayUiWindowAudio();
    }

    public void Init()
    {
        IsGameover = false;
    }
}
