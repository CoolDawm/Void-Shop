using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Image _weightImage;
    public InventorySlotUI[] slotUI;
    
    [Header("Model References")]
    public InventoryModel inventoryModel;
    [Header("Weight")]
    [SerializeField]
    private List<Sprite> weightSprites = new List<Sprite>();
    private void Start()
    {
        if (slotUI == null || slotUI.Length != inventoryModel.SlotCount)
        {
            slotUI = GetComponentsInChildren<InventorySlotUI>();
        }

        inventoryModel.OnInventoryUpdated += UpdateInventoryUI;
        inventoryModel.OnActiveItemChanged += UpdateActiveItem;
        
        UpdateActiveItem(inventoryModel.GetItem(inventoryModel.SelectedSlotIndex));
    }
    private void UpdateInventoryUI(int weightLevel)
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
        _weightImage.sprite = weightSprites[weightLevel];
    }
    private void OnDestroy()
    {
        inventoryModel.OnInventoryUpdated -= UpdateInventoryUI;
        inventoryModel.OnActiveItemChanged -= UpdateActiveItem;
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