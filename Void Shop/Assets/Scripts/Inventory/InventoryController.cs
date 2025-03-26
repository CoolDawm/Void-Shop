public class InventoryController
{
    private InventoryModel _model;
    private InventoryView _view;

    public InventoryController(InventoryModel model, InventoryView view)
    {
        _model = model;
        _view = view;

        _view.Initialize(this);
        _model.OnActiveItemChanged += _view.UpdateActiveItem;

        _model.SetActiveSlot(_model.SelectedSlotIndex);
    }

    public bool AddItem(Item item)
    {
        return _model.AddItem(item);
    }

    public void RemoveItem(int index)
    {
        _model.RemoveItem(index);
    }


    public void SetActiveSlot(int slotIndex)
    {
        _model.SetActiveSlot(slotIndex);
    }

    public void DropActiveItem()
    {
        if (_model.GetItem(_model.SelectedSlotIndex) != null)
        {
            _view.DropItem(GetActiveItem());
            _model.RemoveItem(_model.SelectedSlotIndex);
        }
    }

    public void DropAllItems()
    {
        _view.DropAllItems(GetAllItems());
        ClearInventory();
    }

    public Item GetItem(int index)
    {
        return _model.GetItem(index);
    }

    public Item GetActiveItem()
    {
        return _model.GetItem(_model.SelectedSlotIndex);
    }

    public Item[] GetAllItems()
    {
        return _model.GetItems();
    }

    public void ClearInventory()
    {
        _model.Clear();
    }
}