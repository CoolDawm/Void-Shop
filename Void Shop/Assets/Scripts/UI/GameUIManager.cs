using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;



    private void Start()
    {
        // Initialize all UI elements as hidden
        HideAllMenus();

        
    }

    public void ShowWinMenu()
    {
        Time.timeScale = 0f; // Pause the game
        if (_winMenu != null) _winMenu.SetActive(true);
    }

    public void ShowLoseMenu()
    {
        Time.timeScale = 0f; // Pause the game
        if (_loseMenu != null) _loseMenu.SetActive(true);
    }

    public void HideAllMenus()
    {
        Time.timeScale = 1f; // Unpause the game
        if (_winMenu != null) _winMenu.SetActive(false);
        if (_loseMenu != null) _loseMenu.SetActive(false);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}