using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public PlayerHUD playerHUD;
    public int slotCount = 5;
    private int selectedSlotIndex = 0;

    void Start()
    {
        playerHUD.SetActiveSlot(selectedSlotIndex);
    }

    void Update()
    {
        switch (Input.inputString)
        {
            case "1":
                SelectSlot(0);
                break;
            case "2":
                SelectSlot(1);
                break;
            case "3":
                SelectSlot(2);
                break;
            case "4":
                SelectSlot(3);
                break;
            case "5":
                SelectSlot(4);
                break;
        }
    }

    public void SelectSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotCount && selectedSlotIndex != slotIndex)
        {
            selectedSlotIndex = slotIndex;
            playerHUD.SetActiveSlot(selectedSlotIndex);
        }
    }
}