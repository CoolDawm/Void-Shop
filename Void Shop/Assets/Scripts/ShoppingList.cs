using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingList : MonoBehaviour
{
    [SerializeField] private Item[] shoppingItems = new Item[5];
    [SerializeField] private GameObject _itemsIconsParent;
    [SerializeField] private GameObject _itemTextsParent;

    private List<Image> itemIcons = new List<Image>();
    private List<TextMeshProUGUI> itemTexts = new List<TextMeshProUGUI>();

    private void Start()
    {
        foreach (Transform child in _itemsIconsParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _itemTextsParent.transform)
        {
            Destroy(child.gameObject);
        }

        itemIcons.Clear();
        itemTexts.Clear();

        foreach (var item in shoppingItems)
        {
            if (item == null) continue;

            if (_itemsIconsParent != null && item.icon != null)
            {
                GameObject iconGO = new GameObject("ItemIcon");
                iconGO.transform.SetParent(_itemsIconsParent.transform, false);

                Image iconImage = iconGO.AddComponent<Image>();
                iconImage.sprite = item.icon;
                iconImage.color = new Color(1f, 1f, 1f, 0.5f); 
                itemIcons.Add(iconImage);
            }
            if (_itemTextsParent != null)
            {
                GameObject textGO = new GameObject("ItemText");
                textGO.transform.SetParent(_itemTextsParent.transform, false);

                TextMeshProUGUI textComponent = textGO.AddComponent<TextMeshProUGUI>();
                textComponent.text = item.itemName;
                textComponent.enableAutoSizing = true;
                textComponent.color = new Color(1f, 1f, 1f, 0.5f); 
                itemTexts.Add(textComponent);
            }
        }
    }

    public bool CheckInventoryWithList(Item[] items)
    {
        int trueCount = 0;
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < shoppingItems.Length; j++)
            {
                if (items[i] == shoppingItems[j])
                {
                    trueCount++;
                    break;
                }
            }
        }
        return trueCount == shoppingItems.Length;
    }

    public void UpdateUIList(Item[] playerItems)
    {
        for (int i = 0; i < itemIcons.Count; i++)
        {
            if (itemIcons[i] != null)
            {
                Color iconColor = itemIcons[i].color;
                iconColor.a = 0.5f;
                itemIcons[i].color = iconColor;
            }
        }

        for (int i = 0; i < itemTexts.Count; i++)
        {
            if (itemTexts[i] != null)
            {
                Color textColor = itemTexts[i].color;
                textColor.a = 0.5f;
                itemTexts[i].color = textColor;
            }
        }

        for (int i = 0; i < playerItems.Length; i++)
        {
            for (int j = 0; j < shoppingItems.Length; j++)
            {
                if (playerItems[i] == shoppingItems[j] && j < itemIcons.Count && j < itemTexts.Count)
                {
                    if (itemIcons[j] != null)
                    {
                        Color iconColor = itemIcons[j].color;
                        iconColor.a = 1f;
                        itemIcons[j].color = iconColor;
                    }

                    if (itemTexts[j] != null)
                    {
                        Color textColor = itemTexts[j].color;
                        textColor.a = 1f;
                        itemTexts[j].color = textColor;
                    }
                    break;
                }
            }
        }
    }
}