using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem coinParticle;
    public AudioSource audioSource;
    public AudioClip coinSound; // 인스펙터에서 연결할 수 있도록 AudioClip 변수 추가
    private SpriteRenderer spriteRenderer; // SpriteRenderer 참조 추가
    public bool isFirst = true; // 첫 번째 코인인지 여부를 나타내는 변수

    void Start()
    {
        coinParticle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 가져오기

        // Start 메서드에서 오디오를 재생하지 않음
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogWarning("AudioSource 또는 Audio Clip이 설정되지 않았습니다.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& isFirst == true )
        {
            isFirst = false; // 첫 번째 코인 획득 후 false로 설정
            if (audioSource != null)
            {
                if (!audioSource.enabled)
                {
                    Debug.LogWarning("AudioSource가 비활성화 상태입니다. 활성화합니다.");
                    audioSource.enabled = true; // AudioSource 활성화
                }

                if (audioSource.clip != null)
                {
                    Debug.Log("AudioSource.PlayOneShot 호출됨");
                    audioSource.PlayOneShot(audioSource.clip);
                }
                else
                {
                    Debug.LogWarning("Audio Clip이 null입니다.");
                }
            }
            else
            {
                Debug.LogWarning("AudioSource가 null입니다.");
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
