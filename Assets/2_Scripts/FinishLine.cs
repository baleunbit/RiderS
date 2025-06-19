using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1f; // 씬 재로드 딜레이
    [SerializeField] private ParticleSystem finishEffect; // 인스펙터에서 반드시 할당

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (finishEffect != null)
            {
                finishEffect.Play(); // 도착 시 이펙트 재생
            }

            Invoke(nameof(ReloadScene), reloadDelay); // 딜레이 후 씬 재로드
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(0); // 씬 인덱스 0으로 재로드
    }
}
