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

    public void AddToStorage(object itemType)
    {
        if (!storage.ContainsKey(itemType))
        {
            storage[itemType] = 0;
        }
        storage[itemType] += 1;
        count++;
    }

    public void RemoveFromStorage(object itemType)
    {
        if (storage.ContainsKey(itemType))
        {
            storage[itemType] -= 1;
            if (storage[itemType] <= 0)
            {
                storage.Remove(itemType);
            }
        }
    }
}