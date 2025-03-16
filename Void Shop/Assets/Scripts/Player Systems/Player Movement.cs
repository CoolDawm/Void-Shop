using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask _groundLayer; // Слой для проверки земли

    private CharacterController _controller;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;

    private CinemachinePOV _povComponent;
    private Vector3 _velocity;
    private float _verticalRotation;
    private bool _isInStasis=false;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _lookAction = _playerInput.actions["Look"];
        _virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        _povComponent = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_isInStasis) return;
        HandleMovement();
        HandleJump();
        ApplyGravity();
        RotatePlayerToCameraDirection();
    }

    private void HandleMovement()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = (cameraForward * input.y + cameraRight * input.x).normalized;
        _controller.Move(move * _moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (_jumpAction.triggered && IsGrounded())
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void RotatePlayerToCameraDirection()
    {
        Vector3 cameraDirection = mainCamera.transform.forward;
        cameraDirection.y = 0; 
        if (cameraDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        float rayLength = 2f; 
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(rayOrigin, Vector3.down, Color.red);
        if (Physics.Raycast(rayOrigin, Vector3.down, rayLength, _groundLayer))
        {
            return true;
        }
        return false;
    }
    public void Stasis(float duration)
    {
        _isInStasis = true;
        Invoke("UnStasis", duration);
    }

    private void UnStasis()
    {
        _isInStasis = false;
    }
}