using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        Time.timeScale = 1f; // ����� ��������� ����� �� ������ �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // �������� �� ��� ����� ����� �������� ����
    }

    public void QuitGame()
    {
#if    UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}       