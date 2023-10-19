using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public Dictionary<object, int> storage;
    public int count = 0;
    public UnityEvent onInventoryChanged;

    private void Start()
    {
        storage = new();
    }

    private void UpdateStorage(object itemType, int amount)
    {
        if(!storage.ContainsKey(itemType))
        {
            storage[itemType] = 0;
        }

        storage[itemType] += amount;

        if (storage[itemType] <= 0)
        {
            storage.Remove(itemType);
        }
    }

    public void AddToStorage(object itemType)
    {
        UpdateStorage(itemType, 1);
        onInventoryChanged?.Invoke();
    }

    public void RemoveFromStorage(object itemType)
    {
        UpdateStorage(itemType, -1);
        onInventoryChanged?.Invoke();
    }
}