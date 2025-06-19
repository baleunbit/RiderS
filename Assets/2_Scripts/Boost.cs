using UnityEngine;

public class Boost : MonoBehaviour
{
    [Header("Boost Settings")]
    public Sprite boostSprite; // 부스트 시 스프라이트
    public float boostSpeed = 50f; // 부스트 시 속도
    public float boostDuration = 2f; // 부스트 지속 시간

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (boostSprite != null)
        {
            spriteRenderer.sprite = boostSprite; // 부스트 스프라이트 설정
        }
        else
        {
            Debug.LogWarning("Boost 스프라이트가 설정되지 않았습니다.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CarController carController = other.GetComponent<CarController>();
            if (carController != null)
            {
                carController.StartBoost(boostSpeed, boostDuration, boostSprite); // 부스트 시작
                Destroy(gameObject); // 부스트 오브젝트 제거
            }
        }
    }
}
