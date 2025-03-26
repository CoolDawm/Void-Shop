using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI weightText;
    public InventorySlotUI[] slotUI;
    
    [Header("Model References")]
    public InventoryModel inventoryModel; 

    private void Start()
    {
        if (slotUI == null || slotUI.Length != inventoryModel.SlotCount)
        {
            slotUI = GetComponentsInChildren<InventorySlotUI>();
        }

        inventoryModel.OnInventoryUpdated += UpdateInventoryUI;
        inventoryModel.OnInventoryUpdated += UpdateWeight;
        inventoryModel.OnActiveItemChanged += UpdateActiveItem;

        UpdateInventoryUI();
        UpdateWeight();
        UpdateActiveItem(inventoryModel.GetItem(inventoryModel.SelectedSlotIndex));
    }

    private void OnDestroy()
    {
        inventoryModel.OnInventoryUpdated -= UpdateInventoryUI;
        inventoryModel.OnInventoryUpdated -= UpdateWeight;
        inventoryModel.OnActiveItemChanged -= UpdateActiveItem;
    }

    public void UpdateWeight()
    {
        weightText.text = $"{inventoryModel.CurrentWeight} / {inventoryModel.MaxWeight} kg";
    }

    public void UpdateInventoryUI()
    {
        Item[] slots = inventoryModel.GetItemsList();
        if (slots != null)
        {
            for (int i = 0; i < slotUI.Length; i++)
            {
                if (i < slots.Length)
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

    public void UpdateActiveItem(Item activeItem)
    {
        int activeIndex = inventoryModel.SelectedSlotIndex;
        
        for (int i = 0; i < slotUI.Length; i++)
        {
            if (slotUI[i] != null)
            {
                slotUI[i].SetSelectionActive(i == activeIndex);
            }
        }
    }
}