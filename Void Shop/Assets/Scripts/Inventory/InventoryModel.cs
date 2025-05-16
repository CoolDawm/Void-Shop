using System;

[Serializable]
public class InventoryModel
{
    public int SlotCount { get; private set; }
    public int MaxWeight { get; private set; }
    public int CurrentWeight { get; private set; }
    public int SelectedSlotIndex { get; private set; }

    private Item[] _slots;
    private int _weightLevel = 0;

    public event Action<int> OnInventoryUpdated;
    public event Action<Item> OnActiveItemChanged;

    public InventoryModel(int slotCount, int maxWeight)
    {
        SlotCount = slotCount;
        MaxWeight = maxWeight;
        _slots = new Item[slotCount];
        SelectedSlotIndex = 0;
    }

    public int WeightLevel => _weightLevel;

    private void UpdateWeightLevel()
    {
        if (MaxWeight == 0) return;

        float fillPercentage = (float)CurrentWeight / MaxWeight;
        if (fillPercentage == 0)
        {
            _weightLevel = 0;
        }
        else if (fillPercentage >0 && fillPercentage <= 0.25f)
            _weightLevel = 1;
        else if (fillPercentage <= 0.5f)
            _weightLevel = 2;
        else if (fillPercentage <= 0.75f)
            _weightLevel = 3;
        else
            _weightLevel = 4;
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
            UpdateWeightLevel(); // Обновляем уровень веса
            OnInventoryUpdated?.Invoke(_weightLevel);
            OnActiveItemChanged?.Invoke(item);
            return true;
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == null)
            {
                _slots[i] = item;
                CurrentWeight += item.weight;
                UpdateWeightLevel(); // Обновляем уровень веса
                OnInventoryUpdated?.Invoke(_weightLevel);
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
            UpdateWeightLevel(); // Обновляем уровень веса
            OnInventoryUpdated?.Invoke(_weightLevel);
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
        UpdateWeightLevel(); // Обновляем уровень веса
        OnInventoryUpdated?.Invoke(_weightLevel);
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