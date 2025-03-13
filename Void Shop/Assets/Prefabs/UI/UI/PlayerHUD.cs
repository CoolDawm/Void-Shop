using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weightText;
    public InventorySlot[] inventorySlots;

    void Start()
    {
        if (inventorySlots == null)
        {
            Debug.LogError("слоты не найдены в плеер худ!");
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

    public void UpdateInventory(List<SO_Item> inventory)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.Count && inventory[i] != null)
            {
                inventorySlots[i].SetItem(inventory[i]);
                inventorySlots[i].gameObject.SetActive(true);
            }
            else
            {
                inventorySlots[i].SetItem(null);
                inventorySlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetActiveSlot(int slotIndex)
    {
        if (inventorySlots == null) return;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != null)
            {
                inventorySlots[i].SetSelectionActive(i == slotIndex);
            }
        }
    }
}