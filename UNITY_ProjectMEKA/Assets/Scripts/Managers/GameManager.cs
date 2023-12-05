using UnityEngine;
using UnityEngine.SceneManagement;

// �̱���
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private void Awake()
    {
        PlayDataManager.Init();

        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
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

	public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
