using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage;
    public Image selectionImage;
    public Image background;
    private Vector3 originalScale;
    public Vector3 selectedScale = Vector3.one * 1.1f;

    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void SetItem(SO_Item item)
    {
        if (itemImage != null)
        {
            if (item != null)
            {
                itemImage.sprite = item.itemImage;
                itemImage.enabled = true;
            }
            else
            {
                itemImage.sprite = null;
                itemImage.enabled = false;
            }
        }
    }

    public void SetSelectionActive(bool isActive)
    {
        if (selectionImage != null)
        {
            Debug.Log("SetSelectionActive: " + isActive);
            selectionImage.color = isActive ? selectedColor : defaultColor;
            selectionImage.transform.localScale = isActive ? Vector3.one * 2f : Vector3.one;
            selectionImage.enabled = isActive;
        }
    }

    public void SetBackgroundColor(bool isActive)
    {
        if (background != null)
        {
            background.color = isActive ? selectedColor : defaultColor;
        }
    }
}