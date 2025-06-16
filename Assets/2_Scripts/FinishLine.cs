using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1;
    [SerializeField] private ParticleSystem finishEffect; // 인스펙터에서 반드시 할당

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            finishEffect.Play(); // null 체크 없이 바로 실행

            // currentTime 초기화
            GameManager.Instance?.GameRestart();

            // 메인 메뉴 패널 호출
            UIManager.Instance?.ShowGameOverPanel();

            // 일정 시간 후 씬 재로드
            Invoke(nameof(ReloadScene), reloadDelay);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}