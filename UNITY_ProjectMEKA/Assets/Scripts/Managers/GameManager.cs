using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// ΩÃ±€≈Ê
public class GameManager : MonoBehaviour
{
    public GameObject player { get; private set; }

    public static GameManager instance { get; private set; }

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

    // ∞‘¿” ø¿πˆ √≥∏Æ
    public void EndGame()
    {
    }

    public void Init()
    {
        IsGameover = false;
    }
}
