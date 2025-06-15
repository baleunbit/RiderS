using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SurfaceEffector2D surfaceEffector2D;

    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isGrounded = false;
    private float lastGroundedAngle = 0f;
    private float rotationAccum = 0f;
    private float scoreTimer = 0f;

    public int BestScore = 0;
    public int score = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        LimitAngularVelocity();
    }
    public float maxAngularVelocity = 100f; // 최고 회전 가속도 제한
    private void LimitAngularVelocity()
    {
        if (rb == null) return;
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
    }


    void Update()
    {
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        HandleRotationScoring();
        HandleSpeedScoring();

        UpdateUI();
        CheckGameOverCondition();
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void HandleRotationScoring()
    {
        if (!isGrounded)
        {
            float rotateInput = Input.GetKey(KeyCode.LeftArrow) ? 1f :
                                Input.GetKey(KeyCode.RightArrow) ? -1f : 0f;
            rb.AddTorque(rotateInput * 3f, ForceMode2D.Impulse);

            float deltaAngle = Mathf.DeltaAngle(lastGroundedAngle, transform.eulerAngles.z);
            rotationAccum += Mathf.Abs(deltaAngle);
            lastGroundedAngle = transform.eulerAngles.z;

            if (rotationAccum >= 360f)
            {
                score += 1;
                rotationAccum -= 360f;
                UIManager.Instance?.UpdateScoreText($"{score:00}");
            }
        }
    }

    private void HandleSpeedScoring()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1f)
            {
                score += 1;
                scoreTimer = 0f;
                UIManager.Instance?.UpdateScoreText($"{score:00}");
            }
        }
        else
        {
            scoreTimer = 0f;
        }

        if (score > BestScore)
        {
            BestScore = score;
            PlayerPrefs.SetInt("BestScore", BestScore);
            PlayerPrefs.Save();
        }
    }

    private void UpdateUI()
    {
        UIManager.Instance?.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");

        if (surfaceEffector2D != null)
        {
            surfaceEffector2D.speed = Input.GetKey(KeyCode.W) ? 10f : 0f;
            UIManager.Instance?.UpdateSurfaceText($"Surface Speed : {surfaceEffector2D.speed:F1}");
        }
    }

    private void CheckGameOverCondition()
    {
        if (isGrounded)
        {
            float angleZ = Mathf.Abs(transform.eulerAngles.z);
            if (angleZ > 90f && angleZ < 270f)
            {
                GameManager.Instance.GameStop();
            }
        }
    }

    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastGroundedAngle = transform.eulerAngles.z;
        rotationAccum = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            surfaceEffector2D = effector;
        }
    }
}
