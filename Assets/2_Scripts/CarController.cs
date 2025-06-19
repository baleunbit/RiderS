using System;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("Move Settings")]
    public float baseSpeed = 10f;
    public float boostSpeed = 50f;
    public float acceleration = 5f;
    public float deceleration = 5f;

    [Header("Jump & Rotation")]
    public float jumpForce = 7f;
    public float airTorque = 300f;

    [Header("Coin & Boost")]
    public int coinCount = 0;
    public int maxCoins = 100;
    public Text coinText;
    public Sprite normalSprite;
    public Sprite boostSprite;
    public float boostDuration = 2f; // 부스트 지속 시간

    [Header("Crash Effect")]
    public GameObject crashEffectPrefab; // CrashEffect 프리팹 참조
    private GameObject crashEffectInstance;

    [Header("Acceleration Sound")]
    [SerializeField] private AudioSource accelerationSound; // 가속 사운드 AudioSource
    [SerializeField] private float minPitch = 1f; // 최소 Pitch 값
    [SerializeField] private float maxPitch = 2.0f; // 최대 Pitch 값

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

        if (accelerationSound != null)
        {
            accelerationSound.loop = true; // 가속 사운드 반복 재생
            accelerationSound.Play(); // 사운드 시작
        }
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

        UpdateAccelerationSound(); // 가속 사운드 업데이트
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

    void UpdateAccelerationSound()
    {
        if (accelerationSound != null && isGrounded)
        {
            // 속도에 따라 Pitch 조정
            float normalizedSpeed = Mathf.InverseLerp(baseSpeed, boostSpeed, currentSpeed);
            accelerationSound.pitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
        }
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

        if (col.collider.CompareTag("Obstacle")) // 장애물 충돌 시
        {
            TriggerCrashEffect(); // CrashEffect 활성화
            SaveFastestTime(); // Fastest Time 저장
            GameManager.Instance?.GameStop(score); // 점수를 전달하여 게임 종료
        }
    }

    public void TriggerCrashEffect()
    {
        if (crashEffectPrefab != null)
        {
            // CrashEffect 생성
            crashEffectInstance = Instantiate(crashEffectPrefab, transform.position, Quaternion.identity);
            Destroy(crashEffectInstance, 2f); // 2초 후 효과 제거
        }
        else
        {
            Debug.LogWarning("CrashEffectPrefab이 설정되지 않았습니다.");
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
        }

        if (col.CompareTag("Boost")) // Boost 태그 처리
        {
            StartBoost(); // Boost 효과 시작
            Invoke(nameof(StopBoost), boostDuration); // 부스트 지속 시간 후 종료
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
        sr.sprite = boostSprite; // Boost 스프라이트로 변경
        currentSpeed = boostSpeed; // Boost 속도로 변경
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

        // Fastest Time 업데이트 (최단 기록 저장)
        if (currentTime < fastestTime) // 현재 시간이 더 짧을 경우 업데이트
        {
            PlayerPrefs.SetFloat("FastestTime", currentTime);
            PlayerPrefs.Save();
            Debug.Log($"[CarController] Fastest Time 업데이트: {currentTime}");
        }
    }

    private string FormatElapsedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }

    public int GetScore()
    {
        return score;
    }

    internal void StartBoost(float boostSpeed, float boostDuration, Sprite boostSprite)
    {
        throw new NotImplementedException();
    }
}
