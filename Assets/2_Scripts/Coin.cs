using UnityEngine;

public class Coin : MonoBehaviour
{
    private ParticleSystem coinParticle;
    private AudioSource audioSource;

    void Start()
    {
        coinParticle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    // Ʈ���ſ� ������ �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter2D(Collider2D other)
    {
        coinParticle.Play();
        audioSource.Play();

        Destroy(gameObject, 0.5f);
    }
}
