using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Text BestScore;

    void Start()
    {
        Time.timeScale = 1f;

        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        int best = PlayerPrefs.GetInt("BestScore", 0);
        BestScore.text = $"Best Score : {best}";

        float fastestTime = PlayerPrefs.GetFloat("FastestTime", float.MaxValue);
        if (fastestTime != float.MaxValue)
        {
            BestScore.text += $"\nFastest Time : {FormatElapsedTime(fastestTime)}";
        }
        else
        {
            BestScore.text += "\nFastest Time : N/A";
        }
    }

    private string FormatElapsedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
