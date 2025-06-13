using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text timeText;
    public Text surfaceSpeedText;
    public Text carSpeedText;
    public Text currentTimeText;
    public Text fastTimeText;
    public Text ScoreText;
    public GameObject panel;
    public GameObject mainMenuPanel;
    public GameObject gameUIPanel;
    public GameObject gameOverPanel;

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

    public void UpdateTimeText(string time)
    {
        timeText.text = time;
    }

    public void UpdateSurfaceText(string speed)
    {
        surfaceSpeedText.text = speed;
    }

    public void UpdateCarSpeedText(string speed)
    {
        carSpeedText.text = speed;
    }

    public void UpdateCarSpeedText(Rigidbody rb)
    {
        carSpeedText.text = $"Car Speed : {rb.linearVelocity.magnitude:F1}";
    }

    public void UpdateCurrentTimeText(string time)
    {
        currentTimeText.text = time;
    }

    public void UpdateFastTimeText(string time)
    {
        fastTimeText.text = time;
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }

    public void GameRestart()
    {
        GameManager.Instance.GameRestart();
    }

    public void UpdateScoreText(string text)
    {
        ScoreText.text = text;
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gameUIPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameUI()
    {
        mainMenuPanel.SetActive(false);
        gameUIPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        mainMenuPanel.SetActive(false);
        gameUIPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
}
