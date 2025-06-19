using UnityEngine;

public class KillZone : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ��ü���� CarController�� ������
        CarController car = collision.GetComponent<CarController>();
        if (car != null)
        {
            car.TriggerCrashEffect(); // CrashEffect Ȱ��ȭ
            GameManager.Instance.GameStop(car.GetScore()); // ������ �����Ͽ� ���� ����
        }
    }
}
