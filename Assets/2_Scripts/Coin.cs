using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem coinParticle;
    public AudioSource audioSource;
    public AudioClip coinSound; // �ν����Ϳ��� ������ �� �ֵ��� AudioClip ���� �߰�
    private SpriteRenderer spriteRenderer; // SpriteRenderer ���� �߰�
    public bool isFirst = true; // ù ��° �������� ���θ� ��Ÿ���� ����

    void Start()
    {
        coinParticle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ��������

        // Start �޼��忡�� ������� ������� ����
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogWarning("AudioSource �Ǵ� Audio Clip�� �������� �ʾҽ��ϴ�.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& isFirst == true )
        {
            isFirst = false; // ù ��° ���� ȹ�� �� false�� ����
            if (audioSource != null)
            {
                if (!audioSource.enabled)
                {
                    Debug.LogWarning("AudioSource�� ��Ȱ��ȭ �����Դϴ�. Ȱ��ȭ�մϴ�.");
                    audioSource.enabled = true; // AudioSource Ȱ��ȭ
                }

                if (audioSource.clip != null)
                {
                    Debug.Log("AudioSource.PlayOneShot ȣ���");
                    audioSource.PlayOneShot(audioSource.clip);
                }
                else
                {
                    Debug.LogWarning("Audio Clip�� null�Դϴ�.");
                }
            }
            else
            {
                Debug.LogWarning("AudioSource�� null�Դϴ�.");
            }

            if (coinParticle != null)
            {
                coinParticle.Play();
            }

            Invoke(nameof(DelayDestroy), 2f);
        }
    }
    void DelayDestroy()
    {
        Destroy(gameObject);
    }
}
