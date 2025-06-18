using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체에서 RiderController를 가져옴
        CarController rider = collision.GetComponent<CarController>();
        if (rider != null)
        {
            int score = rider.GetScore(); // RiderController에서 점수를 가져옴
            GameManager.Instance.GameStop(score); // 점수를 전달하여 게임 종료
        }
    }
}
