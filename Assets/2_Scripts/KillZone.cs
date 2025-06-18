using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ��ü���� ������ ������ (��: ScoreProvider ������Ʈ)
        ScoreProvider scoreProvider = collision.GetComponent<ScoreProvider>();
        int score = scoreProvider != null ? scoreProvider.GetScore() : 0;

        // GameManager�� ���� ����
        GameManager.Instance.GameStop(score);
    }
}
