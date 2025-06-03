using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionsSystem : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _switchSlotAction;
    private InputAction _HotKeysAction;
    private InputAction _mouseScrollAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _switchSlotAction = _playerInput.actions["SwitchSlot"];
        _HotKeysAction = _playerInput.actions["HotKeys"];
        _mouseScrollAction = _playerInput.actions["MouseScrollY"];
    }

    private void OnEnable()
    {
        _switchSlotAction.performed += OnSwitchSlot;
        _HotKeysAction.performed += OnHotkey;
        _mouseScrollAction.performed += OnMouseScroll;
    }

    private void OnDisable()
    {
        _switchSlotAction.performed -= OnSwitchSlot;
        _HotKeysAction.performed -= OnHotkey;
        _mouseScrollAction.performed -= OnMouseScroll;
    }

    private void OnSwitchSlot(InputAction.CallbackContext context)
    {
        InputEvents.InvokeSwitchSlot(context);
    }

    private void OnHotkey(InputAction.CallbackContext context)
    {
        InputEvents.InvokeHotkey(context);
    }
    private void OnMouseScroll(InputAction.CallbackContext context)
    {
        InputEvents.InvokeMouseScroll(context);
    }
}