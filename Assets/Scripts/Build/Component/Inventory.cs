using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class InventoryChangedEvent : UnityEvent<Dictionary<object, int>> { }

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public Dictionary<object, int> storage;
    public InventoryChangedEvent onInventoryChanged;

    private void Start()
    {
        storage = new();
    }

    protected virtual void UpdateStorage(ICollectable item, int amount)
    {

        object key = item.GetItemType();
        if (!storage.ContainsKey(key))
        {
            storage[key] = 0;
        }

        storage[key] += amount;

        if (storage[key] <= 0)
        {
            storage.Remove(key);
        }
        onInventoryChanged?.Invoke(storage);
    }

    public virtual void AddToStorage(ICollectable item)
    {
        UpdateStorage(item, 1);
    }

    public virtual void RemoveFromStorage(ICollectable item)
    {
        UpdateStorage(item, -1);
    }
}
