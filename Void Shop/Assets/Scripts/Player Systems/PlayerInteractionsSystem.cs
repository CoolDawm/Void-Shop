using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionsSystem : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _switchSlotAction;
    private InputAction _HotKeysAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _switchSlotAction = _playerInput.actions["SwitchSlot"];
        _HotKeysAction = _playerInput.actions["HotKeys"];
    }

    private void OnEnable()
    {
        _switchSlotAction.performed += OnSwitchSlot;
        _HotKeysAction.performed += OnHotkey;
    }

    private void OnDisable()
    {
        _switchSlotAction.performed -= OnSwitchSlot;
        _HotKeysAction.performed -= OnHotkey;
    }

    private void OnSwitchSlot(InputAction.CallbackContext context)
    {
        InputEvents.InvokeSwitchSlot(context);
    }

    private void OnHotkey(InputAction.CallbackContext context)
    {
        InputEvents.InvokeHotkey(context);
    }

}