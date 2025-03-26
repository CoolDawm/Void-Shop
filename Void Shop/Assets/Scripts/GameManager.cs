using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public float pickUpDistance = 2f;
    public LayerMask itemLayer;
    public System.Action<GameObject> onPickUpInput;

    [Header("Inventory Settings")]
    public int inventorySlotCount = 5;
    public int inventoryMaxWeight = 10;

    private Camera _playerCamera;
    private InventoryModel _inventoryModel;
    private InventoryView _inventoryView;
    private InventoryController _inventoryController;
    private PlayerUI _playerUI;
    void Awake()
    {
        _playerCamera = Camera.main;
        _inventoryView=FindAnyObjectByType<InventoryView>();
        _inventoryModel = new InventoryModel(inventorySlotCount, inventoryMaxWeight);
        _inventoryController = new InventoryController(_inventoryModel, _inventoryView);
        _inventoryView.Initialize(_inventoryController);
        FindAnyObjectByType<PlayerUI>().inventoryModel = _inventoryModel;
        _playerUI = FindAnyObjectByType<PlayerUI>();
        InputEvents.OnSwitchSlot += HandleSwitchSlot;
        InputEvents.OnHotkey += HandleHotkey;
        _playerUI.inventoryModel = _inventoryModel;
    }

    void OnDestroy()
    {
        InputEvents.OnSwitchSlot -= HandleSwitchSlot;
        InputEvents.OnHotkey -= HandleHotkey;
    }

    private void HandleSwitchSlot(InputAction.CallbackContext context)
    {
        int slotIndex = int.Parse(context.control.name) - 1;
        if (slotIndex >= 0 && slotIndex < inventorySlotCount)
        {
            _inventoryController.SetActiveSlot(slotIndex);
        }
    }

    private void HandleHotkey(InputAction.CallbackContext context)
    {
        switch (context.control.name)
        {
            case "q":
                _inventoryController.DropAllItems();
                break;
            case "e":
                PickUpItem();
                break;
            case "r":
                _inventoryController.DropActiveItem();
                break;
        }
    }

    private void PickUpItem()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpDistance, itemLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                Item item = interactable.itm;
                if (_inventoryController.AddItem(item))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}