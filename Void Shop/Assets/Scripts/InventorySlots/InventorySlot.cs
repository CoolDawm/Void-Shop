using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage;
    public GameObject selectionFrame;
    private Vector3 originalScale;
    public Vector3 selectedScale = Vector3.one * 1.1f;

    void Start()
    {
        originalScale = selectionFrame.transform.localScale;
    }

    public void SetItem(Item item)
    {
        if (itemImage != null)
        {
            if (item != null)
            {
                itemImage.sprite = item.icon;
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
        if (selectionFrame != null)
        {
            Debug.Log("SetSelectionActive: " + isActive);
            selectionFrame.SetActive(isActive);
            selectionFrame.transform.localScale = isActive ? selectedScale : originalScale;
        }
        else
        {
            Debug.LogWarning("selectionFrame is null");
        }
    }
}