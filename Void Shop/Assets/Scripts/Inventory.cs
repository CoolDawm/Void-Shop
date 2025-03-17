using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotCount = 5;
    public int maxWeight = 10;
    public Transform dropPoint;

    private Item[] _slots;
    private int currentWeight = 0;
    private int selectedSlotIndex = 0;

    public int CurrentWeight => currentWeight;
    public int SelectedSlotIndex => selectedSlotIndex;

    public delegate void InventoryUpdated();
    public event InventoryUpdated OnInventoryUpdated;

    public delegate void ActiveItemChanged(int index);
    public event ActiveItemChanged OnActiveItemChanged;

    private void Awake()
    {
        _slots = new Item[slotCount];
    }

    void Start()
    {
        if (dropPoint == null)
        {
            dropPoint = GameObject.FindGameObjectWithTag("Hand").transform;
        }

        if (dropPoint == null)
        {
            Debug.LogError("DropPoint (Hand) not found! Please assign a GameObject with the tag 'Hand'.");
        }

        SetActiveSlot(selectedSlotIndex);
        UpdateInventoryUI();
    }

    public bool AddItem(Item item)
    {
        if (currentWeight + item.weight > maxWeight)
        {
            Debug.Log("Not enough weight to pick up " + item.itemName);
            return false;
        }

        if (selectedSlotIndex >= 0 && selectedSlotIndex < _slots.Length)
        {
            if (_slots[selectedSlotIndex] == null)
            {
                _slots[selectedSlotIndex] = item;
                currentWeight += item.weight;
                SetActiveActiveItem(selectedSlotIndex);
                UpdateInventoryUI();
                return true;
            }
            else
            {
                for (int i = 0; i < _slots.Length; i++)
                {
                    if (_slots[i] == null)
                    {
                        _slots[i] = item;
                        currentWeight += item.weight;
                        SetActiveActiveItem(i);
                        UpdateInventoryUI();
                        return true;
                    }
                }
                return false;
            }
        }
        else
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                {
                    _slots[i] = item;
                    currentWeight += item.weight;
                    SetActiveActiveItem(i);
                    UpdateInventoryUI();
                    return true;
                }
            }
            return false;
        }
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < _slots.Length && _slots[index] != null)
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
            UpdateInventoryUI();
        }
    }

    public void DropActiveItem()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < _slots.Length && _slots[selectedSlotIndex] != null)
        {
            if (dropPoint.Find("HandItem"))
            {
                Rigidbody rb = dropPoint.Find("HandItem").GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.transform.parent = null;
            }

            currentWeight -= _slots[selectedSlotIndex].weight;
            _slots[selectedSlotIndex] = null;
            UpdateInventoryUI();
        }
    }

    public void PlaceItemInSlot(Item item, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < _slots.Length)
        {
            if (_slots[slotIndex] == null)
            {
                _slots[slotIndex] = item;
                UpdateInventoryUI();
            }
            else
            {
                Debug.LogWarning("Slot " + slotIndex + " is not empty.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid slot index: " + slotIndex);
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
        UpdateInventoryUI();
        selectedSlotIndex = -1;
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        float radius = 2f;
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        return spawnPosition;
    }

    public void SetActiveSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        OnActiveItemChanged?.Invoke(selectedSlotIndex);
        SetActiveActiveItem(selectedSlotIndex);
    }

    private void SetActiveActiveItem(int index)
    {
        if (dropPoint.Find("HandItem"))
        {
            Destroy(dropPoint.Find("HandItem").gameObject);
        }

        if (index != -1 && _slots[index] != null && dropPoint != null)
        {
            GameObject handItem = Instantiate(_slots[index].prefab, dropPoint.position, dropPoint.rotation);
            handItem.name = "HandItem";
            handItem.transform.parent = dropPoint;
            Rigidbody rb = handItem.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    public Item[] GetItemsList()
    {
        return _slots;
    }

    private void UpdateWeight()
    {
        OnInventoryUpdated?.Invoke();
        Debug.Log("Current weight: " + currentWeight + " / Max weight: " + maxWeight);
    }

    private void UpdateInventoryUI()
    {
        OnInventoryUpdated?.Invoke();
    }
}