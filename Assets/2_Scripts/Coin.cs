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

    // 트리거에 들어왔을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        coinParticle.Play();
        audioSource.Play();

        Destroy(gameObject, 0.5f);
    }
}
