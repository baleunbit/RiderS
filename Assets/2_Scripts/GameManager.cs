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
        Time.timeScale = 1f; // ���� �� �׻� �ð� ����ȭ
    }

    void Update()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is NULL. UI ������Ʈ ����.");
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

        // Fastest Time ������Ʈ
        float fastestTime = PlayerPrefs.GetFloat("FastestTime", float.MaxValue);
        if (elapsedTime < fastestTime) // �ִ� ��� ����
        {
            PlayerPrefs.SetFloat("FastestTime", elapsedTime);
            PlayerPrefs.Save();
            Debug.Log($"[GameManager] Fastest Time ������Ʈ: {elapsedTime}");
        }

        // UI ������Ʈ
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

        // UI �ʱ�ȭ
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
