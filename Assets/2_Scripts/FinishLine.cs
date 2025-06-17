using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float delayToMainMenu = 2f; // 메인 메뉴로 전환 전 딜레이
    [SerializeField] private ParticleSystem finishEffect; // 파티클 이펙트 (인스펙터에서 연결)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (finishEffect != null)
            {
                finishEffect.Play(); // 도착 시 이펙트 재생
            }

            Time.timeScale = 0f; // 게임 일시정지
            UIManager.Instance?.ShowGameOverPanel(); // 게임 오버 패널 즉시 표시

            Invoke(nameof(GoToMainMenu), delayToMainMenu); // 2초 후 메인 메뉴 이동
        }
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // 시간 다시 정상화
        SceneManager.LoadScene("1_Scenes/MainMenuScene"); // 메인 메뉴 씬 로드
    }
}
