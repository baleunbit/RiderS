using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1f; // �� ��ε� ������
    [SerializeField] private ParticleSystem crashEffect; // ũ���� ����Ʈ (�ν����Ϳ��� ����)
    [SerializeField] private AudioSource crashSound; // ũ���� ���� (�ν����Ϳ��� ����)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (crashEffect != null)
            {
                crashEffect.Play(); // ũ���� ����Ʈ ���
            }

            if (crashSound != null)
            {
                crashSound.Play(); // ũ���� ���� ���
            }

            UIManager.Instance?.ShowGameOverPanel(); // ���� ���� �г� ǥ��
            Invoke(nameof(ReloadScene), reloadDelay); // ������ �� �� ��ε�
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(0); // �� �ε��� 0���� ��ε�
    }
}