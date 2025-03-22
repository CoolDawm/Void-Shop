using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotsParent;
    public GameObject slotPrefab;
    public Image activeItemImage;

    private InventorySlotUI[] slots;

    private void Start()
    {
        InitializeSlots();
        UpdateUI();
        UpdateActiveItem(inventory.SelectedSlotIndex);

        inventory.OnInventoryUpdated += UpdateUI;
        inventory.OnActiveItemChanged += UpdateActiveItem;
    }

    private void OnDestroy()
    {
        inventory.OnInventoryUpdated -= UpdateUI;
        inventory.OnActiveItemChanged -= UpdateActiveItem;
    }

    private void InitializeSlots()
    {
        slots = new InventorySlotUI[inventory.slotCount];

        for (int i = 0; i < inventory.slotCount; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotsParent);
            InventorySlotUI slot = slotObject.GetComponent<InventorySlotUI>();
            slot.slotIndex = i;
            slots[i] = slot;
        }
    }

    private void UpdateUI()
    {
        Item[] items = inventory.GetItemsList();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetItem(i < items.Length ? items[i] : null);
        }
    }

    public void UpdateActiveItem(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelectionActive(i == index);
        }

        if (index >= 0 && index < inventory.GetItemsList().Length && inventory.GetItemsList()[index] != null)
        {
            activeItemImage.sprite = inventory.GetItemsList()[index].icon;
            activeItemImage.enabled = true;
        }
        else
        {
            activeItemImage.sprite = null;
            activeItemImage.enabled = false;
        }
    }
}