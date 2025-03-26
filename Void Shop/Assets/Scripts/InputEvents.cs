using UnityEngine.InputSystem;

public static class InputEvents
{
    public static event System.Action<InputAction.CallbackContext> OnSwitchSlot;
    public static event System.Action<InputAction.CallbackContext> OnHotkey;
    public static void InvokeSwitchSlot(InputAction.CallbackContext context) => OnSwitchSlot?.Invoke(context);
    public static void InvokeHotkey(InputAction.CallbackContext context) => OnHotkey?.Invoke(context);
}