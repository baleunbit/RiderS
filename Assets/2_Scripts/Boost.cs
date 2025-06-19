using UnityEngine;

public class Boost : MonoBehaviour
{
    [Header("Boost Settings")]
    public Sprite boostSprite; // �ν�Ʈ �� ��������Ʈ
    public float boostSpeed = 50f; // �ν�Ʈ �� �ӵ�
    public float boostDuration = 2f; // �ν�Ʈ ���� �ð�

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (boostSprite != null)
        {
            spriteRenderer.sprite = boostSprite; // �ν�Ʈ ��������Ʈ ����
        }
        else
        {
            Debug.LogWarning("Boost ��������Ʈ�� �������� �ʾҽ��ϴ�.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CarController carController = other.GetComponent<CarController>();
            if (carController != null)
            {
                carController.StartBoost(boostSpeed, boostDuration, boostSprite); // �ν�Ʈ ����
                Destroy(gameObject); // �ν�Ʈ ������Ʈ ����
            }
        }
    }
}
