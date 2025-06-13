using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public Text bestScoreText;
    public Image carPreviewImage;
    public Button playButton;
    public Sprite[] carSprites;

    void Start()
    {
        // 최고 점수 표시
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = bestScore.ToString("00");

        // 현재 차 프리뷰 표시
        int carIndex = PlayerPrefs.GetInt("SelectedCar", 0);
        carPreviewImage.sprite = carSprites[carIndex];

        playButton.onClick.AddListener(OnPlayClicked);
    }

    void OnPlayClicked()
    {
        mainMenuPanel.SetActive(false);
        // 게임 오브젝트/게임 UI 활성화 등 추가
        // 또는 씬 전환: SceneManager.LoadScene("GameScene");
    }
}
