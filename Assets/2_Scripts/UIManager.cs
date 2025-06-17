using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text timeText;
    public Text surfaceSpeedText;
    public Text carSpeedText;
    public Text currentTimeText;
    public Text fastTimeText;
    public Text ScoreText;
    public GameObject gameOverPanel;



    void Start()
    {
        ReconnectUIElements();
    }

    public void ReconnectUIElements()
    {
        if (timeText == null)
            timeText = GameObject.Find("TimeText")?.GetComponent<Text>();
        if (surfaceSpeedText == null)
            surfaceSpeedText = GameObject.Find("SurfaceSpeedText")?.GetComponent<Text>();
        if (carSpeedText == null)
            carSpeedText = GameObject.Find("CarSpeedText")?.GetComponent<Text>();
        if (currentTimeText == null)
            currentTimeText = GameObject.Find("CurrentTimeText")?.GetComponent<Text>();
        if (fastTimeText == null)
            fastTimeText = GameObject.Find("FastestTimeText")?.GetComponent<Text>();
        if (ScoreText == null)
            ScoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();
        if (gameOverPanel == null)
            gameOverPanel = GameObject.Find("GameOverPanel");
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 모든 Text 컴포넌트에 대해 Find 및 로그
        TryAssignIfNull(ref timeText, "TimeText");
        TryAssignIfNull(ref surfaceSpeedText, "SurfaceSpeedText");
        TryAssignIfNull(ref carSpeedText, "CarSpeedText");
        TryAssignIfNull(ref currentTimeText, "CurrentTimeText");
        TryAssignIfNull(ref fastTimeText, "FastTimeText");
        TryAssignIfNull(ref ScoreText, "ScoreText");
        TryAssignIfNull(ref gameOverPanel, "GameOverPanel");
    }

    void TryAssignIfNull<T>(ref T component, string name) where T : Component
    {
        if (component == null)
        {
            var obj = GameObject.Find(name);
            if (obj != null)
            {
                component = obj.GetComponent<T>();
                Debug.Log($"[UIManager] '{name}' 자동 할당 완료.");
            }
            else
            {
                Debug.LogWarning($"[UIManager] '{name}' GameObject 찾을 수 없음.");
            }
        }
    }

    void TryAssignIfNull(ref GameObject obj, string name)
    {
        if (obj == null)
        {
            var found = GameObject.Find(name);
            if (found != null)
            {
                obj = found;
                Debug.Log($"[UIManager] '{name}' GameObject 자동 할당 완료.");
            }
            else
            {
                Debug.LogWarning($"[UIManager] '{name}' GameObject 찾을 수 없음.");
            }
        }
    }

    public void GameRestart()
    {
        GameManager.Instance?.GameRestart();
    }

    public void UpdateTimeText(string time)
    {
        if (timeText == null) { LogNull("timeText"); return; }
        timeText.text = time;
    }

    public void UpdateSurfaceText(string surface)
    {
        if (surfaceSpeedText == null) { LogNull("surfaceSpeedText"); return; }
        surfaceSpeedText.text = surface;
    }

    public void UpdateCarSpeedText(string speed)
    {
        if (carSpeedText == null) { LogNull("carSpeedText"); return; }
        carSpeedText.text = speed;
    }

    public void UpdateCurrentTimeText(string time)
    {
        if (currentTimeText == null) { LogNull("currentTimeText"); return; }
        currentTimeText.text = time;
    }

    public void UpdateFastTimeText(string time)
    {
        if (fastTimeText == null) { LogNull("fastTimeText"); return; }
        fastTimeText.text = time;
    }

    public void UpdateScoreText(string text)
    {
        if (ScoreText == null) { LogNull("ScoreText"); return; }
        ScoreText.text = text;
    }

    public void ShowGameOverPanel()
    {
        if (gameOverPanel == null) { LogNull("gameOverPanel"); return; }
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideGameOverPanel()
    {
        if (gameOverPanel == null) { LogNull("gameOverPanel"); return; }
        gameOverPanel.SetActive(false);
    }

    public void OnQuitClicked()
    {
        // currentTime 초기화
        GameManager.Instance?.GameRestart();

        // 메인 메뉴 씬으로 이동
        Time.timeScale = 1f;
        SceneManager.LoadScene("1_Scenes/MainMenuScene");
    }

    private void LogNull(string name)
    {
        Debug.LogWarning($"[UIManager] {name} is NULL. 연결 또는 오브젝트 확인 필요.");
    }
}
