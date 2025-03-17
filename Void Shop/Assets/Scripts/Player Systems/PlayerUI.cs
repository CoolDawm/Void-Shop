using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI weightText;
    public InventorySlot[] slotUI;
    public Inventory inventory;

    private void Start()
    {
        if (slotUI == null || slotUI.Length != inventory.slotCount)
        {
            Debug.LogError("Слоты не назначены или их количество не соответствует slotCount!");
            return;
        }

        inventory.OnInventoryUpdated += UpdateInventoryUI;
        inventory.OnActiveItemChanged += UpdateActiveItem;

        UpdateInventoryUI();
        UpdateWeight();
        UpdateActiveItem(inventory.SelectedSlotIndex);
    }

    void Update()
    {
        switch (Input.inputString)
        {
            case "1":
                inventory.SetActiveSlot(0);
                break;
            case "2":
                inventory.SetActiveSlot(1);
                break;
            case "3":
                inventory.SetActiveSlot(2);
                break;
            case "4":
                inventory.SetActiveSlot(3);
                break;
            case "5":
                inventory.SetActiveSlot(4);
                break;
        }

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0)
        {
            if (scrollWheel < 0)
            {
                inventory.SetActiveSlot((inventory.SelectedSlotIndex - 1 + inventory.slotCount) % inventory.slotCount);
            }
            else
            {
                inventory.SetActiveSlot((inventory.SelectedSlotIndex + 1) % inventory.slotCount);
            }
        }
    }

    public void UpdateWeight()
    {
        weightText.text = inventory.CurrentWeight.ToString("F0") + " / " + inventory.maxWeight.ToString("F0") + "kg";
    }

    public void UpdateInventoryUI()
    {
        Debug.Log("UpdateInventoryUI called");
        Item[] slots = inventory.GetItemsList();
        if (slots != null)
        {
            Debug.Log("Slots count: " + slots.Length);
            for (int i = 0; i < slotUI.Length; i++)
            {
                Debug.Log("Slot " + i + ": " + (slots[i] != null ? slots[i].itemName : "null"));
                if (i < slots.Length && slots[i] != null)
                {
                    slotUI[i].SetItem(slots[i]);
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

    public void UpdateActiveItem(int index)
    {
        if (slotUI == null)
        {
            Debug.LogWarning("slotUI is null");
            return;
        }

        for (int i = 0; i < slotUI.Length; i++)
        {
            if (slotUI[i] != null)
            {
                slotUI[i].SetSelectionActive(i == index);
            }
            else
            {
                Debug.LogWarning("slotUI[" + i + "] is null");
            }
        }
    }
}