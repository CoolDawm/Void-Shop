using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionsSystem : MonoBehaviour
{
    public float pickUpDistance = 2f;
    public LayerMask itemLayer;

    private Inventory _inventory;
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
       
        int slotIndex = int.Parse(context.control.name) - 1;
        if (slotIndex >= 0 && slotIndex < 5)
        {
            _inventory.SetActiveSlot(slotIndex);
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