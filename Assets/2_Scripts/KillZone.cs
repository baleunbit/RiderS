using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ��ü���� RiderController�� ������
        CarController rider = collision.GetComponent<CarController>();
        if (rider != null)
        {
            int score = rider.GetScore(); // RiderController���� ������ ������
            GameManager.Instance.GameStop(score); // ������ �����Ͽ� ���� ����
        }
    }
}
