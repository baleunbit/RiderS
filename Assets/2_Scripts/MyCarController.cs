using UnityEngine;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool onGround = false;

    public float jumpForce = 7f;

    // 점수 및 회전 체크용 변수
    private int score = 0;
    private float lastGroundedAngle = 0f;
    private float rotationAccum = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            onGround = true;
            surfaceEffector2D = effector;

            // 착지 시 회전량 초기화
            rotationAccum = 0f;
            lastGroundedAngle = transform.eulerAngles.z;
        }
    }

    private void Update()
    {
        if (surfaceEffector2D == null) return;

        // W 키를 누를 때만 앞으로 이동
        if (Input.GetKey(KeyCode.W))
        {
            surfaceEffector2D.speed = 10f;
        }
        else
        {
            surfaceEffector2D.speed = 0f;
        }
        UIManager.Instance.UpdateSurfaceText($"Surface Speed : {surfaceEffector2D.speed:F1}");

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            Jump();
        }

        // 공중에서 좌/우 방향키로 차량 회전
        if (!onGround)
        {
            float rotateInput = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
                rotateInput = 1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                rotateInput = -1f;

            rb.AddTorque(rotateInput * 5f, ForceMode2D.Force);

            // 회전 누적 계산
            float deltaAngle = Mathf.DeltaAngle(lastGroundedAngle, transform.eulerAngles.z);
            rotationAccum += Mathf.Abs(deltaAngle);
            lastGroundedAngle = transform.eulerAngles.z;

            // 360도 회전 체크
            if (rotationAccum >= 360f)
            {
                score += 1;
                rotationAccum -= 360f; // 여러 바퀴 돌았을 때도 처리
                UIManager.Instance.UpdateScoreText($"Score : {score}");
            }
        }

        UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");
    }

    private void Jump()
    {
        onGround = false;

        if (rb == null) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // 점프 시작 시 회전 체크 초기화
        lastGroundedAngle = transform.eulerAngles.z;
        rotationAccum = 0f;
    }
}
