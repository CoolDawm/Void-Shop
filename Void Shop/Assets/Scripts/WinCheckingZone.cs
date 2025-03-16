using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCheckingZone : MonoBehaviour
{
    private ShoppingList _shoppingList;
    void Start()
    {
        _shoppingList = FindAnyObjectByType<ShoppingList>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           
            Item[] items = other.GetComponent<PlayerInventory>().GetItemsList();
            if (_shoppingList.CheckInventoryWithList(items)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
