using UnityEngine;
using UnityEngine.SceneManagement;

// 한글 주석 테스트
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool IsLoaded = false;
    private void Awake()
    {
        PlayDataManager.Init();

        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayDataManager.Save();
		}
	}

    public void SaveExecution()
    {
        PlayDataManager.Save();
    }

	public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
