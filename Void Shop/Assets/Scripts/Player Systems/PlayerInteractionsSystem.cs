using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionsSystem : MonoBehaviour
{
    public float pickUpDistance = 2f;
    public LayerMask itemLayer;

    private Inventory _inventory;
    private InventoryUI _inventoryUI;
    private PlayerInput _playerInput;
    private InputAction _switchSlotAction;
    private Camera _playerCamera; 


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _switchSlotAction = _playerInput.actions["SwitchSlot"];
        _inventory = GetComponent<Inventory>();
        _playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        _switchSlotAction.performed += OnSwitchSlot;
    }

    private void OnDisable()
    {
        _switchSlotAction.performed -= OnSwitchSlot;
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {

            TryPickUpItem();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            _inventory.DropAllItems();
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            _inventory.DropActiveItem();
        }
    }

    private void OnSwitchSlot(InputAction.CallbackContext context)
    {
        int slotIndex;

        if (context.control.name.StartsWith("1"))
        {
            slotIndex = 0;
        }
        else if (context.control.name.StartsWith("2"))
        {
            slotIndex = 1;
        }
        else if (context.control.name.StartsWith("3"))
        {
            slotIndex = 2;
        }
        else if (context.control.name.StartsWith("4"))
        {
            slotIndex = 3;
        }
        else if (context.control.name.StartsWith("5"))
        {
            slotIndex = 4;
        }
        else if (context.control.name == "mouse scroll wheel")
        {
            float scrollValue = context.ReadValue<Vector2>().y;
            if (scrollValue < 0)
            {
                slotIndex = (_inventory.SelectedSlotIndex - 1 + _inventory.slotCount) % _inventory.slotCount;
            }
            else
            {
                slotIndex = (_inventory.SelectedSlotIndex + 1) % _inventory.slotCount;
            }
        }
        else
        {
            return;
        }

        if (slotIndex >= 0 && slotIndex < _inventory.slotCount)
        {
            _inventory.SwitchSlot(slotIndex);
            _inventoryUI.UpdateActiveItem(slotIndex);
        }
    }

    private void TryPickUpItem()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpDistance, itemLayer))
        {
            ItemPickup itemPickup = hit.collider.GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                Item item = itemPickup.item;
                if (_inventory.AddItem(item))
                {
                    Destroy(hit.collider.gameObject); 
                }
            }
        }
    }
}