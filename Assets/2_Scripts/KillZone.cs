using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ��ü���� RiderController�� ������
        RiderController rider = collision.GetComponent<RiderController>();
        if (rider != null)
        {
            int score = rider.GetScore(); // RiderController���� ������ ������
            GameManager.Instance.GameStop(score); // ������ �����Ͽ� ���� ����
        }
    }
}
