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
            if (audioSource != null && audioSource.enabled) // AudioSource�� Ȱ��ȭ�� ��츸 ���
            {
                audioSource.Play();
            }

            if (coinParticle != null)
            {
                coinParticle.Play();
            }

            // ���� ȹ�� ����
            Destroy(gameObject, 0.5f);
        }
    }
}
