using UnityEngine;

public class ItemZoneTrigger : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] 
    private int _requiredItems = 3;  
    [SerializeField] 
    private GameObject _doorToRemove; 
    [SerializeField] 
    private LayerMask _itemLayer;     

    private int currentItemsCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _itemLayer) != 0)
        {
            Debug.Log(currentItemsCount);
            currentItemsCount++;
            CheckItems();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _itemLayer) != 0)
        {
        
            Debug.Log(currentItemsCount);
            currentItemsCount--;
            CheckItems();
        }
    }

    private void CheckItems()
    {
        if (currentItemsCount >= _requiredItems && _doorToRemove != null)
        {
            Destroy(_doorToRemove);
            enabled = false; 
        }
    }
}