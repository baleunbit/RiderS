using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Text BestScore;

    void Start()
    {
        Time.timeScale = 1f; // ����ȭ�� ���� �� �ð� ����

        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        int best = PlayerPrefs.GetInt("BestScore", 0);
        BestScore.text = $"Best Score : {best}";
    }
}
