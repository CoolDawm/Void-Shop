using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _rotationSpeed = 100f;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Camera mainCamera;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckRadius = 0.3f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private Vector3 _groundCheckOffset = new Vector3(0, -0.1f, 0);
    [SerializeField] private int _groundCheckRays = 4;

    [Header("Footstep Settings")]
    [SerializeField] private float _footstepInterval = 0.5f;
    private float _footstepTimer;

    [Header("Weight Settings")]
    [SerializeField] private float[] speedModifiers = new float[] { 1f, 0.9f, 0.75f, 0.6f, 0.5f };
    private float _baseMoveSpeed;

    private CharacterController _controller;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;
    private CinemachinePOV _povComponent;
    private Vector3 _velocity;
    private bool _isInStasis = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _baseMoveSpeed = _moveSpeed;
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _lookAction = _playerInput.actions["Look"];

        if (_virtualCamera == null)
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        _povComponent = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        GameManager.Instance.GetInventoryModel().OnInventoryUpdated += UpdateSpeedByWeight;
    }

    private void OnDestroy()
    {
        GameManager.Instance.GetInventoryModel().OnInventoryUpdated -= UpdateSpeedByWeight;
    }

    private void Update()
    {
        if (_isInStasis) return;
        HandleMovement();
        HandleJump();
        ApplyGravity();
        RotatePlayerToCameraDirection();
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        bool isMoving = IsGrounded() && IsMoving();

        if (isMoving)
        {
            // Если таймер достиг нуля - сразу играем звук
            if (_footstepTimer <= 0f)
            {
                GameAudioEvents.OnFootstepRequested?.Invoke();
                _footstepTimer = _footstepInterval;
            }
            else
            {
                _footstepTimer -= Time.deltaTime;
            }
        }
        else
        {
            _footstepTimer = 0f; // Сбрасываем таймер при остановке
            GameAudioEvents.OnFootstepStopped?.Invoke();
        }
    }

    private bool IsMoving()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();
        return input.magnitude > 0.1f;
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
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity.y < 0)
            _velocity.y = -2f;

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
        Vector3 center = transform.position + _groundCheckOffset;
        if (Physics.Raycast(center, Vector3.down, _groundCheckDistance + _groundCheckRadius, _groundLayer))
        {
            Debug.DrawRay(center, Vector3.down * (_groundCheckDistance + _groundCheckRadius), Color.green);
            return true;
        }

        for (int i = 0; i < _groundCheckRays; i++)
        {
            float angle = i * (360f / _groundCheckRays);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward * _groundCheckRadius;
            Vector3 rayStart = center + dir;

            if (Physics.Raycast(rayStart, Vector3.down, _groundCheckDistance, _groundLayer))
            {
                Debug.DrawRay(rayStart, Vector3.down * _groundCheckDistance, Color.green);
                return true;
            }
            Debug.DrawRay(rayStart, Vector3.down * _groundCheckDistance, Color.red);
        }
        return false;
    }

    private void UpdateSpeedByWeight(int weightLevel)
    {
        if (weightLevel >= 0 && weightLevel < speedModifiers.Length)
            _moveSpeed = _baseMoveSpeed * speedModifiers[weightLevel];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + _groundCheckOffset;
        Gizmos.DrawWireSphere(center, _groundCheckRadius);
        Gizmos.DrawLine(center, center + Vector3.down * _groundCheckDistance);
    }

    public void Stasis(float duration)
    {
        _isInStasis = true;
        Invoke(nameof(UnStasis), duration);
    }

    public void UnStasis()
    {
        _isInStasis = false;
    }
}