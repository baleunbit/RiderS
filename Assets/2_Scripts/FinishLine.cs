using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float delayToMainMenu = 2f; // ���� �޴��� ��ȯ �� ������
    [SerializeField] private ParticleSystem finishEffect; // ��ƼŬ ����Ʈ (�ν����Ϳ��� ����)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (finishEffect != null)
            {
                finishEffect.Play(); // ���� �� ����Ʈ ���
            }

            Time.timeScale = 0f; // ���� �Ͻ�����
            UIManager.Instance?.ShowGameOverPanel(); // ���� ���� �г� ��� ǥ��

            Invoke(nameof(GoToMainMenu), delayToMainMenu); // 2�� �� ���� �޴� �̵�
        }
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // �ð� �ٽ� ����ȭ
        SceneManager.LoadScene("1_Scenes/MainMenuScene"); // ���� �޴� �� �ε�
    }
}
