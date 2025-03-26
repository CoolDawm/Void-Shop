using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image itemImage;
    public GameObject selectionFrame;
    private Vector3 originalScale;
    public Vector3 selectedScale = Vector3.one * 1.1f;
    public int slotIndex;

    void Start()
    {
        originalScale = selectionFrame.transform.localScale;
    }

    public void SetItem(Item item)
    {
        if (itemImage != null)
        {
            itemImage.sprite = item != null ? item.icon : null;
            itemImage.enabled = item != null;
        }
    }

    public void SetSelectionActive(bool isActive)
    {
        if (selectionFrame != null)
        {
            selectionFrame.SetActive(isActive);
            selectionFrame.transform.localScale = isActive ? selectedScale : originalScale;
        }
    }
}