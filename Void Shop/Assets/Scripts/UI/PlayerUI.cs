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