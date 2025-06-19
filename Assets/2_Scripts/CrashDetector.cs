using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1f; // 씬 재로드 딜레이
    [SerializeField] private ParticleSystem crashEffect; // 크래시 이펙트 (인스펙터에서 연결)
    [SerializeField] private AudioSource crashSound; // 크래시 사운드 (인스펙터에서 연결)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (crashEffect != null)
            {
                crashEffect.Play(); // 크래시 이펙트 재생
            }

            if (crashSound != null)
            {
                crashSound.Play(); // 크래시 사운드 재생
            }

            UIManager.Instance?.ShowGameOverPanel(); // 게임 오버 패널 표시
            Invoke(nameof(ReloadScene), reloadDelay); // 딜레이 후 씬 재로드
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(0); // 씬 인덱스 0으로 재로드
    }
}