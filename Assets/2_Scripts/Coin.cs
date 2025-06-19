using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem coinParticle;
    public AudioSource audioSource;

    void Start()
    {
        coinParticle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioSource != null && audioSource.enabled) // AudioSource가 활성화된 경우만 재생
            {
                audioSource.Play();
            }

            if (coinParticle != null)
            {
                coinParticle.Play();
            }

            // 코인 획득 로직
            Destroy(gameObject, 0.5f);
        }
    }
}
