using UnityEngine;

public class Inventory : MonoBehaviour
{
   
    public Item flashlight; //to private in future or delete entirly
    public int currentWeight = 0; //to private in future
    public int activeSlotIndex = -1; //to private in future
    public Transform dropPoint;//to private in future
    private Item[] _slots = new Item[5];
    private void Start()
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

    public bool AddItem(Item item)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = item;
                currentWeight += item.weight;
                UpdateWeight();
                return true;
            }
        }
        return false; 
    }

    public void RemoveItem(int index)
    {
        if (_slots[index] != null)
        {
            if (dropPoint != null)
            {
                Instantiate(_slots[index].prefab, dropPoint.position, dropPoint.rotation);
            }
            else
            {
                Debug.LogWarning("DropPoint is not assigned!");
            }

            currentWeight -= _slots[index].weight;
            _slots[index] = null;
            UpdateWeight();
        }
    }
    public void DropActiveItem()
    {
        if (activeSlotIndex != -1 && _slots[activeSlotIndex] != null)
        {
            if (dropPoint.Find("HandItem"))
            {
                Destroy(dropPoint.Find("HandItem").gameObject);
            }

            Vector3 spawnPosition = GetRandomPositionAroundPlayer();
            Instantiate(_slots[activeSlotIndex].prefab, spawnPosition, Quaternion.identity);

            currentWeight -= _slots[activeSlotIndex].weight;
            _slots[activeSlotIndex] = null;
            UpdateWeight();

            activeSlotIndex = -1;
        }
    }
    public void DropAllItems()
    {
        if (dropPoint.Find("HandItem"))
        {
            Destroy(dropPoint.Find("HandItem").gameObject);
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
            {
                Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                Instantiate(_slots[i].prefab, spawnPosition, Quaternion.identity);

                _slots[i] = null;
            }
        }

        currentWeight = 0;
        UpdateWeight();
        activeSlotIndex = -1;
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        float radius = 2f; 
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y); // Позиция на уровне игрока
        return spawnPosition;
    }

    public void SetActiveSlot(int index)
    {
        if (index >= 0 && index < _slots.Length)
        {
            activeSlotIndex = index;
            UpdateActiveItem();
        }
    }

    private void UpdateActiveItem()
    {
        if (dropPoint.Find("HandItem"))
        {
            Destroy(dropPoint.Find("HandItem").gameObject);
        }

        if (activeSlotIndex != -1 && _slots[activeSlotIndex] != null && dropPoint != null)
        {
            GameObject handItem = Instantiate(_slots[activeSlotIndex].prefab, dropPoint.position, dropPoint.rotation);
            handItem.name = "HandItem";
            handItem.transform.parent = dropPoint;
            handItem.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void UpdateWeight()
    {
       
    }
}