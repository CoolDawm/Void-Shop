using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weightText;
    public InventorySlot[] slotUI;
    public int slotCount = 5;
    private int selectedSlotIndex = 0;
    public int maxWeight = 10;

    public Item flashlight; //to private in future or delete entirly
    public int currentWeight = 0; //to private in future
    public Transform dropPoint;//to private in future
    private Item[] _slots = new Item[5];

    void Start()
    {
        if (slotUI == null || slotUI.Length != slotCount)
        {
            Debug.LogError("Слоты не назначены или их количество не соответствует slotCount!");
            return;
        }

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
        UpdateWeight();
    }

    void Update()
    {
        switch (Input.inputString)
        {
            case "1":
                SelectSlot(0);
                break;
            case "2":
                SelectSlot(1);
                break;
            case "3":
                SelectSlot(2);
                break;
            case "4":
                SelectSlot(3);
                break;
            case "5":
                SelectSlot(4);
                break;
        }

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0)
        {
            if (scrollWheel < 0) 
            {
                selectedSlotIndex = (selectedSlotIndex - 1 + slotCount) % slotCount;
            }
            else
            {
                selectedSlotIndex = (selectedSlotIndex + 1) % slotCount;
            }
            SelectSlot(selectedSlotIndex);
        }
    }

    public void SelectSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotCount)
        {
            selectedSlotIndex = slotIndex;
            Debug.Log("Selected slot index: " + selectedSlotIndex);
            SetActiveSlot(selectedSlotIndex);
            SetActiveActiveItem(selectedSlotIndex);
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = currentHealth.ToString("F0") + "/" + maxHealth.ToString("F0");
    }

    public void UpdateWeight(float currentWeight, float maxWeight)
    {
        weightText.text = currentWeight.ToString("F0") + "/" + maxWeight.ToString("F0");
    }

    public void UpdateInventoryUI()
    {
        if (_slots != null)
        {
            for (int i = 0; i < slotUI.Length; i++)
            {
                if (i < _slots.Length && _slots[i] != null)
                {
                    slotUI[i].SetItem(_slots[i]);
                    slotUI[i].gameObject.SetActive(true);
                }
                else
                {
                    slotUI[i].SetItem(null);
                    slotUI[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetActiveSlot(int slotIndex)
    {
        Debug.Log("SetActiveSlot called with slotIndex: " + slotIndex);
        if (slotUI == null)
        {
            Debug.LogWarning("slotUI is null");
            return;
        }

        for (int i = 0; i < slotUI.Length; i++)
        {
            if (slotUI[i] != null)
            {
                Debug.Log("Setting selection for slot: " + i + ", active: " + (i == slotIndex));
                slotUI[i].SetSelectionActive(i == slotIndex);
            }
            else
            {
                Debug.LogWarning("slotUI[" + i + "] is null");
            }
        }
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
                UpdateWeight();
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
                        UpdateWeight();
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
                    UpdateWeight();
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
            UpdateWeight();
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
            UpdateWeight();
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
        weightText.text = currentWeight.ToString("F0") + " / " + maxWeight.ToString("F0") + "kg";
    }
}