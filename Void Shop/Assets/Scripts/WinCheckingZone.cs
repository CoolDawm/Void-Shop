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
            Debug.Log("Level Complete");
            //Item[] items = other.GetComponent<Inventory>().GetItemsList();
          //  if (_shoppingList.CheckInventoryWithList(items)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
