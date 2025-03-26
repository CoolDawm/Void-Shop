using UnityEditor.Search;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Transform dropPoint;

    //private InventoryController _controller;
    private GameObject _currentHandItem;

    public void Initialize(InventoryController controller)
    {
        if (dropPoint == null)
        {
            dropPoint = GameObject.FindGameObjectWithTag("Hand").transform;
        }

        if (dropPoint == null)
        {
            Debug.LogError("DropPoint (Hand) not found! Please assign a GameObject with the tag 'Hand'.");
        }
    }

    public void UpdateActiveItem(Item item)
    {
        ClearHandItem();

        if (item != null && dropPoint != null)
        {
            _currentHandItem = Instantiate(item.prefab, dropPoint.position, dropPoint.rotation);
            _currentHandItem.name = "HandItem";
            _currentHandItem.transform.parent = dropPoint;

            var rb = _currentHandItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

    public void DropItem(Item item)
    {
        if (item != null)
        {
            Instantiate(item.prefab, dropPoint.position, Quaternion.identity);
        }
    }

    public void DropAllItems(Item[] items)
    {
        ClearHandItem();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                Instantiate(items[i].prefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private void ClearHandItem()
    {
        if (_currentHandItem != null)
        {
            Destroy(_currentHandItem);
            _currentHandItem = null;
        }
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        float radius = 2f;
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    }
}