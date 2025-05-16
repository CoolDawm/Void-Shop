using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCheckingZone : MonoBehaviour
{
    private ShoppingList _shoppingList;
    private GameUIManager _uiManager;

    void Start()
    {
        _shoppingList = FindAnyObjectByType<ShoppingList>();
        _uiManager = FindAnyObjectByType<GameUIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("GameUIManager not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Level Complete");
            Item[] items = GameManager.Instance.GetPlayerItems();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (_shoppingList.CheckInventoryWithList(items))
            {
                WinGame();
            }
            else
            {
                LoseGame();
            }
        }
    }

    private void WinGame()
    {
        if (_uiManager != null)
        {
            _uiManager.ShowWinMenu();
        }
        else
        {
            Debug.Log("Win condition achieved!");
            // Fallback if UI Manager is missing
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void LoseGame()
    {
        if (_uiManager != null)
        {
            _uiManager.ShowLoseMenu();
        }
        else
        {
            Debug.Log("Lose condition achieved!");
            // Fallback if UI Manager is missing
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}