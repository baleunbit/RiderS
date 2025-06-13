using UnityEngine;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool onGround = false;

    public float jumpForce = 7f;

    private int score = 0;
    private float lastGroundedAngle = 0f;
    private float rotationAccum = 0f;
    private float scoreTimer = 0f;

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

            rotationAccum = 0f;
            lastGroundedAngle = transform.eulerAngles.z;
        }
    }

    private void Update()
    {
        if (surfaceEffector2D == null) return;

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
                UIManager.Instance.UpdateScoreText($"{score:00}");
            }
        }

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1f)
            {
                score += 1;
                scoreTimer = 0f;
                UIManager.Instance.UpdateScoreText($"{score:00}");
            }
        }
        else
        {
            scoreTimer = 0f;
        }

        UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");
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
