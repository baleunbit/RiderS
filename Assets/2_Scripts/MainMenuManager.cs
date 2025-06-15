using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Text BestScore;

    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        // PlayerPrefs에서 불러오기
        int best = PlayerPrefs.GetInt("BestScore", 0);
        BestScore.text = $"Best Score : {best}";
    }
}
