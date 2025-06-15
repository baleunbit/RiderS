using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private float elapsedTime = 0f;
    private float fastestTime = float.MaxValue;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
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
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateTimeText(FormatElapsedTime(elapsedTime));
            }

            elapsedTime += Time.deltaTime;
    }

    public void GameRestart()
    {
        elapsedTime = 0f;
        fastestTime = float.MaxValue;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    public void GameStop()
    {
        Time.timeScale = 0f;

        if (elapsedTime < fastestTime)
        {
            fastestTime = elapsedTime;
        }

        UIManager.Instance?.UpdateCurrentTimeText("Current Time : " + FormatElapsedTime(elapsedTime));
        UIManager.Instance?.UpdateFastTimeText("Fastest Time : " + FormatElapsedTime(fastestTime));
        UIManager.Instance?.ShowGameOverPanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
    Time.timeScale = 1f;
    elapsedTime = 0f;

    // 여기서 UIManager 재참조 보장
    if (UIManager.Instance != null)
    {
        UIManager.Instance.HideGameOverPanel();
        UIManager.Instance.UpdateCurrentTimeText("Current Time : " + FormatElapsedTime(elapsedTime));
        UIManager.Instance.UpdateFastTimeText("Fastest Time : " + FormatElapsedTime(fastestTime));
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
