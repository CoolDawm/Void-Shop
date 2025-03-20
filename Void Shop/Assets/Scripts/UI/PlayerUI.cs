using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI weightText;
    public InventorySlotUI[] slotUI;
    public Inventory inventory;

    private void Start()
    {
        if (slotUI == null || slotUI.Length != inventory.slotCount)
        {
            Debug.LogError("Слоты не назначены или их количество не соответствует slotCount!");
            return;
        }

        inventory.OnInventoryUpdated += UpdateInventoryUI;
        inventory.OnInventoryUpdated += UpdateWeight;
        inventory.OnActiveItemChanged += UpdateActiveItem;

        UpdateInventoryUI();
        UpdateWeight();
        UpdateActiveItem(inventory.SelectedSlotIndex);
    }

    void Update()
    {
        HandleInput();
        HandleScrollWheel();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SetActiveSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SetActiveSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SetActiveSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) inventory.SetActiveSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) inventory.SetActiveSlot(4);
    }

    private void HandleScrollWheel()
    {
        float scrollWheel = Input.mouseScrollDelta.y;
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
        weightText.text = inventory.CurrentWeight.ToString("F0") + " / " + inventory.maxWeight.ToString("F0") + " kg";
    }

    public void UpdateInventoryUI()
    {
        Item[] slots = inventory.GetItemsList();
        if (slots != null)
        {
            for (int i = 0; i < slotUI.Length; i++)
            {
                if (i < slots.Length && slots[i] != null)
                {
                    slotUI[i].SetItem(slots[i]);
                }
                else
                {
                    slotUI[i].SetItem(null);
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