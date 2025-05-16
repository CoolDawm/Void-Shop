
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TutorialController tutorialController;
    [SerializeField]
    private PlayerMovement playerMovement;
    public float pickUpDistance = 2f;
    public LayerMask itemLayer;
    public System.Action<GameObject> onPickUpInput;

    [Header("Inventory Settings")]
    public int inventorySlotCount = 5;
    public int inventoryMaxWeight = 10;
    [Header("Tutorial Settings")]
   
    private Camera _playerCamera;
    private InventoryModel _inventoryModel;
    private InventoryView _inventoryView;
    private InventoryController _inventoryController;
    private PlayerUI _playerUI;
    private ShoppingList _shoppingList;
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        _playerCamera = Camera.main;
        _inventoryView = FindAnyObjectByType<InventoryView>();
        _inventoryModel = new InventoryModel(inventorySlotCount, inventoryMaxWeight);
        _inventoryController = new InventoryController(_inventoryModel, _inventoryView);
        _inventoryView.Initialize(_inventoryController);
        FindAnyObjectByType<PlayerUI>().inventoryModel = _inventoryModel;
        _playerUI = FindAnyObjectByType<PlayerUI>();
        InputEvents.OnSwitchSlot += HandleSwitchSlot;
        InputEvents.OnHotkey += HandleHotkey;
        _playerUI.inventoryModel = _inventoryModel;
        _shoppingList = FindAnyObjectByType<ShoppingList>();

        if (Instance == null) Instance = this;

        if (tutorialController != null && tutorialController.IsTutorialActive())
        {
            if (playerMovement == null)
            {
                playerMovement = FindAnyObjectByType<PlayerMovement>();
            }
            playerMovement.Stasis(float.MaxValue);
        }
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
                _shoppingList.UpdateUIList(_inventoryController.GetAllItems());
                break;
            case "e":
                PickUpItem();
                break;
            case "r":
                _inventoryController.DropActiveItem();
                _shoppingList.UpdateUIList(_inventoryController.GetAllItems());
                break;
        }
    }
    public void CompleteTutorial()
    {
        if (playerMovement != null)
        {
            playerMovement.UnStasis();
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
                    _shoppingList.UpdateUIList(_inventoryController.GetAllItems());
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
    public InventoryModel GetInventoryModel()
    {
        return _inventoryModel;
    }
    public Item[] GetPlayerItems()
    {
        return _inventoryController.GetAllItems();
    }
    public void EndGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }
}