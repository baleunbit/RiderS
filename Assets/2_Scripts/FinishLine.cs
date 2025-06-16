using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 1;
    [SerializeField] private ParticleSystem finishEffect; // �ν����Ϳ��� �ݵ�� �Ҵ�

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            finishEffect.Play(); // null üũ ���� �ٷ� ����

            // currentTime �ʱ�ȭ
            GameManager.Instance?.GameRestart();

            // ���� �޴� �г� ȣ��
            UIManager.Instance?.ShowGameOverPanel();

            // ���� �ð� �� �� ��ε�
            Invoke(nameof(ReloadScene), reloadDelay);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}