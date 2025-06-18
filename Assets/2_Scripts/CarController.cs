using UnityEngine;
using UnityEngine.UI;

public class RiderController : MonoBehaviour
{
    [Header("Move Settings")]
    public float baseSpeed = 10f;
    public float boostSpeed = 30f;
    public float acceleration = 5f;
    public float deceleration = 5f;

    [Header("Jump & Rotation")]
    public float jumpForce = 7f;
    public float airTorque = 200f;

    [Header("Coin & Boost")]
    public int coinCount = 0;
    public int maxCoins = 100;
    public Text coinText;
    public Sprite normalSprite;
    public Sprite boostSprite;
    public float boostDuration = 5f;

    private bool isGrounded = true;
    private bool isBoosting = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float currentSpeed;
    private float boostTimer = 0f;
    private float elapsedTime = 0f;

    private float scoreTimer = 0f;
    private int score = 0;
    private float lastZRotation = 0f;
    private float rotationAccum = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentSpeed = baseSpeed;
        lastZRotation = transform.eulerAngles.z;
        UpdateCoinUI();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        UIManager.Instance?.UpdateTimeText(FormatElapsedTime(elapsedTime));

        HandleBoostInput();
        HandleScoreBySpeed();
        HandleRotationScore();
        HandleJumpInput(); // 점프 입력 처리 추가
        UpdateUI();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleAirRotation();
        HandleBoostTimer();
    }

    void HandleMovement()
    {
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
            horizontal = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow))
            horizontal = -1f;

        float targetSpeed = baseSpeed;

        if (isBoosting)
        {
            targetSpeed = boostSpeed;
        }
        else
        {
            if (horizontal > 0)
                targetSpeed = boostSpeed;
            else if (horizontal < 0)
                targetSpeed = 0f;
            else
                targetSpeed = baseSpeed;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed,
            (targetSpeed > currentSpeed ? acceleration : deceleration) * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    void HandleAirRotation()
    {
        if (!isGrounded)
        {
            if (Input.GetKey(KeyCode.RightArrow))
                rb.AddTorque(-airTorque * Time.fixedDeltaTime, ForceMode2D.Force);
            else if (Input.GetKey(KeyCode.LeftArrow))
                rb.AddTorque(airTorque * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }

    void HandleBoostInput()
    {
        if (Input.GetKeyDown(KeyCode.I) && coinCount >= maxCoins && !isBoosting)
        {
            StartBoost();
        }
    }

    void HandleBoostTimer()
    {
        if (isBoosting)
        {
            boostTimer -= Time.fixedDeltaTime;
            if (boostTimer <= 0f)
            {
                StopBoost();
            }
        }
    }

    void HandleScoreBySpeed()
    {
        if (rb.linearVelocity.magnitude > 2f)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 2f)
            {
                AddScore(1);
                scoreTimer = 0f;
            }
        }
        else
        {
            scoreTimer = 0f;
        }
    }

    void HandleRotationScore()
    {
        float currentZ = transform.eulerAngles.z;
        float deltaZ = Mathf.DeltaAngle(lastZRotation, currentZ);
        rotationAccum += Mathf.Abs(deltaZ);
        lastZRotation = currentZ;

        if (rotationAccum >= 360f)
        {
            AddScore(1);
            rotationAccum = 0f;
        }
    }

    void HandleJumpInput()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // Space 키로 점프
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 점프 힘 추가
        }
    }

    void AddScore(int amount)
    {
        score += amount;
        UIManager.Instance?.UpdateScoreText(score.ToString("00"));
    }

    void UpdateUI()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is NULL. UI 업데이트 실패.");
            return;
        }

        UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");
        UIManager.Instance.UpdateSurfaceText($"Surface Speed : {currentSpeed:F1}");
        UIManager.Instance.UpdateScoreText(score.ToString("00"));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
            isGrounded = true;

        if (col.collider.CompareTag("Obstacle")) // Obstacle 태그 처리
        {
            SaveFastestTime(); // Fastest Time 저장
            UIManager.Instance?.ShowGameOverPanel(); // 게임 오버 패널 활성화
        }
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
            coinCount += 10; // 코인 하나를 먹으면 10개 증가
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

    void SaveFastestTime()
    {
        float currentTime = elapsedTime;
        float fastestTime = PlayerPrefs.GetFloat("FastestTime", float.MaxValue);

        // Fastest Time 업데이트
        if (currentTime > fastestTime) // 가장 오래 플레이한 시간 저장
        {
            PlayerPrefs.SetFloat("FastestTime", currentTime);
            PlayerPrefs.Save();
        }

        // UI 업데이트
        UIManager.Instance?.UpdateCurrentTimeText($"Current Time : {FormatElapsedTime(currentTime)}");
        UIManager.Instance?.UpdateFastTimeText($"Fastest Time : {FormatElapsedTime(PlayerPrefs.GetFloat("FastestTime", float.MaxValue))}");
    }

    private string FormatElapsedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
