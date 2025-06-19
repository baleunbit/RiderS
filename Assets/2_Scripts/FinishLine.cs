using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1f; // �� ��ε� ������
    [SerializeField] private ParticleSystem finishEffect; // �ν����Ϳ��� �ݵ�� �Ҵ�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (finishEffect != null)
            {
                finishEffect.Play(); // ���� �� ����Ʈ ���
            }

            Invoke(nameof(ReloadScene), reloadDelay); // ������ �� �� ��ε�
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(0); // �� �ε��� 0���� ��ε�
    }
}
