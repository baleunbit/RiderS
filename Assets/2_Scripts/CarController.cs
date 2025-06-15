using UnityEngine;

public class CarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool onGround = false;
    public static CarController Instance { get; private set; }

    public float jumpForce = 7f;
    public int BestScore = 0;
    public int score = 0;
    private float lastGroundedAngle = 0f;
    private float rotationAccum = 0f;
    private float scoreTimer = 0f;

    private EdgeCollider2D carEdgeCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        carEdgeCollider = GetComponent<EdgeCollider2D>(); // Car의 EdgeCollider2D 가져오기
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            Debug.Log("SurfaceEffector2D 발견!");
            onGround = true;
            surfaceEffector2D = effector;
        }
        else
        {
            Debug.LogWarning("SurfaceEffector2D가 없습니다!");
        }
    }

    private void Update()
    {
        if (score > BestScore)
        {
            BestScore = score;
            PlayerPrefs.SetInt("BestScore", BestScore); // 저장
            PlayerPrefs.Save();
        }

        if (surfaceEffector2D == null) return;

        if (Input.GetKey(KeyCode.W))
        {
            surfaceEffector2D.speed = 20f; // 더 높은 속도 설정
        }
        else
        {
            surfaceEffector2D.speed = 0f; // 정지 상태
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateSurfaceText($"Surface Speed : {surfaceEffector2D.speed:F1}");
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            Jump();
        }

        if (!onGround)
        {
            float rotateInput = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
                rotateInput = 1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                rotateInput = -1f;

            rb.AddTorque(rotateInput * 5f, ForceMode2D.Force);

            float deltaAngle = Mathf.DeltaAngle(lastGroundedAngle, transform.eulerAngles.z);
            rotationAccum += Mathf.Abs(deltaAngle);
            lastGroundedAngle = transform.eulerAngles.z;

            if (rotationAccum >= 360f)
            {
                score += 1;
                rotationAccum -= 360f;

                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateScoreText($"{score:00}");
                }
            }
        }

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1f)
            {
                score += 1;
                scoreTimer = 0f;

                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateScoreText($"{score:00}");
                }
            }
        }
        else
        {
            scoreTimer = 0f;
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");
        }

        // 차가 뒤집혔는지 확인
        if (Mathf.Abs(transform.eulerAngles.z) > 90f && Mathf.Abs(transform.eulerAngles.z) < 270f)
        {
            GameManager.Instance.GameStop(); // 게임 오버 처리
        }
    }

    private void Jump()
    {
        onGround = false;

        if (rb == null) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        lastGroundedAngle = transform.eulerAngles.z;
        rotationAccum = 0f;
    }
}
