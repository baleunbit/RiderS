using UnityEngine;

public class KillZone : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체에서 CarController를 가져옴
        CarController car = collision.GetComponent<CarController>();
        if (car != null)
        {
            car.TriggerCrashEffect(); // CrashEffect 활성화
            GameManager.Instance.GameStop(car.GetScore()); // 점수를 전달하여 게임 종료
        }
    }
}
