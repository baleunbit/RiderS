using UnityEngine;
using UnityEngine.UI;

public class RiderController : MonoBehaviour
{
    [Header("Move Settings")]
    public float baseSpeed = 5f;
    public float boostSpeed = 12f;
    public float acceleration = 2f;

    [Header("Jump & Rotation")]
    public float jumpForce = 7f;
    public float airTorque = 200f;

    [Header("Coin & Boost")]
    public int coinCount = 0;
    public int maxCoins = 100;
    public Text coinText; // UI �ؽ�Ʈ ���� �ʿ�
    public Sprite normalSprite;
    public Sprite boostSprite;
    public float boostDuration = 5f;

    private bool isGrounded = true;
    private bool isBoosting = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float currentSpeed;
    private float boostTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentSpeed = baseSpeed;
        UpdateCoinUI();
    }

    void Update()
    {
        // ����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // �ν�Ʈ �ߵ�
        if (Input.GetKeyDown(KeyCode.I) && coinCount >= maxCoins && !isBoosting)
        {
            StartBoost();
        }
    }

    void FixedUpdate()
    {
        // �׻� �⺻ �ӵ��� ����
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        // ���� ȸ�� ����
        if (!isGrounded)
        {
            if (Input.GetKey(KeyCode.RightArrow))
                rb.AddTorque(-airTorque * Time.fixedDeltaTime, ForceMode2D.Force);
            else if (Input.GetKey(KeyCode.LeftArrow))
                rb.AddTorque(airTorque * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        // �ν�Ʈ ���� �ð� üũ
        if (isBoosting)
        {
            boostTimer -= Time.fixedDeltaTime;
            if (boostTimer <= 0f)
            {
                StopBoost();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin") && coinCount < maxCoins)
        {
            coinCount++;
            UpdateCoinUI();
            Destroy(col.gameObject);
        }
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = coinCount.ToString() + " / " + maxCoins.ToString();
        }
    }

    void StartBoost()
    {
        isBoosting = true;
        coinCount = 0;
        UpdateCoinUI();
        sr.sprite = boostSprite;
        currentSpeed = boostSpeed;
        boostTimer = boostDuration;
    }

    void StopBoost()
    {
        isBoosting = false;
        currentSpeed = baseSpeed;
        sr.sprite = normalSprite;
    }
}
