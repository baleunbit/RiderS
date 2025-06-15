using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private float elapsedTime = 0f;
    private float fatestTime = float.MaxValue;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        UIManager.Instance.UpdateTimeText(FormatElapsedTime(elapsedTime));
    }

    public void GameStop()
    {
        Time.timeScale = 0f;

        if (elapsedTime < fatestTime)
        {
            fatestTime = elapsedTime;
        }
        UIManager.Instance.UpdateCurrentTimeText("Current Time : " + FormatElapsedTime(elapsedTime));
        UIManager.Instance.UpdateFastTimeText("Fastest Time : " + FormatElapsedTime(fatestTime));

        // 게임 오버 패널 활성화
        UIManager.Instance.ShowGameOverPanel();
    }

    public void GameRestart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); // 게임 씬 이름 사용
        elapsedTime = 0f;
        UIManager.Instance.HideGameOverPanel();
    }

    private string FormatElapsedTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int milliseconds = (int)((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
