using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체에서 점수를 가져옴 (예: ScoreProvider 컴포넌트)
        ScoreProvider scoreProvider = collision.GetComponent<ScoreProvider>();
        int score = scoreProvider != null ? scoreProvider.GetScore() : 0;

        // GameManager에 점수 전달
        GameManager.Instance.GameStop(score);
    }
}
