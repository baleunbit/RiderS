using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private float elapsedTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;    
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f; // 시작 시 항상 시간 정상화
    }

    void Update()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is NULL. UI 업데이트 실패.");
            return;
        }

        UIManager.Instance.UpdateTimeText(FormatElapsedTime(elapsedTime));
        elapsedTime += Time.deltaTime;
    }

    public void GameRestart()
    {
        elapsedTime = 0f;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    public void GameStop(int score)
    {
        Time.timeScale = 0f;

        // Fastest Time 업데이트
        float fastestTime = PlayerPrefs.GetFloat("FastestTime", float.MaxValue);
        if (elapsedTime < fastestTime) // 최단 기록 저장
        {
            PlayerPrefs.SetFloat("FastestTime", elapsedTime);
            PlayerPrefs.Save();
            Debug.Log($"[GameManager] Fastest Time 업데이트: {elapsedTime}");
        }

        // UI 업데이트
        UIManager.Instance?.UpdateCurrentTimeText($"Current Time : {FormatElapsedTime(elapsedTime)}");
        UIManager.Instance?.UpdateFastTimeText($"Fastest Time : {FormatElapsedTime(PlayerPrefs.GetFloat("FastestTime", float.MaxValue))}");
        UIManager.Instance?.UpdateScoreText($"Best Score : {PlayerPrefs.GetInt("BestScore", 0)}");
        UIManager.Instance?.ShowGameOverPanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Time.timeScale = 1f;
        elapsedTime = 0f;

        // UI 초기화
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideGameOverPanel();
            UIManager.Instance.UpdateCurrentTimeText($"Current Time : {FormatElapsedTime(elapsedTime)}");
            UIManager.Instance.UpdateFastTimeText($"Fastest Time : {FormatElapsedTime(PlayerPrefs.GetFloat("FastestTime", 0f))}");
        }
    }

    private string FormatElapsedTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int milliseconds = (int)((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
