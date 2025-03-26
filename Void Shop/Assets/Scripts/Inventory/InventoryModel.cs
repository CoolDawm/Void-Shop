using System;

[Serializable]
public class InventoryModel
{
    public int SlotCount { get; private set; }
    public int MaxWeight { get; private set; }
    public int CurrentWeight { get; private set; }
    public int SelectedSlotIndex { get; private set; }

    private Item[] _slots;

    public event Action OnInventoryUpdated;
    public event Action<Item> OnActiveItemChanged;

    public InventoryModel(int slotCount, int maxWeight)
    {
        SlotCount = slotCount;
        MaxWeight = maxWeight;
        _slots = new Item[slotCount];
        SelectedSlotIndex = 0;
    }

    public Item[] GetItems()
    {
        return (Item[])_slots.Clone();
    }

    public Item GetItem(int index)
    {
        if (index >= 0 && index < _slots.Length)
            return _slots[index];
        return null;
    }

    public bool AddItem(Item item)
    {
        if (CurrentWeight + item.weight > MaxWeight)
            return false;

        if (SelectedSlotIndex >= 0 && SelectedSlotIndex < _slots.Length && _slots[SelectedSlotIndex] == null)
        {
            _slots[SelectedSlotIndex] = item;
            CurrentWeight += item.weight;
            OnInventoryUpdated?.Invoke();
            OnActiveItemChanged?.Invoke(item);
            return true;
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = item;
                CurrentWeight += item.weight;
                OnInventoryUpdated?.Invoke();
                OnActiveItemChanged?.Invoke(item);
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < _slots.Length && _slots[index] != null)
        {
            CurrentWeight -= _slots[index].weight;
            _slots[index] = null;
            OnInventoryUpdated?.Invoke();
            if (index == SelectedSlotIndex)
            {
                OnActiveItemChanged?.Invoke(null);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = null;
        }
        CurrentWeight = 0;
        SelectedSlotIndex = -1;
        OnInventoryUpdated?.Invoke();
        OnActiveItemChanged?.Invoke(null);
    }

    public void SetActiveSlot(int slotIndex)
    {
        if (slotIndex >= -1 && slotIndex < _slots.Length)
        {
            SelectedSlotIndex = slotIndex;
            OnActiveItemChanged?.Invoke(_slots[slotIndex]);
        }
    }
    public Item[] GetItemsList()
    {
        return _slots;
    }
}